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
			if (objectType == typeof(Property))
			{
				return serializer.Deserialize(reader);
			}

			if (reader.TokenType != JsonToken.StartObject) throw new JsonReaderException("Expected StartObject token!");
			reader.Read();

			var propertyDictionary = (PropertyDictionary)existingValue ?? new PropertyDictionary();

			while (reader.TokenType == JsonToken.PropertyName)
			{
				object value = null;
				var propertyName = (string)reader.Value;
				reader.Read();
				var property = new Property();
				property.Name = propertyName;
				switch (reader.TokenType)
				{
					case JsonToken.StartArray:
						property.Value = serializer.Deserialize<List<PropertyDictionary>>(reader);
						break;
					default:
						property.Value = serializer.Deserialize(reader);
						break;
				}
				property.SetValueTypeFromObject(property.Value);
				propertyDictionary.Add(propertyName, property);
				reader.Read();
			}

			if (reader.TokenType != JsonToken.EndObject) throw new JsonReaderException("Expected EndObject token!");

			return propertyDictionary;
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
