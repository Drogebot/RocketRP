using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketRP.Serializers
{
	public class KeyFrameJsonConverter : JsonConverter
	{
		public override bool CanConvert(Type objectType)
		{
			return objectType == typeof(KeyFrame);
		}

		public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, Newtonsoft.Json.JsonSerializer serializer)
		{
			if (reader.TokenType != JsonToken.StartObject) throw new JsonReaderException("Expected StartObject token!");
			reader.Read();

			var keyFrame = (KeyFrame)existingValue ?? new KeyFrame();

			while (reader.TokenType == JsonToken.PropertyName)
			{
				var propertyName = (string)reader.Value;
				reader.Read();
				switch (propertyName)
				{ 
					case "Time":
						keyFrame.Time = serializer.Deserialize<float>(reader);
						break;
					case "Frame":
						keyFrame.Frame = serializer.Deserialize<uint>(reader);
						break;
					default:
						throw new JsonException($"Unexpected property: {propertyName}");
				}
				reader.Read();
			}

			if (reader.TokenType != JsonToken.EndObject) throw new JsonReaderException("Expected EndObject token!");

			return keyFrame;
		}

		public override void WriteJson(JsonWriter writer, object? value, Newtonsoft.Json.JsonSerializer serializer)
		{
			if (value == null)
			{
				writer.WriteNull();
				return;
			}

			var keyFrame = (KeyFrame)value;

			writer.WriteStartObject();

			writer.WriteKeyValue("Time", keyFrame.Time, serializer);
			writer.WriteKeyValue("Frame", keyFrame.Frame, serializer);

			writer.WriteEndObject();
		}
	}
}
