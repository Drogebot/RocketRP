using RocketRP.DataTypes;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace RocketRP.Serializers
{
	public class JsonRigidBodyConverter : JsonConverter<ReplicatedRBState>
	{
		public override ReplicatedRBState Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			return JsonSerializer.Deserialize<ReplicatedRBState>(ref reader);
		}

		public override void Write(Utf8JsonWriter writer, ReplicatedRBState rbState, JsonSerializerOptions options)
		{
			writer.WriteStartObject();

			writer.WriteBoolean("Sleeping", rbState.Sleeping);
			writer.WritePropertyName("Position");
			JsonSerializer.Serialize(writer, rbState.Position, options);
			writer.WritePropertyName("Rotation");
			JsonSerializer.Serialize(writer, rbState.Rotation, options);

			if (!rbState.Sleeping)
			{
				writer.WritePropertyName("LinearVelocity");
				JsonSerializer.Serialize(writer, rbState.LinearVelocity, options);
				writer.WritePropertyName("AngularVelocity");
				JsonSerializer.Serialize(writer, rbState.AngularVelocity, options);
			}

			writer.WriteEndObject();
		}
	}

	public class JsonGameServerIDConverter : JsonConverter<GameServerID>
	{
		public override GameServerID Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			return reader.GetString();
		}

		public override void Write(Utf8JsonWriter writer, GameServerID gameServerID, JsonSerializerOptions options)
		{
			writer.WriteStringValue(gameServerID);
		}
	}

	public class JsonGameModeConverter : JsonConverter<GameMode>
	{
		public override GameMode Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			return reader.GetByte();
		}

		public override void Write(Utf8JsonWriter writer, GameMode gameMode, JsonSerializerOptions options)
		{
			writer.WriteNumberValue(gameMode);
		}
	}
}
