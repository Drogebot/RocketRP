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
		[JsonProperty(Order = -2)]
		protected string ObjectTypeName { get => GetType().Name; }

		public static void Deserialize(object obj, BinaryReader br, IFileVersionInfo versionInfo)
		{
			while (true)
			{
				var isLast = DeserializeProperty(obj, br, versionInfo);
				if (isLast) break;
			}
		}

		public static bool DeserializeProperty(object obj, BinaryReader br, IFileVersionInfo versionInfo)
		{
			var propName = br.ReadString();
			if (propName == "None") return true;

			var propertyInfo = obj.GetType().GetProperty(propName, BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public);
			if (propertyInfo == null) throw new Exception($"Field {propName} not found in {obj.GetType().Name}");
			var propertyType = propertyInfo.PropertyType;
			if (Nullable.GetUnderlyingType(propertyType) != null) propertyType = propertyType.GenericTypeArguments[0];

			if(propertyType.IsArray)
			{
				var existingArray = (Array?)propertyInfo.GetValue(obj);
				var newArray = (Array)Activator.CreateInstance(propertyType, new object[] { existingArray?.Length + 1 ?? 1 });
				existingArray?.CopyTo(newArray, 0);
				var value = DeserializePropertyValue(obj, propName, propertyType.GetElementType(), br, versionInfo);
				newArray.SetValue(value, newArray.Length - 1);
				propertyInfo.SetValue(obj, newArray);
			}
			else
			{
				propertyInfo.SetValue(obj, DeserializePropertyValue(obj, propName, propertyType, br, versionInfo));
			}

			return false;
		}

		public static object DeserializePropertyValue(object obj, string propName, Type propertyType, BinaryReader br, IFileVersionInfo versionInfo)
		{
			var type = br.ReadString();
			var valueLength = br.ReadInt32();
			var valueIndex = br.ReadInt32();

			if (propertyType == typeof(bool))
			{
				if (type != "BoolProperty") throw new InvalidDataException($"Expected type BoolProperty for {obj.GetType().FullName}.{propName} but got {type}");
				return br.ReadBoolean();
			}
			else if (propertyType == typeof(int) || propertyType == typeof(uint))
			{
				if (type != "IntProperty") throw new InvalidDataException($"Expected type IntProperty for {obj.GetType().FullName}.{propName} but got {type}");
				return br.ReadInt32();
			}
			else if (propertyType == typeof(long) || propertyType == typeof(ulong))
			{
				//if(type == "IntProperty") propertyInfo.SetValue(obj, (ulong)br.ReadUInt32());	// Patch for old versions of RocketRP made Replays
				//else propertyInfo.SetValue(obj, br.ReadUInt64());								// Remove the 2 lines below and uncomment these 2 lines
				if (type != "QWordProperty") throw new InvalidDataException($"Expected type QwordProperty for {obj.GetType().FullName}.{propName} but got {type}");
				return br.ReadUInt64();
			}
			else if (propertyType == typeof(float))
			{
				if (type != "FloatProperty") throw new InvalidDataException($"Expected type FloatProperty for {obj.GetType().FullName}.{propName} but got {type}");
				return br.ReadSingle();
			}
			else if (propertyType == typeof(string))
			{
				if (type != "StrProperty") throw new InvalidDataException($"Expected type StrProperty for {obj.GetType().FullName}.{propName} but got {type}");
				return br.ReadString();
			}
			else if (propertyType == typeof(Name))
			{
				if (type != "NameProperty") throw new InvalidDataException($"Expected type NameProperty for {obj.GetType().FullName}.{propName} but got {type}");   // Remove this line for patch
				return Name.Deserialize(br);
			}
			else if (propertyType == typeof(byte) || propertyType.IsEnum)
			{
				if (type == "IntProperty")
				{
					return Enum.ToObject(propertyType, br.ReadUInt32());
				}
				if (type != "ByteProperty") throw new InvalidDataException($"Expected type ByteProperty for {obj.GetType().FullName}.{propName} but got {type}");
				var typeName = br.ReadString();
				if (typeName == "None") return br.ReadByte();
				//if(typeName.Contains(".")) propertyInfo.SetValue(obj, Enum.Parse(propertyType, typeName.Split(".")[1]));	// Patch for old versions of RocketRP made Replays
				//else propertyInfo.SetValue(obj, Enum.Parse(propertyType, "".Deserialize(br)));							// Remove the 2 lines below and uncomment these 2 lines
				return Enum.Parse(propertyType, br.ReadString());
			}
			else if (propertyType.GetInterface("IObjectTarget") == typeof(IObjectTarget))
			{
				if (type != "ObjectProperty") throw new InvalidDataException($"Expected type ObjectProperty for {obj.GetType().FullName}.{propName} but got {type}");
				var objectTarget = propertyType.GetMethod("Deserialize", new Type[] { typeof(BinaryReader) }).Invoke(null, new object[] { br });
				return objectTarget;
			}
			else if (propertyType.GetInterface("IArrayProperty") == typeof(IArrayProperty))
			{
				if(type != "ArrayProperty") throw new InvalidDataException($"Expected type ArrayProperty for {obj.GetType().FullName}.{propName} but got {type}");
				var array = propertyType.GetMethod("Deserialize", new Type[] { typeof(BinaryReader), typeof(IFileVersionInfo) }).Invoke(null, new object[] { br, versionInfo });
				return array;
			}
			else
			{
				if (type != "StructProperty") throw new InvalidDataException($"Expected type StructProperty for {obj.GetType().FullName}.{propName} but got {type}");
				if (propertyType == typeof(StructProperty))
				{
					return StructProperty.Deserialize(br, valueLength);
				}
				var structType = br.ReadString();
				var value = Activator.CreateInstance(propertyType);
				if (value is ISpecialSerialized specialValue) specialValue.Deserialize(br, versionInfo);
				else Deserialize(value, br, versionInfo);
				return value;
			}

			// You can't reach this point
			throw new Exception($"Unhandled propertyType {propertyType}");
		}

		public static void Serialize(object obj, BinaryWriter bw, IFileVersionInfo versionInfo)
		{
			foreach (var property in obj.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public))
			{
				SerializeProperty(obj, bw, versionInfo, property);
			}
			bw.Write("None");
		}

		private static void SerializeProperty(object obj, BinaryWriter bw, IFileVersionInfo versionInfo, PropertyInfo propertyInfo)
		{
			var value = propertyInfo.GetValue(obj);
			if (value == null) return;
			var propertyType = propertyInfo.PropertyType;
			if (Nullable.GetUnderlyingType(propertyType) != null) propertyType = propertyType.GenericTypeArguments[0];

			if (propertyType.IsArray)
			{
				var valueIndex = 0;
                foreach (var item in (Array)value)
				{
					bw.Write(propertyInfo.Name);
					SerializePropertyValue(obj, propertyType, item, valueIndex++, bw, versionInfo);
				}
			}
			else
			{
				bw.Write(propertyInfo.Name);
				SerializePropertyValue(obj, propertyType, value, 0, bw, versionInfo);
			}
		}

		public static void SerializePropertyValue(object obj, Type propertyType, object value, int valueIndex, BinaryWriter bw, IFileVersionInfo versionInfo)
		{
			if(value is null)
			{
				// This shouldn't be possible because we're checking for null values in SerializeProperty
				throw new ArgumentNullException(nameof(value));
			}
			else if (value is bool boolvalue)
			{
				bw.Write("BoolProperty");
				bw.Write(0);
				bw.Write(valueIndex);
				bw.Write(boolvalue);
			}
			else if (value is int intvalue)
			{
				bw.Write("IntProperty");
				bw.Write(4);
				bw.Write(valueIndex);
				bw.Write(intvalue);
			}
			else if (value is uint uintvalue)
			{
				bw.Write("IntProperty");
				bw.Write(4);
				bw.Write(valueIndex);
				bw.Write(uintvalue);
			}
			else if (value is long longvalue)
			{
				bw.Write("QWordProperty");
				bw.Write(8);
				bw.Write(valueIndex);
				bw.Write(longvalue);
			}
			else if (value is ulong ulongvalue)
			{
				bw.Write("QWordProperty");
				bw.Write(8);
				bw.Write(valueIndex);
				bw.Write(ulongvalue);
			}
			else if (value is float floatvalue)
			{
				bw.Write("FloatProperty");
				bw.Write(4);
				bw.Write(valueIndex);
				bw.Write(floatvalue);
			}
			else if (value is string stringvalue)
			{
				bw.Write("StrProperty");
				if(stringvalue != String.Empty) bw.Write(4 + (stringvalue.Length + 1) * (stringvalue.Any(c => c > 255) ? 2 : 1));
				else bw.Write(4);
				bw.Write(valueIndex);
				bw.Write(stringvalue);
			}
			else if (value is Name namevalue)
			{
				bw.Write("NameProperty");
				bw.Write(4 + namevalue.Value.Length + 1);
				bw.Write(valueIndex);
				namevalue.Serialize(bw);
			}
			else if(value is byte bytevalue)
			{
				bw.Write("ByteProperty");
				bw.Write(1);
				bw.Write(valueIndex);
				bw.Write("None");
				bw.Write(bytevalue);
			}
			else if (value is Enum enumvalue)
			{
				if(enumvalue.GetType().GetEnumUnderlyingType() == typeof(uint))
				{
					bw.Write("IntProperty");
					bw.Write(4);
					bw.Write(valueIndex);
					bw.Write(Convert.ToUInt32(enumvalue));
					return;
				}
				bw.Write("ByteProperty");
				bw.Write(4 + enumvalue.ToString().Length + 1);
				bw.Write(valueIndex);
				bw.Write(value.GetType().Name);
				bw.Write(enumvalue.ToString());
			}
			else if (value is ObjectTarget<ClassObject> objectvalue)
			{
				bw.Write("ObjectProperty");
				bw.Write(4);
				bw.Write(valueIndex);
				objectvalue.Serialize(bw);
			}
			else if (value is IArrayProperty listvalue)
			{
				bw.Write("ArrayProperty");
				var lengthPos = (int)bw.BaseStream.Position;
				bw.Write(0);
				bw.Write(valueIndex);
				listvalue.Serialize(bw, versionInfo);

				var length = (int)bw.BaseStream.Position - lengthPos - 4 - 4;
				bw.BaseStream.Seek(lengthPos, SeekOrigin.Begin);
				bw.Write(length);
				bw.BaseStream.Seek(length + 4, SeekOrigin.Current);
			}
			else
			{
				bw.Write("StructProperty");
				var lengthPos = (int)bw.BaseStream.Position;
				bw.Write(0);
				bw.Write(valueIndex);
				int propTypeNameLength;

				if (value is StructProperty structvalue)
				{
					propTypeNameLength = structvalue.Type.Length;
					structvalue.Serialize(bw);
				}
				else
				{
					propTypeNameLength = value.GetType().Name.Length;
					bw.Write(value.GetType().Name);
					if (value is ISpecialSerialized specialValue) specialValue.Serialize(bw, versionInfo);
					else Serialize(value, bw, versionInfo);
				}
				var length = (int)bw.BaseStream.Position - lengthPos - 4 - 4;
				bw.BaseStream.Seek(lengthPos, SeekOrigin.Begin);
				bw.Write(length - (4 + propTypeNameLength + 1));
				bw.BaseStream.Seek(length + 4, SeekOrigin.Current);
			}
		}
	}
}
