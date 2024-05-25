using Newtonsoft.Json;
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
			throw new NotImplementedException();
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
