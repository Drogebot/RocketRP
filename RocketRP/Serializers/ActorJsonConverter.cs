using Newtonsoft.Json;
using RocketRP.Actors.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketRP.Serializers
{
	public class ActorJsonConverter : JsonConverter
	{
		public override bool CanConvert(Type objectType)
		{
			return objectType.IsSubclassOf(typeof(Actor));
		}

		public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, Newtonsoft.Json.JsonSerializer serializer)
		{
			if (reader.TokenType == JsonToken.Null) return null;

			if (reader.TokenType != JsonToken.StartObject) throw new JsonReaderException("Expected StartObject token!");
			reader.Read();

			var actor = (Actor)existingValue ?? (Actor)Activator.CreateInstance(objectType);

			while (reader.TokenType == JsonToken.PropertyName)
			{
				var propertyName = (string)reader.Value;
				reader.Read();
				var value = serializer.Deserialize(reader, actor.GetType().GetProperty(propertyName).PropertyType);
				actor.GetType().GetProperty(propertyName)?.SetValue(actor, value);
				if(propertyName != "InitialPosition" && propertyName != "InitialRotation") actor.SetPropertyNames.Add(propertyName);
				reader.Read();
			}

			if (reader.TokenType != JsonToken.EndObject) throw new JsonReaderException("Expected EndObject token!");

			return actor;
		}

		public override void WriteJson(JsonWriter writer, object? value, Newtonsoft.Json.JsonSerializer serializer)
		{
			if (value == null)
			{
				writer.WriteNull();
				return;
			}

			var actor = (Actor)value;

			writer.WriteStartObject();

			if (actor.InitialPosition.HasValue)
			{
				writer.WriteKeyValue("InitialPosition", actor.InitialPosition.Value, serializer);
			}

			if (actor.InitialRotation.HasValue)
			{
				writer.WriteKeyValue("InitialRotation", actor.InitialRotation.Value, serializer);
			}

			var properties = actor.GetType().GetProperties();
			foreach (var property in properties)
			{
				if(actor.SetPropertyNames.Contains(property.Name))
				{
					writer.WriteKeyValue(property.Name, property.GetValue(actor), serializer);
				}
			}

			writer.WriteEndObject();
		}
	}
}
