using Newtonsoft.Json;
using RocketRP.DataTypes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RocketRP.Actors.Core
{
	public abstract class Object
	{
		private static readonly string NONE = "None";
		private static readonly string BOOL_PROPERTY = "BoolProperty";
		private static readonly string INT_PROPERTY = "IntProperty";
		private static readonly string QWORD_PROPERTY = "QWordProperty";
		private static readonly string FLOAT_PROPERTY = "FloatProperty";
		private static readonly string STR_PROPERTY = "StrProperty";
		private static readonly string NAME_PROPERTY = "NameProperty";
		private static readonly string BYTE_PROPERTY = "ByteProperty";
		private static readonly string OBJECT_PROPERTY = "ObjectProperty";
		private static readonly string STRUCT_PROPERTY = "StructProperty";
		private static readonly string ARRAY_PROPERTY = "ArrayProperty";

		[JsonProperty(Order = -2)]
		protected string ObjectTypeName { get => GetType().Name; }

		private static int RecalculateValueSizeFromType(int valueSize, object? value, string? type)
		{
			if (value is bool) return 0;
			else if (value is byte) return sizeof(byte);
			else if (value is Enum || type == STRUCT_PROPERTY) return valueSize - (4 + value!.GetType().Name.Length + 1);
			return valueSize;
		}

		public static void Deserialize(object obj, BinaryReader br, IFileVersionInfo versionInfo)
		{
			while (true)
			{
				var propName = br.ReadString()!;
				if (propName == NONE) break;
				DeserializeProperty(obj, br, versionInfo, propName);
			}
		}

		public static void DeserializeProperty(object obj, BinaryReader br, IFileVersionInfo versionInfo, string propName)
		{
			var type = br.ReadString()!;
			var valueLength = br.ReadInt32();
			var valueIndex = br.ReadInt32();

			var propertyInfo = obj.GetType().GetProperty(propName, BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public);
			if (propertyInfo == null) throw new Exception($"Property {propName} not found in {obj.GetType().Name}");
			var propertyType = propertyInfo.PropertyType;
			propertyType = Nullable.GetUnderlyingType(propertyType) ?? propertyType;

			object? value = null;
			var curPos = br.BaseStream.Position;
			int valueSize = 0;

			if (!propertyType.IsArray)
			{
				if (valueIndex != 0) throw new IndexOutOfRangeException($"Trying to access {propName} of type {propertyType.Name} as Array");
				value = DeserializePropertyValue(br, versionInfo, propertyType, type);
				propertyInfo.SetValue(obj, value);

				valueSize = RecalculateValueSizeFromType((int)(br.BaseStream.Position - curPos), value, type);
				if (valueSize != valueLength && type != STR_PROPERTY)
					throw new InternalBufferOverflowException($"Size of read value({valueSize}) doesn't match the expected size {valueLength}");
				return;
			}

			Array? arr;
			var arrType = propertyType.GetElementType()!;
			var arrItemType = Nullable.GetUnderlyingType(arrType) ?? arrType;
			if (type != ARRAY_PROPERTY) //Fixed Size Array
			{
				arr = (Array?)propertyInfo.GetValue(obj);
				if (arr is null)
				{
					arr = Array.CreateInstance(arrType, propertyInfo.GetCustomAttribute<FixedArraySize>()!.Size);
					propertyInfo.SetValue(obj, arr);
				}
				value = DeserializePropertyValue(br, versionInfo, arrItemType, type);
				arr.SetValue(value, valueIndex);

				valueSize = RecalculateValueSizeFromType((int)(br.BaseStream.Position - curPos), value, type);
				if (valueSize != valueLength)
					throw new InternalBufferOverflowException($"Size of read value({valueSize}) doesn't match the expected size {valueLength}");
				return;
			}

			//Dynamic Size Array
			if (valueIndex != 0) throw new IndexOutOfRangeException($"Dynamic Size Array {propName} of type {arrType.Name} should not be accesed as Array");

			var arrayLength = br.ReadInt32();
			arr = Array.CreateInstance(arrType, arrayLength);
			propertyInfo.SetValue(obj, arr);

			for (int vi = 0; vi < arrayLength; vi++)
			{
				value = DeserializePropertyValue(br, versionInfo, arrItemType, type);
				arr.SetValue(value, vi);
			}

			valueSize = (int)(br.BaseStream.Position - curPos);
			if (valueSize != valueLength) throw new InternalBufferOverflowException($"Size of read value({valueSize}) doesn't match the expected size {valueLength}");
		}

		public static object? DeserializePropertyValue(BinaryReader br, IFileVersionInfo versionInfo, Type propertyType, string type)
		{
			if (propertyType == typeof(bool))
			{
				if (type != BOOL_PROPERTY && type != ARRAY_PROPERTY) throw new InvalidDataException($"Expected type {BOOL_PROPERTY} for {propertyType.Name} but got {type}");
				return br.ReadBoolean();
			}
			else if (propertyType == typeof(int) || propertyType == typeof(uint))
			{
				if (type != INT_PROPERTY && type != ARRAY_PROPERTY) throw new InvalidDataException($"Expected type {INT_PROPERTY} for {propertyType.Name} but got {type}");
				return br.ReadInt32();
			}
			else if (propertyType == typeof(long) || propertyType == typeof(ulong))
			{
				if (type != QWORD_PROPERTY && type != ARRAY_PROPERTY) throw new InvalidDataException($"Expected type {QWORD_PROPERTY} for {propertyType.Name} but got {type}");
				return br.ReadUInt64();
			}
			else if (propertyType == typeof(float))
			{
				if (type != FLOAT_PROPERTY && type != ARRAY_PROPERTY) throw new InvalidDataException($"Expected type {FLOAT_PROPERTY} for {propertyType.Name} but got {type}");
				return br.ReadSingle();
			}
			else if (propertyType == typeof(string))
			{
				if (type != STR_PROPERTY && type != ARRAY_PROPERTY) throw new InvalidDataException($"Expected type {STR_PROPERTY} for {propertyType.Name} but got {type}");
				return br.ReadString();
			}
			else if (propertyType == typeof(Name))
			{
				if (type != NAME_PROPERTY && type != ARRAY_PROPERTY) throw new InvalidDataException($"Expected type {NAME_PROPERTY} for {propertyType.Name} but got {type}");
				return Name.Deserialize(br);
			}
			else if (propertyType == typeof(byte) || propertyType.IsEnum)
			{
				if (type == INT_PROPERTY) return Enum.ToObject(propertyType, br.ReadUInt32());
				if (type != BYTE_PROPERTY && type != ARRAY_PROPERTY) throw new InvalidDataException($"Expected type {BYTE_PROPERTY} for {propertyType.Name} but got {type}");
				var typeName = br.ReadString();
				if (typeName == NONE) return br.ReadByte();
				return Enum.Parse(propertyType, br.ReadString()!);
			}
			else if (propertyType.GetInterface("IObjectTarget") == typeof(IObjectTarget))
			{
				if (type != OBJECT_PROPERTY && type != ARRAY_PROPERTY) throw new InvalidDataException($"Expected type {OBJECT_PROPERTY} for {propertyType.Name} but got {type}");
				return ObjectTarget<ClassObject>.Deserialize(br);
			}
			else
			{
				if (type != STRUCT_PROPERTY && type != ARRAY_PROPERTY) throw new InvalidDataException($"Expected type {STRUCT_PROPERTY} for {propertyType.Name} but got {type}");
				var typeName = type == ARRAY_PROPERTY ? propertyType.Name : br.ReadString();
				if (typeName != propertyType.Name) throw new InvalidDataException($"Expected type {propertyType.Name} for {propertyType.Name} but got {typeName}");
				var value = Activator.CreateInstance(propertyType);
				if (value is null) throw new MissingMethodException($"{propertyType.Name} does not have a parameterless constructor");
				if (value is ISpecialSerialized specialValue) specialValue.Deserialize(br, versionInfo);
				else Deserialize(value, br, versionInfo);
				return value;
			}
		}

		public static void Serialize(object obj, BinaryWriter bw, IFileVersionInfo versionInfo)
		{
			foreach (var property in obj.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public))
			{
				SerializeProperty(obj, bw, versionInfo, property);
			}
			bw.Write(NONE);
		}

		private static void SerializeProperty(object obj, BinaryWriter bw, IFileVersionInfo versionInfo, PropertyInfo propertyInfo)
		{
			var value = propertyInfo.GetValue(obj);
			if (value == null) return;

			var propertyType = propertyInfo.PropertyType;
			propertyType = Nullable.GetUnderlyingType(propertyType) ?? propertyType;

			var pbw = new BinaryWriter(new MemoryStream());
			string type;
			int numBytes;
			int valueSize;

			if (!propertyType.IsArray)
			{
				bw.Write(propertyInfo.Name);
				SerializePropertyValue(pbw, versionInfo, value, false, out type);
				numBytes = (int)pbw.BaseStream.Position;
				valueSize = RecalculateValueSizeFromType(numBytes, value, type);
				SerializePropertyValueHeader(bw, type, valueSize, 0);
				pbw.BaseStream.Position = 0;
				pbw.BaseStream.CopyTo(bw.BaseStream, numBytes);
				return;
			}

			var arr = (Array)value;
			if (arr.Length <= 0) throw new NullReferenceException($"Array size needs to be at least 1");
			var fixedSize = propertyInfo.GetCustomAttribute<FixedArraySize>()?.Size;

			if (fixedSize is not null)  //Fixed Size Array
			{
				for (int valueIndex = 0; valueIndex < arr.Length; valueIndex++)
				{
					value = arr.GetValue(valueIndex);
					if (value is null) continue;

					bw.Write(propertyInfo.Name);
					SerializePropertyValue(pbw, versionInfo, value, false, out type);
					numBytes = (int)pbw.BaseStream.Position;
					valueSize = RecalculateValueSizeFromType(numBytes, value, type);
					SerializePropertyValueHeader(bw, type, valueSize, valueIndex);
					pbw.BaseStream.Position = 0;
					pbw.BaseStream.CopyTo(bw.BaseStream, numBytes);
					pbw.BaseStream.Position = 0;
				}
				return;
			}

			//Dynamic Size Array
			bw.Write(propertyInfo.Name);
			pbw.Write(arr.Length);
			for (int vi = 0; vi < arr.Length; vi++)
			{
				value = arr.GetValue(vi);
				if (value is null) throw new NullReferenceException($"Array value at index {vi} was null");
				SerializePropertyValue(pbw, versionInfo, value, true, out type);
			}

			numBytes = (int)pbw.BaseStream.Position;
			SerializePropertyValueHeader(bw, ARRAY_PROPERTY, numBytes, 0);
			pbw.BaseStream.Position = 0;
			pbw.BaseStream.CopyTo(bw.BaseStream, numBytes);
		}

		private static void SerializePropertyValueHeader(BinaryWriter bw, string type, int valueLength, int valueIndex)
		{
			bw.Write(type);
			bw.Write(valueLength);
			bw.Write(valueIndex);
		}

		public static void SerializePropertyValue(BinaryWriter bw, IFileVersionInfo versionInfo, object value, bool isArrayProp, out string type)
		{
			ArgumentNullException.ThrowIfNull(value);

			if (value is bool boolvalue)
			{
				type = BOOL_PROPERTY;
				bw.Write(boolvalue);
				return;
			}
			else if (value is int intvalue)
			{
				type = INT_PROPERTY;
				bw.Write(intvalue);
				return;
			}
			else if (value is uint uintvalue)
			{
				type = INT_PROPERTY;
				bw.Write(uintvalue);
				return;
			}
			else if (value is long longvalue)
			{
				type = QWORD_PROPERTY;
				bw.Write(longvalue);
				return;
			}
			else if (value is ulong ulongvalue)
			{
				type = QWORD_PROPERTY;
				bw.Write(ulongvalue);
				return;
			}
			else if (value is float floatvalue)
			{
				type = FLOAT_PROPERTY;
				bw.Write(floatvalue);
				return;
			}
			else if (value is string stringvalue)
			{
				type = STR_PROPERTY;
				bw.Write(stringvalue);
				return;
			}
			else if (value is Name namevalue)
			{
				type = NAME_PROPERTY;
				namevalue.Serialize(bw);
				return;
			}
			else if (value is byte bytevalue)
			{
				type = BYTE_PROPERTY;
				bw.Write(NONE);
				bw.Write(bytevalue);
				return;
			}
			else if (value is Enum enumvalue)
			{
				if (enumvalue.GetType().GetEnumUnderlyingType() == typeof(uint))
				{
					type = INT_PROPERTY;
					bw.Write(Convert.ToUInt32(enumvalue));
					return;
				}
				type = BYTE_PROPERTY;
				bw.Write(value.GetType().Name);
				bw.Write(enumvalue.ToString());
				return;
			}
			else if (value is ObjectTarget<ClassObject> objectvalue)
			{
				type = OBJECT_PROPERTY;
				objectvalue.Serialize(bw);
				return;
			}
			else
			{
				type = STRUCT_PROPERTY;
				if (!isArrayProp) bw.Write(value.GetType().Name);
				if (value is ISpecialSerialized specialValue) specialValue.Serialize(bw, versionInfo);
				else Serialize(value, bw, versionInfo);
				return;
			}
		}
	}
}
