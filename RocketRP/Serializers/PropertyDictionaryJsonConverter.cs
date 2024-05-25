using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketRP.Serializers
{
	public class PropertyDictionaryJsonConverter : JsonConverter
	{
		public override bool CanConvert(Type objectType)
		{
			return objectType == typeof(PropertyDictionary) || objectType == typeof(Property);
		}

		public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, Newtonsoft.Json.JsonSerializer serializer)
		{
			throw new NotImplementedException();
		}

		public override void WriteJson(JsonWriter writer, object? value, Newtonsoft.Json.JsonSerializer serializer)
		{
			if (value == null)
			{
				writer.WriteNull();
				return;
			}

			if (value.GetType() == typeof(Property))
			{
				var property = (Property)value;

				serializer.Serialize(writer, property.Value);
				return;
			}

			var propertyDictionary = (PropertyDictionary)value;

			writer.WriteStartObject();

			foreach (var property in propertyDictionary)
			{
				writer.WritePropertyName(property.Key);
				serializer.Serialize(writer, property.Value);
			}

			writer.WriteEndObject();
		}
	}
}
