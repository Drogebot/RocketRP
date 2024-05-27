using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketRP.Serializers
{
	public class ClassNetCacheJsonConverter : JsonConverter
	{
		public override bool CanConvert(Type objectType)
		{
			return objectType == typeof(ClassNetCache);
		}

		public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, Newtonsoft.Json.JsonSerializer serializer)
		{
			if (reader.TokenType != JsonToken.StartObject) throw new JsonReaderException("Expected StartObject token!");
			reader.Read();

			var classNetCache = (ClassNetCache)existingValue ?? new ClassNetCache();

			while (reader.TokenType == JsonToken.PropertyName)
			{
				var propertyName = (string)reader.Value;
				reader.Read();
				switch (propertyName)
				{
					case "ObjectIndex":
						classNetCache.ObjectIndex = serializer.Deserialize<int>(reader);
						break;
					case "MinPropertyId":
						classNetCache.MinPropertyId = serializer.Deserialize<int>(reader);
						break;
					case "MaxPropertyId":
						classNetCache.MaxPropertyId = serializer.Deserialize<int>(reader);
						break;
					case "Properties":
						classNetCache.Properties = serializer.Deserialize<List<ClassNetCacheProperty>>(reader);
						break;
					default:
						throw new JsonException($"Unexpected property: {propertyName}");
				}
				reader.Read();
			}

			if (reader.TokenType != JsonToken.EndObject) throw new JsonReaderException("Expected EndObject token!");

			return classNetCache;
		}

		public override void WriteJson(JsonWriter writer, object? value, Newtonsoft.Json.JsonSerializer serializer)
		{
			if (value == null)
			{
				writer.WriteNull();
				return;
			}

			var classNetCache = (ClassNetCache)value;

			writer.WriteStartObject();

			writer.WriteKeyValue("ObjectIndex", classNetCache.ObjectIndex, serializer);
			writer.WriteKeyValue("MinPropertyId", classNetCache.MinPropertyId, serializer);
			writer.WriteKeyValue("MaxPropertyId", classNetCache.MaxPropertyId, serializer);
			writer.WriteKeyValue("Properties", classNetCache.Properties, serializer);

			writer.WriteEndObject();
		}
	}
}
