using RocketRP.Actors.TAGame;
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
			var propName = "".Deserialize(br);

			if (propName == "None") return true;

			var propertyInfo = obj.GetType().GetProperty(propName, BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public);
			if (propertyInfo == null) throw new Exception($"Field {propName} not found in {obj.GetType().Name}");
			var propertyType = propertyInfo.PropertyType;
			if (Nullable.GetUnderlyingType(propertyType) != null) propertyType = propertyType.GenericTypeArguments[0];

			var type = "".Deserialize(br);
			var valueLength = br.ReadInt64();

			if (propertyType == typeof(bool))
			{
				if (type != "BoolProperty") throw new InvalidDataException($"Expected type BoolProperty for {obj.GetType().FullName}.{propName} but got {type}");
				propertyInfo.SetValue(obj, br.ReadBoolean());
			}
			else if (propertyType == typeof(int) || propertyType == typeof(uint))
			{
				if (type != "IntProperty") throw new InvalidDataException($"Expected type IntProperty for {obj.GetType().FullName}.{propName} but got {type}");
				propertyInfo.SetValue(obj, br.ReadInt32());
			}
			else if (propertyType == typeof(long) || propertyType == typeof(ulong))
			{
				//if(type == "IntProperty") propertyInfo.SetValue(obj, (ulong)br.ReadUInt32());	// Patch for old versions of RocketRP made Replays
				//else propertyInfo.SetValue(obj, br.ReadUInt64());								// Remove the 2 lines below and uncomment these 2 lines
				if (type != "QWordProperty") throw new InvalidDataException($"Expected type QwordProperty for {obj.GetType().FullName}.{propName} but got {type}");
				propertyInfo.SetValue(obj, br.ReadUInt64());
			}
			else if (propertyType == typeof(float))
			{
				if (type != "FloatProperty") throw new InvalidDataException($"Expected type FloatProperty for {obj.GetType().FullName}.{propName} but got {type}");
				propertyInfo.SetValue(obj, br.ReadSingle());
			}
			else if (propertyType == typeof(string))
			{
				if (type != "StrProperty") throw new InvalidDataException($"Expected type StrProperty for {obj.GetType().FullName}.{propName} but got {type}");
				propertyInfo.SetValue(obj, "".Deserialize(br));
			}
			else if (propertyType == typeof(Name))
			{
				if (type != "NameProperty") throw new InvalidDataException($"Expected type NameProperty for {obj.GetType().FullName}.{propName} but got {type}");	// Remove this line for patch
				propertyInfo.SetValue(obj, Name.Deserialize(br));
			}
			else if (propertyType.IsEnum)
			{
				var typeName = "".Deserialize(br);
				//if(typeName.Contains(".")) propertyInfo.SetValue(obj, Enum.Parse(propertyType, typeName.Split(".")[1]));	// Patch for old versions of RocketRP made Replays
				//else propertyInfo.SetValue(obj, Enum.Parse(propertyType, "".Deserialize(br)));							// Remove the 2 lines below and uncomment these 2 lines
				propertyInfo.SetValue(obj, Enum.Parse(propertyType, "".Deserialize(br)));
				if (type != "ByteProperty") throw new InvalidDataException($"Expected type ByteProperty for {obj.GetType().FullName}.{propName} but got {type}");
			}
			else if (propertyType == typeof(ObjectTarget))
			{
				if (type != "ObjectProperty") throw new InvalidDataException($"Expected type ObjectProperty for {obj.GetType().FullName}.{propName} but got {type}");
				propertyInfo.SetValue(obj, ObjectTarget.Deserialize(br));
			}
			else if (propertyType.GetInterface("IArrayProperty") == typeof(IArrayProperty))
			{
				if (type != "ArrayProperty") throw new InvalidDataException($"Expected type ArrayProperty for {obj.GetType().FullName}.{propName} but got {type}");
				var array = propertyType.GetMethod("Deserialize", new Type[] { typeof(BinaryReader), typeof(IFileVersionInfo) }).Invoke(null, new object[] { br, versionInfo });
				propertyInfo.SetValue(obj, array);
			}
			else
			{
				if (type != "StructProperty") throw new InvalidDataException($"Expected type StructProperty for {obj.GetType().FullName}.{propName} but got {type}");
				if (propertyType == typeof(StructProperty))
				{
					propertyInfo.SetValue(obj, StructProperty.Deserialize(br, valueLength));
					return false;
				}
				var structType = "".Deserialize(br);
				var value = Activator.CreateInstance(propertyType);
				if (propertyType.GetInterface("ISpecialSerialized") == typeof(ISpecialSerialized)) propertyType.GetMethod("Deserialize", new Type[] { typeof(BinaryReader), typeof(IFileVersionInfo) }).Invoke(value, new object[] { br, versionInfo });
				else Deserialize(value, br, versionInfo);
				propertyInfo.SetValue(obj, value);
			}

			return false;
		}

		public static void Serialize(object obj, BinaryWriter bw, IFileVersionInfo versionInfo)
		{
			foreach (var property in obj.GetType().GetProperties())
			{
				SerializeProperty(obj, bw, versionInfo, property);
			}
			"None".Serialize(bw);
		}

		private static void SerializeProperty(object obj, BinaryWriter bw, IFileVersionInfo versionInfo, PropertyInfo propertyInfo)
		{
			var value = propertyInfo.GetValue(obj);
			if (value == null) return;
			propertyInfo.Name.Serialize(bw);
			var propertyType = propertyInfo.PropertyType;
			if (Nullable.GetUnderlyingType(propertyType) != null) propertyType = propertyType.GenericTypeArguments[0];

			if (value is bool boolvalue)
			{
				"BoolProperty".Serialize(bw);
				bw.Write(0L);
				bw.Write(boolvalue);
			}
			else if (value is int intvalue)
			{
				"IntProperty".Serialize(bw);
				bw.Write(4L);
				bw.Write(intvalue);
			}
			else if (value is uint uintvalue)
			{
				"IntProperty".Serialize(bw);
				bw.Write(4L);
				bw.Write(uintvalue);
			}
			else if (value is long longvalue)
			{
				"QWordProperty".Serialize(bw);
				bw.Write(8L);
				bw.Write(longvalue);
			}
			else if (value is ulong ulongvalue)
			{
				"QWordProperty".Serialize(bw);
				bw.Write(8L);
				bw.Write(ulongvalue);
			}
			else if (value is float floatvalue)
			{
				"FloatProperty".Serialize(bw);
				bw.Write(4L);
				bw.Write(floatvalue);
			}
			else if (value is string stringvalue)
			{
				"StrProperty".Serialize(bw);
				bw.Write(4L + stringvalue.Length + 1);
				stringvalue.Serialize(bw);
			}
			else if (value is Name namevalue)
			{
				"NameProperty".Serialize(bw);
				bw.Write(4L + namevalue.Value.Length + 1);
				namevalue.Serialize(bw);
			}
			else if (value is Enum bytevalue)
			{
				"ByteProperty".Serialize(bw);
				bw.Write(4L + bytevalue.ToString().Length + 1);
				value.GetType().Name.Serialize(bw);
				bytevalue.ToString().Serialize(bw);
			}
			else if (value is ObjectTarget objectvalue)
			{
				"ObjectProperty".Serialize(bw);
				bw.Write(4L);
				objectvalue.Serialize(bw);
			}
			else if (value is IArrayProperty listvalue)
			{
				"ArrayProperty".Serialize(bw);
				var lengthPos = bw.BaseStream.Position;
				bw.Write(0L);
				listvalue.Serialize(bw, versionInfo);

				var length = bw.BaseStream.Position - lengthPos - sizeof(long);
				bw.BaseStream.Seek(lengthPos, SeekOrigin.Begin);
				bw.Write(length);
				bw.BaseStream.Seek(length, SeekOrigin.Current);
			}
			else
			{
				"StructProperty".Serialize(bw);
				var lengthPos = bw.BaseStream.Position;
				bw.Write(0L);
				int propTypeNameLength;

				if (value is StructProperty structvalue)
				{
					propTypeNameLength = structvalue.Type.Length;
					structvalue.Serialize(bw);
				}
				else
				{
					propTypeNameLength = propertyType.Name.Length;
					propertyType.Name.Serialize(bw);
					if (propertyType.GetInterface("ISpecialSerialized") == typeof(ISpecialSerialized)) propertyType.GetMethod("Serialize", new Type[] { typeof(BinaryWriter), typeof(IFileVersionInfo) }).Invoke(value, new object[] { bw, versionInfo });
					else Serialize(value, bw, versionInfo);
				}

				var length = bw.BaseStream.Position - lengthPos - sizeof(long);
				bw.BaseStream.Seek(lengthPos, SeekOrigin.Begin);
				bw.Write(length - (4 + propTypeNameLength + 1));
				bw.BaseStream.Seek(length, SeekOrigin.Current);
			}
		}
	}
}
