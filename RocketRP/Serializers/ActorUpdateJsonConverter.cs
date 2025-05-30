using Newtonsoft.Json;
using RocketRP.Actors.Core;
using RocketRP.Actors.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketRP.Serializers
{
	public class ActorUpdateJsonConverter : JsonConverter
	{
		public override bool CanConvert(Type objectType)
		{
			return objectType == typeof(ActorUpdate);
		}

		public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, Newtonsoft.Json.JsonSerializer serializer)
		{
			if (reader.TokenType != JsonToken.StartObject) throw new JsonReaderException("Expected StartObject token!");
			reader.Read();

			var actorUpdate = (ActorUpdate)existingValue ?? new ActorUpdate();

			while (reader.TokenType == JsonToken.PropertyName)
			{
				var propertyName = (string)reader.Value;
				reader.Read();
				switch (propertyName)
				{
					case "ChannelId":
						actorUpdate.ChannelId = serializer.Deserialize<int>(reader);
						break;
					case "State":
						actorUpdate.State = serializer.Deserialize<ChannelState>(reader);
						break;
					case "NameId":
						actorUpdate.NameId = serializer.Deserialize<int>(reader);
						break;
					case "Name":
						actorUpdate.Name = serializer.Deserialize<string>(reader);
						break;
					case "TypeId":
						actorUpdate.TypeId = serializer.Deserialize<ObjectTarget<ClassObject>>(reader);
						break;
					case "TypeName":
						actorUpdate.TypeName = serializer.Deserialize<string>(reader);
						break;
					case "ObjectId":
						actorUpdate.ObjectId = serializer.Deserialize<int>(reader);
						break;
					case "ObjectName":
						actorUpdate.ObjectName = serializer.Deserialize<string>(reader);
						break;
					case "ActorData":
						if(string.IsNullOrEmpty(actorUpdate.ObjectName)) throw new JsonException("Expected ObjectName to be set before ActorData!");
						var type = Type.GetType($"RocketRP.Actors.{actorUpdate.ObjectName}");
						if(actorUpdate.State == ChannelState.Open)
						{
							;
						}
						actorUpdate.Actor = (Actor)serializer.Deserialize(reader, type);
						break;
					default:
						throw new JsonException($"Unexpected property: {propertyName}");
				}
				reader.Read();
			}

			if (reader.TokenType != JsonToken.EndObject) throw new JsonReaderException("Expected EndObject token!");

			return actorUpdate;
		}

		public override void WriteJson(JsonWriter writer, object? value, Newtonsoft.Json.JsonSerializer serializer)
		{
			if (value == null)
			{
				writer.WriteNull();
				return;
			}

			var actorUpdate = (ActorUpdate)value;

			writer.WriteStartObject();

			writer.WriteKeyValue("ChannelId", actorUpdate.ChannelId, serializer);
			writer.WriteKeyValue("State", actorUpdate.State, serializer);

			if (actorUpdate.NameId.HasValue)
			{
				writer.WriteKeyValue("NameId", actorUpdate.NameId.Value, serializer);
				writer.WriteKeyValue("Name", actorUpdate.Name, serializer);
			}

			writer.WriteKeyValue("TypeId", actorUpdate.TypeId, serializer);
			writer.WriteKeyValue("TypeName", actorUpdate.TypeName, serializer);
			writer.WriteKeyValue("ObjectId", actorUpdate.ObjectId, serializer);
			writer.WriteKeyValue("ObjectName", actorUpdate.ObjectName, serializer);

			writer.WriteKeyValue("ActorData", actorUpdate.Actor, serializer);

			writer.WriteEndObject();
		}
	}
}
