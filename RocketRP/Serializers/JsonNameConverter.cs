using RocketRP.DataTypes;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace RocketRP.Serializers
{
	public class JsonNameConverter : JsonConverter<Name>
	{
		public override Name Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			return reader.GetString() ?? new Name();
		}

		public override void Write(Utf8JsonWriter writer, Name nameValue, JsonSerializerOptions options)
		{
			writer.WriteStringValue(nameValue);
		}
	}
}
