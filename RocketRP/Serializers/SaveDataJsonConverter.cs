using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketRP.Serializers
{
	public class SaveDataJsonConverter<T> : JsonConverter where T : Actors.Core.Object
	{
		public override bool CanConvert(Type objectType)
		{
			return objectType == typeof(SaveData<T>);
		}

		public override object ReadJson(JsonReader reader, Type objectType, object? existingValue, Newtonsoft.Json.JsonSerializer serializer)
		{
			if (reader.TokenType != JsonToken.StartObject) throw new JsonReaderException("Expected StartObject token!");
			reader.Read();

			var savedata = (SaveData<T>)existingValue ?? new SaveData<T>();

			JArray objects = null;

			while (reader.TokenType == JsonToken.PropertyName)
			{
				object value = null;
				var propertyName = (string)reader.Value;
				var propInfo = savedata.GetType().GetProperty(propertyName);
				reader.Read();

				if(propertyName == "Objects")
				{
					objects = serializer.Deserialize<JArray>(reader);
					reader.Read();
					continue;
				}
				
				value = serializer.Deserialize(reader, propInfo?.PropertyType);

				propInfo?.SetValue(savedata, value);
				reader.Read();
			}

			if (reader.TokenType != JsonToken.EndObject) throw new JsonReaderException("Expected EndObject token!");

			for(int i = 0; i < objects.Count; i++)
			{
				savedata.Objects.Add((Actors.Core.Object)objects[i].ToObject(Type.GetType($"RocketRP.Actors.{savedata.ObjectTypes[i].Type}"), serializer));
			}

			return savedata;
		}

		public override void WriteJson(JsonWriter writer, object? value, Newtonsoft.Json.JsonSerializer serializer)
		{
			if (value == null)
			{
				writer.WriteNull();
				return;
			}

			var savedata = (SaveData<T>)value;

			writer.WriteStartObject();

			//writer.WriteKeyValue("Part1Length", savedata.Part1Length, serializer);
			//writer.WriteKeyValue("Part1CRC", savedata.Part1CRC, serializer);

			//writer.WriteKeyValue("Foosball", savedata.Foosball, serializer);
			//writer.WriteKeyValue("Magic", savedata.Magic, serializer);

			writer.WriteKeyValue("VersionInfo", savedata.VersionInfo, serializer);

			var nullValueHandling = serializer.NullValueHandling;
			serializer.NullValueHandling = NullValueHandling.Ignore;

			writer.WriteKeyValue("Properties", savedata.Properties, serializer);

			writer.WriteKeyValue("Objects", savedata.Objects, serializer);

			serializer.NullValueHandling = nullValueHandling;

			writer.WriteKeyValue("ObjectTypes", savedata.ObjectTypes, serializer);

			writer.WriteEndObject();
		}
	}
}
