using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketRP.Serializers
{
	public class PropertyJsonConverter : JsonConverter
	{
		public override bool CanConvert(Type objectType)
		{
			return objectType.IsSubclassOf(typeof(Property));
		}

		public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, Newtonsoft.Json.JsonSerializer serializer)
		{
			if (objectType == typeof(BoolProperty)) return (BoolProperty)serializer.Deserialize<dynamic>(reader);
			else if (objectType == typeof(IntProperty)) return (IntProperty)serializer.Deserialize<dynamic>(reader);
			else if (objectType == typeof(QWordProperty)) return (QWordProperty)serializer.Deserialize<dynamic>(reader);
			else if (objectType == typeof(FloatProperty)) return (FloatProperty)serializer.Deserialize<dynamic>(reader);
			else if (objectType == typeof(StrProperty)) return (StrProperty)serializer.Deserialize<dynamic>(reader);
			else if (objectType == typeof(NameProperty)) return (NameProperty)serializer.Deserialize<dynamic>(reader);
			else if (objectType == typeof(ByteProperty)) return (ByteProperty)serializer.Deserialize<dynamic>(reader);
			else if (objectType == typeof(ObjectProperty)) return (ObjectProperty)serializer.Deserialize<dynamic>(reader);
			else if (objectType == typeof(StructProperty)) return (StructProperty)serializer.Deserialize<dynamic>(reader);
			throw new Exception($"Unknown property type {objectType.Name}");
		}

		public override void WriteJson(JsonWriter writer, object? value, Newtonsoft.Json.JsonSerializer serializer)
		{
			if (value == null)
			{
				writer.WriteNull();
				return;
			}

			serializer.Serialize(writer, ((Property)value).GetValue());
		}
	}
}
