using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace RocketRP.Serializers
{
	public class JsonSaveDataConverter<T> : JsonConverter<SaveData<T>> where T : Actors.Core.Object, new()
	{
		public override SaveData<T>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			if (reader.TokenType == JsonTokenType.Null)
				return null;

			if (reader.TokenType != JsonTokenType.StartObject) throw new JsonException("Expected StartObject token!");
			var saveData = new SaveData<T>() { Properties = new T() };

			while (reader.Read())
			{
				if (reader.TokenType == JsonTokenType.EndObject)
					return saveData;

				if (reader.TokenType != JsonTokenType.PropertyName)
					throw new JsonException("Expected PropertyName token!");

				var propertyName = reader.GetString();
				reader.Read();
				switch (propertyName)
				{
					case "VersionInfo":
						saveData.VersionInfo = JsonSerializer.Deserialize<SaveDataVersionInfo>(ref reader, options);
						break;
					case "Properties":
						saveData.Properties = JsonSerializer.Deserialize<T>(ref reader, options) ?? throw new JsonException("Failed to deserialize SaveData properties!");
						break;
					case "Objects":
						if(reader.TokenType != JsonTokenType.StartArray) throw new JsonException("Expected StartArray token for Objects!");
						reader.Read();
						while (reader.TokenType != JsonTokenType.EndArray)
						{
							var objectData = JsonSerializer.Deserialize<JsonObject>(ref reader, options) ?? throw new JsonException("Failed to deserialize Object!");
							if(!objectData.TryGetPropertyValue("ObjectName", out var typeNode))
								throw new JsonException("Object is missing 'ObjectName' property!");
							var objectType = Type.GetType($"RocketRP.Actors.{typeNode.Deserialize<string>()}") ?? throw new JsonException("Invalid type!");
							saveData.Objects.Add((Actors.Core.Object)(objectData.Deserialize(objectType, options) ?? throw new JsonException($"Failed to convert Object to {objectType.Name}")));
							reader.Read();
						}
						if(reader.TokenType != JsonTokenType.EndArray) throw new JsonException("Expected EndArray token for Objects!");
						break;
					case "ObjectTypes":
						saveData.ObjectTypes = JsonSerializer.Deserialize<List<ObjectType>>(ref reader, options) ?? throw new JsonException("Failed to deserialize ObjectTypes!");
						break;
					default:
						reader.Skip();
						break;
				}
			}

			throw new JsonException("Expected EndObject token!");
		}

		public override void Write(Utf8JsonWriter writer, SaveData<T> savedata, JsonSerializerOptions options)
		{
			if (savedata == null)
			{
				writer.WriteNullValue();
				return;
			}

			writer.WriteStartObject();

			writer.WritePropertyName("VersionInfo");
			JsonSerializer.Serialize(writer, savedata.VersionInfo, options);

			writer.WritePropertyName("Properties");
			JsonSerializer.Serialize(writer, savedata.Properties, options);

			writer.WritePropertyName("Objects");
			writer.WriteStartArray();
			savedata.Objects.ForEach(obj => JsonSerializer.Serialize(writer, obj.ToDictionary(), options));
			writer.WriteEndArray();

			writer.WritePropertyName("ObjectTypes");
			JsonSerializer.Serialize(writer, savedata.ObjectTypes, options);

			writer.WriteEndObject();
		}
	}
}
