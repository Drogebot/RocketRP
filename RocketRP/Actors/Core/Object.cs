using RocketRP.Actors.TAGame;
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
		public static void Deserialize(object obj, BinaryReader br)
		{
			while (true)
			{
				var isLast = DeserializeProperty(obj, br);
				if (isLast) break;
			}
		}

		public static bool DeserializeProperty(object obj, BinaryReader br)
		{
			var propName = "".Deserialize(br);

			if (propName == "None") return true;

			var propertyInfo = obj.GetType().GetProperty(propName, BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public);
			if (propertyInfo == null) throw new Exception($"Field {propName} not found in {obj.GetType().Name}");

			var type = "".Deserialize(br);
			var valueLength = br.ReadInt64();

			switch (type)
			{
				case "BoolProperty":
					propertyInfo.SetValue(obj, BoolProperty.Deserialize(br));
					break;
				case "IntProperty":
					propertyInfo.SetValue(obj, IntProperty.Deserialize(br));
					break;
				case "QWordProperty":
					propertyInfo.SetValue(obj, QWordProperty.Deserialize(br));
					break;
				case "FloatProperty":
					propertyInfo.SetValue(obj, FloatProperty.Deserialize(br));
					break;
				case "StrProperty":
					propertyInfo.SetValue(obj, StrProperty.Deserialize(br));
					break;
				case "NameProperty":
					propertyInfo.SetValue(obj, NameProperty.Deserialize(br));
					break;
				case "ByteProperty":
					propertyInfo.SetValue(obj, ByteProperty.Deserialize(br));
					break;
				case "StructProperty":
					propertyInfo.SetValue(obj, StructProperty.Deserialize(br, valueLength));
					break;
				case "ObjectProperty":
					propertyInfo.SetValue(obj, ObjectProperty.Deserialize(br));
					break;
				case "ArrayProperty":
					var arrayLength = br.ReadInt32();
					var isBasicProperty = propertyInfo.PropertyType.GetGenericArguments()[0].IsSubclassOf(typeof(Property));
					propertyInfo.SetValue(obj, Activator.CreateInstance(propertyInfo.PropertyType));
					for (int i = 0; i < arrayLength; i++)
					{
						if (!isBasicProperty)
						{
							var value = Activator.CreateInstance(propertyInfo.PropertyType.GetGenericArguments()[0]);
							Deserialize(value, br);
							((IList)propertyInfo.GetValue(obj)).Add(value);
						}
						else
						{
							var value = GetBasicPropertyValue(br, propertyInfo.PropertyType.GetGenericArguments()[0], valueLength);
							((IList)propertyInfo.GetValue(obj)).Add(value);
						}
					}
					break;
				default:
					throw new Exception("Unknown property type: " + type);
			}

			return false;
		}

		private static object GetBasicPropertyValue(BinaryReader br, Type propertyType, long valueLength)
		{
			if (propertyType == typeof(BoolProperty)) return BoolProperty.Deserialize(br);
			else if (propertyType == typeof(IntProperty)) return IntProperty.Deserialize(br);
			else if (propertyType == typeof(QWordProperty)) return QWordProperty.Deserialize(br);
			else if (propertyType == typeof(FloatProperty)) return FloatProperty.Deserialize(br);
			else if (propertyType == typeof(StrProperty)) return StrProperty.Deserialize(br);
			else if (propertyType == typeof(NameProperty)) return NameProperty.Deserialize(br);
			else if (propertyType == typeof(ByteProperty)) return ByteProperty.Deserialize(br);
			else if (propertyType == typeof(StructProperty)) return StructProperty.Deserialize(br, valueLength);
			else if (propertyType == typeof(ObjectProperty)) return ObjectProperty.Deserialize(br);

			throw new Exception("Unsupported property type: " + propertyType);
		}

		public static void Serialize(object obj, BinaryWriter bw)
		{
			foreach (var property in obj.GetType().GetProperties())
			{
				SerializeProperty(obj, bw, property);
			}
			"None".Serialize(bw);
		}

		private static void SerializeProperty(object obj, BinaryWriter bw, PropertyInfo propertyInfo)
		{
			var value = propertyInfo.GetValue(obj);
			if (value == null) return;
			propertyInfo.Name.Serialize(bw);

			if (value is BoolProperty boolvalue)
			{
				"BoolProperty".Serialize(bw);
				bw.Write(0L);
				boolvalue.Serialize(bw);
			}
			else if (value is IntProperty intvalue)
			{
				"IntProperty".Serialize(bw);
				bw.Write(4L);
				intvalue.Serialize(bw);
			}
			else if (value is QWordProperty qwordvalue)
			{
				"QWordProperty".Serialize(bw);
				bw.Write(8L);
				qwordvalue.Serialize(bw);
			}
			else if (value is FloatProperty floatvalue)
			{
				"FloatProperty".Serialize(bw);
				bw.Write(4L);
				floatvalue.Serialize(bw);
			}
			else if (value is StrProperty stringvalue)
			{
				"StrProperty".Serialize(bw);
				bw.Write(4L + stringvalue.Value.Length + 1);
				stringvalue.Serialize(bw);
			}
			else if (value is NameProperty namevalue)
			{
				"NameProperty".Serialize(bw);
				bw.Write(4L + namevalue.Value.Length + 1);
				namevalue.Serialize(bw);
			}
			else if (value is ByteProperty bytevalue)
			{
				"ByteProperty".Serialize(bw);
				bw.Write(4L + bytevalue.Value.Length + 1);
				bytevalue.Serialize(bw);
			}
			else if (value is StructProperty structvalue)
			{
				"StructProperty".Serialize(bw);
				bw.Write((long)structvalue.Value.Length);
				structvalue.Serialize(bw);
			}
			else if (value is ObjectProperty objectvalue)
			{
				"ObjectProperty".Serialize(bw);
				bw.Write(4L);
				objectvalue.Serialize(bw);
			}
			else if (value is IList listvalue)
			{
				"ArrayProperty".Serialize(bw);
				var lengthPos = bw.BaseStream.Position;
				bw.Write(0L);
				bw.Write(listvalue.Count);
				foreach (var item in listvalue)
				{
					if (item is Property prop)
					{
						prop.Serialize(bw);
					}
					else
					{
						Serialize(item, bw);
					}
				}

				var length = bw.BaseStream.Position - lengthPos - sizeof(long);
				bw.BaseStream.Seek(lengthPos, SeekOrigin.Begin);
				bw.Write(length);
				bw.BaseStream.Seek(length, SeekOrigin.Current);
			}
			else
			{
				throw new Exception("Unknown property type: " + value.GetType());
			}
		}
	}
}
