using Newtonsoft.Json;
using RocketRP.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketRP.Serializers
{
	public class DataTypesJsonConverter : JsonConverter
	{
		public override bool CanConvert(Type objectType)
		{
			return objectType == typeof(GameServerID) || objectType == typeof(GameMode);
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

			if (value.GetType() == typeof(GameServerID))
			{
				var gameServerID = (GameServerID)value;

				serializer.Serialize(writer, gameServerID.Value);
				return;
			}

			if (value.GetType() == typeof(GameMode))
			{
				var gameMode = (GameMode)value;

				serializer.Serialize(writer, gameMode.Value);
				return;
			}
		}
	}
}
