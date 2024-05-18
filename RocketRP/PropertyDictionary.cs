using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketRP
{
	public class PropertyDictionary : Dictionary<string, object>
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
	}

	public class Property
	{
		public string Name;
		public string Type;
		public uint ValueLength;
		public object Value { get; set; }


		public static Property Deserialize(BinaryReader br)
		{
			var prop = new Property();
			prop.Name = br.ReadString2();
			
			if(prop.Name == "None") return prop;

			prop.Type = br.ReadString2();
			prop.ValueLength = br.ReadUInt32();
			br.ReadUInt32(); // Unknown

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
					prop.Value = br.ReadString2();
					break;
				case "ByteProperty":
					prop.Value = br.ReadString2() + "." + br.ReadString2();
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
	}
}
