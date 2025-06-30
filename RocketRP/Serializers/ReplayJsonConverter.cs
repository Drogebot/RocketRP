using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketRP.Serializers
{
	public class ReplayJsonConverter : JsonConverter
	{
		public override bool CanConvert(Type objectType)
		{
			return objectType == typeof(Replay);
		}

		public override object ReadJson(JsonReader reader, Type objectType, object? existingValue, Newtonsoft.Json.JsonSerializer serializer)
		{
			if (reader.TokenType != JsonToken.StartObject) throw new JsonReaderException("Expected StartObject token!");
			reader.Read();

			var replay = (Replay?)existingValue ?? new Replay();
			replay.NetVersion = 0;

			while (reader.TokenType == JsonToken.PropertyName)
			{
				var propertyName = (string)reader.Value!;
				reader.Read();
				object? value = propertyName switch
				{
					"Part1Length" or "Part1CRC" or "EngineVersion" or "LicenseeVersion" or "NetVersion" or "Part2Length" or "Part2CRC" => serializer.Deserialize<uint>(reader),
					"ReplayClass" => serializer.Deserialize<string>(reader),
					"Properties" => serializer.Deserialize(reader, Type.GetType($"RocketRP.Actors.{replay.ReplayClass}")),
					"Levels" => serializer.Deserialize<List<string>>(reader),
					"KeyFrames" => serializer.Deserialize<List<KeyFrame>>(reader),
					"Frames" => serializer.Deserialize<List<Frame>>(reader),
					"DebugStrings" => serializer.Deserialize<List<DebugString>>(reader),
					"Tickmarks" => serializer.Deserialize<List<Tickmark>>(reader),
					"Packages" => serializer.Deserialize<List<string>>(reader),
					"Objects" => serializer.Deserialize<List<string>>(reader),
					"Names" => serializer.Deserialize<List<string>>(reader),
					"ClassIndexes" => serializer.Deserialize<Dictionary<string, int>>(reader),
					"ClassNetCaches" => serializer.Deserialize<List<ClassNetCache>>(reader),
					_ => throw new JsonReaderException($"Unexpected property {propertyName}!"),
				};
				replay.GetType().GetProperty(propertyName)!.SetValue(replay, value);
				reader.Read();
			}

			if (reader.TokenType != JsonToken.EndObject) throw new JsonReaderException("Expected EndObject token!");

			return replay;
		}

		public override void WriteJson(JsonWriter writer, object? value, Newtonsoft.Json.JsonSerializer serializer)
		{
			if (value == null)
			{
				writer.WriteNull();
				return;
			}

			var replay = (Replay)value;

			writer.WriteStartObject();

			//writer.WriteKeyValue("Part1Length", replay.Part1Length, serializer);
			//writer.WriteKeyValue("Part1CRC", replay.Part1CRC, serializer);

			writer.WriteKeyValue("EngineVersion", replay.EngineVersion, serializer);
			writer.WriteKeyValue("LicenseeVersion", replay.LicenseeVersion, serializer);

			if (replay.EngineVersion >= 868 && replay.LicenseeVersion >= 18)
			{
				writer.WriteKeyValue("NetVersion", replay.NetVersion, serializer);
			}

			writer.WriteKeyValue("ReplayClass", replay.ReplayClass, serializer);

			var nullValueHandling = serializer.NullValueHandling;
			serializer.NullValueHandling = NullValueHandling.Ignore;

			writer.WriteKeyValue("Properties", replay.Properties, serializer);

			serializer.NullValueHandling = nullValueHandling;

			//writer.WriteKeyValue("Part2Length", replay.Part2Length, serializer);
			//writer.WriteKeyValue("Part2CRC", replay.Part2CRC, serializer);

			writer.WriteKeyValue("Levels", replay.Levels, serializer);

			writer.WriteKeyValue("KeyFrames", replay.KeyFrames, serializer);

			writer.WriteKeyValue("Frames", replay.Frames, serializer);

			writer.WriteKeyValue("DebugStrings", replay.DebugStrings, serializer);

			writer.WriteKeyValue("Tickmarks", replay.Tickmarks, serializer);

			writer.WriteKeyValue("Packages", replay.Packages, serializer);

			writer.WriteKeyValue("Objects", replay.Objects, serializer);

			writer.WriteKeyValue("Names", replay.Names, serializer);

			writer.WriteKeyValue("ClassIndexes", replay.ClassIndexes, serializer);

			writer.WriteKeyValue("ClassNetCaches", replay.ClassNetCaches, serializer);

			writer.WriteEndObject();
		}
	}
}
