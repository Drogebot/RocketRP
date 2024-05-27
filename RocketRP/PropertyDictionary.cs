using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketRP
{
	public class PropertyDictionary : Dictionary<string, Property>
	{
		public static PropertyDictionary Deserialize(BinaryReader br)
		{
			var properties = new PropertyDictionary();
			while(true)
			{
				var prop = Property.Deserialize(br);
				if (prop.Name == "None") break;
				properties.Add(prop.Name, prop);
			}

			return properties;
		}

		public void Serialize(BinaryWriter bw)
		{
			foreach (var prop in this)
			{
				prop.Value.Serialize(bw);
			}
			"None".Serialize(bw);
		}
	}

	public class Property
	{
		public string Name;
		public string Type;
		public long ValueLength;
		public object Value { get; set; }

		public void SetValueTypeFromObject(object obj)
		{
			var objType = obj.GetType();
			if (objType == typeof(bool))
			{
				Type = "BoolProperty";
			}
			else if (objType == typeof(int))
			{
				Type = "IntProperty";
			}
			else if (objType == typeof(long))
			{
				if ((long)obj <= int.MaxValue)
				{
					Type = "IntProperty";
					Value = Convert.ToInt32(obj);
					return;
				}
				Type = "QWordProperty";
			}
			else if (objType == typeof(float) || objType == typeof(double))
			{
				Type = "FloatProperty";
				Value = Convert.ToSingle(obj);
			}
			else if (objType == typeof(string))
			{
				Type = "StrProperty";
			}
			else if (objType.GetInterface("IList") == typeof(IList))
			{
				Type = "ArrayProperty";
			}
			else
			{
				throw new Exception("Unknown property type: " + objType);
			}
		}

		public static Property Deserialize(BinaryReader br)
		{
			var prop = new Property();
			prop.Name = "".Deserialize(br);
			
			if(prop.Name == "None") return prop;

			prop.Type = "".Deserialize(br);
			prop.ValueLength = br.ReadInt64();

			switch (prop.Type)
			{
				case "BoolProperty":
					prop.Value = br.ReadBoolean();
					break;
				case "IntProperty":
					prop.Value = br.ReadInt32();
					break;
				case "QWordProperty":
					prop.Value = br.ReadInt64();
					break;
				case "FloatProperty":
					prop.Value = br.ReadSingle();
					break;
				case "StrProperty":
				case "NameProperty":
					prop.Value = "".Deserialize(br);
					break;
				case "ByteProperty":
					prop.Value = "".Deserialize(br) + "." + "".Deserialize(br);
					break;
				case "ArrayProperty":
					var arrayLength = br.ReadInt32();
					var propArray = new List<PropertyDictionary>(arrayLength);
					for(int i = 0; i < arrayLength; i++)
					{
						propArray.Add(PropertyDictionary.Deserialize(br));
					}
					prop.Value = propArray;
					break;
				default:
					throw new Exception("Unknown property type: " + prop.Type);
			}

			return prop;
		}

		public void Serialize(BinaryWriter bw)
		{
			Name.Serialize(bw);
			if (Name == "None") return;

			Type.Serialize(bw);

			// These will be overwritten once we know their values
			bw.Write(0UL);   // ValueLength
			var valuePos = bw.BaseStream.Position;

			switch (Type)
			{
				case "BoolProperty":
					bw.Write((bool)Value);
					break;
				case "IntProperty":
					bw.Write((int)Value);
					break;
				case "QWordProperty":
					bw.Write((long)Value);
					break;
				case "FloatProperty":
					bw.Write((float)Value);
					break;
				case "StrProperty":
				case "NameProperty":
					((string)Value).Serialize(bw);
					break;
				case "ByteProperty":
					var split = ((string)Value).Split('.');
					split[0].Serialize(bw);
					split[1].Serialize(bw);
					break;
				case "ArrayProperty":
					var array = (List<PropertyDictionary>)Value;
					bw.Write(array.Count);
					foreach (var prop in array)
					{
						prop.Serialize(bw);
					}
					break;
				default:
					throw new Exception("Unknown property type: " + Type);
			}

			ValueLength = bw.BaseStream.Position - valuePos;
			bw.BaseStream.Seek(valuePos - sizeof(long), SeekOrigin.Begin);
			bw.Write(ValueLength);
			bw.BaseStream.Seek(ValueLength, SeekOrigin.Current);
		}
	}
}
