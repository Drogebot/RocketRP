using RocketRP.Actors.Core;
using RocketRP.Actors.Engine;
using RocketRP.DataTypes;
using System;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace RocketRP.Serializers
{
	public class JsonActorUpdateConverter : JsonConverter<ActorUpdate>
	{
		public override ActorUpdate? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			if (reader.TokenType == JsonTokenType.Null)
				return null;

			if (reader.TokenType != JsonTokenType.StartObject) throw new JsonException("Expected StartObject token!");
			var value = new ActorUpdate();

			while (reader.Read())
			{
				if (reader.TokenType == JsonTokenType.EndObject)
					return value;

				if (reader.TokenType != JsonTokenType.PropertyName)
					throw new JsonException("Expected PropertyName token!");

				var propertyName = reader.GetString();
				reader.Read();
				switch (propertyName)
				{
					case "ChannelId":
						value.ChannelId = reader.GetInt32();
						break;
					case "State":
						value.State = JsonSerializer.Deserialize<ChannelState>(ref reader, options);
						break;
					case "NameId":
						value.NameId = reader.GetInt32();
						break;
					case "Name":
						value.Name = reader.GetString();
						break;
					case "TypeId":
						value.TypeId = JsonSerializer.Deserialize<ObjectTarget<ClassObject>>(ref reader, options);
						break;
					case "TypeName":
						value.TypeName = reader.GetString();
						break;
					case "ObjectId":
						value.ObjectId = reader.GetInt32();
						break;
					case "ObjectName":
						value.ObjectName = reader.GetString();
						value.ObjectType = Type.GetType($"RocketRP.Actors.{value.ObjectName}") ?? throw new JsonException($"Could not find type {value.ObjectName}!");
						if (value.State == ChannelState.Open)
						{
							//value.Actor = (Actor)(Activator.CreateInstance(value.ObjectType) ?? throw new JsonException($"Could not create instance of type {value.ObjectName}!"));
							value.Actor = Actor.CreateInstance(value.ObjectType);
						}
						break;
					case "InitialPosition":
						value.InitialPosition = JsonSerializer.Deserialize<Vector>(ref reader, options);
						break;
					case "InitialRotation":
						value.InitialRotation = JsonSerializer.Deserialize<Rotator>(ref reader, options);
						break;
					case "ActorData":
						if (string.IsNullOrEmpty(value.ObjectName)) throw new JsonException("Expected ObjectName to be set before ActorData!");
						var actorData = JsonSerializer.Deserialize<JsonObject>(ref reader, options) ?? throw new JsonException("Failed to deserialize ActorData!");
						value.SetPropertyNames = actorData.ToDictionary().Keys.ToHashSet();
						value.ActorSnapshot = (Actor)(actorData.Deserialize(value.ObjectType, options) ?? throw new JsonException($"Failed to convert ActorData to {value.ObjectType.Name}"));
						break;
					default:
						reader.Skip();
						break;
				}
			}

			throw new JsonException("Expected EndObject token!");
		}

		public override void Write(Utf8JsonWriter writer, ActorUpdate actorUpdate, JsonSerializerOptions options)
		{
			if (actorUpdate == null)
			{
				writer.WriteNullValue();
				return;
			}

			writer.WriteStartObject();

			writer.WriteNumber("ChannelId", actorUpdate.ChannelId);
			writer.WriteString("State", actorUpdate.State.ToString());

			if(actorUpdate.Name != null)
			{
				writer.WriteNumber("NameId", actorUpdate.NameId);
				writer.WriteString("Name", actorUpdate.Name);
			}

			writer.WritePropertyName("TypeId");
			JsonSerializer.Serialize(writer, actorUpdate.TypeId, options);
			writer.WriteString("TypeName", actorUpdate.TypeName);
			writer.WriteNumber("ObjectId", actorUpdate.ObjectId);
			writer.WriteString("ObjectName", actorUpdate.ObjectName);

			if (actorUpdate.State == ChannelState.Open)
			{
				if (actorUpdate.Actor.HasInitialPosition)
				{
					writer.WritePropertyName("InitialPosition");
					JsonSerializer.Serialize(writer, actorUpdate.InitialPosition, options);
					if (actorUpdate.Actor.HasInitialRotation)
					{
						writer.WritePropertyName("InitialRotation");
						JsonSerializer.Serialize(writer, actorUpdate.InitialRotation, options);
					}
				}
			}
			else if (actorUpdate.State == ChannelState.Update)
			{
				if (!actorUpdate.IsSnapshot) throw new Exception("ActorUpdate is not a snapshot, cannot serialize ActorData! Try deserializing the replay with snapshots");
				writer.WritePropertyName("ActorData");
				JsonSerializer.Serialize(writer, actorUpdate.ActorSnapshot.ToDictionary(actorUpdate.SetProperties), options);
			}

			writer.WriteEndObject();
		}
	}
}
