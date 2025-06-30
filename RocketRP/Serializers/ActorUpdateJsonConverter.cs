using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RocketRP.Actors.Core;
using RocketRP.Actors.Engine;
using RocketRP.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
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

			var actorUpdate = (ActorUpdate?)existingValue ?? new ActorUpdate();

			while (reader.TokenType == JsonToken.PropertyName)
			{
				var propertyName = (string)reader.Value!;
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
						actorUpdate.TypeName = serializer.Deserialize<string>(reader)!;
						break;
					case "ObjectId":
						actorUpdate.ObjectId = serializer.Deserialize<int>(reader);
						break;
					case "ObjectName":
						actorUpdate.ObjectName = serializer.Deserialize<string>(reader)!;
						actorUpdate.ObjectType = Type.GetType($"RocketRP.Actors.{actorUpdate.ObjectName}") ?? throw new JsonException($"Could not find type {actorUpdate.ObjectName}!");
						if (actorUpdate.State == ChannelState.Open)
						{
							//actorUpdate.Actor = (Actor)(Activator.CreateInstance(actorUpdate.ObjectType) ?? throw new JsonException($"Could not create instance of type {actorUpdate.ObjectName}!"));
							actorUpdate.Actor = Actor.CreateInstance(actorUpdate.ObjectType);
						}
						break;
					case "InitialPosition":
						actorUpdate.InitialPosition = serializer.Deserialize<Vector>(reader);
						break;
					case "InitialRotation":
						actorUpdate.InitialRotation = serializer.Deserialize<Rotator>(reader);
						break;
					case "ActorData":
						if (string.IsNullOrEmpty(actorUpdate.ObjectName)) throw new JsonException("Expected ObjectName to be set before ActorData!");
						var actorData = serializer.Deserialize<JObject>(reader) ?? throw new JsonException($"Failed to read ActorData for {actorUpdate.ObjectType.Name}");
						actorUpdate.SetPropertyNames = actorData.Properties().Select(a => a.Name).ToHashSet();
						actorUpdate.ActorSnapshot = (Actor?)actorData.ToObject(actorUpdate.ObjectType, serializer) ?? throw new Exception($"Failed to convert ActorData to {actorUpdate.ObjectType.Name}");
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

			if (actorUpdate.State == ChannelState.Open)
			{
				if (actorUpdate.Actor.HasInitialPosition)
				{
					writer.WriteKeyValue("InitialPosition", actorUpdate.InitialPosition, serializer);
					if (actorUpdate.Actor.HasInitialRotation)
					{
						writer.WriteKeyValue("InitialRotation", actorUpdate.InitialRotation, serializer);
					}
				}
			}
			else if(actorUpdate.State == ChannelState.Update)
			{
				if (!actorUpdate.IsSnapshot) throw new Exception("ActorUpdate is not a snapshot, cannot serialize ActorData! Try deserializing the replay with snapshots");
				writer.WriteKeyValue("ActorData", actorUpdate.ActorSnapshot.ToDictionary(actorUpdate.SetProperties), serializer);
			}

			writer.WriteEndObject();
		}
	}
}
