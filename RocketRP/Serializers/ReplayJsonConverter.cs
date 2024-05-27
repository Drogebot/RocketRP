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
			if(reader.TokenType != JsonToken.StartObject) throw new JsonReaderException("Expected StartObject token!");
			reader.Read();

			var replay = (Replay)existingValue ?? new Replay();

			while (reader.TokenType == JsonToken.PropertyName)
			{
				object value = null;
				var propertyName = (string)reader.Value;
				reader.Read();
				switch (propertyName)
				{
					case "Part1Length":
					case "Part1CRC":
					case "EngineVersion":
					case "LicenseeVersion":
					case "NetVersion":
					case "Part2Length":
					case "Part2CRC":
						value = serializer.Deserialize<uint>(reader);
						break;
					case "ReplayClass":
						value = serializer.Deserialize<string>(reader);
						break;
					case "Properties":
						value = serializer.Deserialize<PropertyDictionary>(reader);
						break;
					case "Levels":
						value = serializer.Deserialize<List<string>>(reader);
						break;
					case "KeyFrames":
						value = serializer.Deserialize<List<KeyFrame>>(reader);
						break;
					case "Frames":
						value = serializer.Deserialize<List<Frame>>(reader);
						break;
					case "DebugStrings":
						value = serializer.Deserialize<List<DebugString>>(reader);
						break;
					case "Tickmarks":
						value = serializer.Deserialize<List<Tickmark>>(reader);
						break;
					case "Packages":
						value = serializer.Deserialize<List<string>>(reader);
						break;
					case "Objects":
						value = serializer.Deserialize<List<string>>(reader);
						break;
					case "Names":
						value = serializer.Deserialize<List<string>>(reader);
						break;
					case "ClassIndexes":
						value = serializer.Deserialize<Dictionary<string, int>>(reader);
						break;
					case "ClassNetCaches":
						value = serializer.Deserialize<List<ClassNetCache>>(reader);
						break;
					default:
						throw new JsonReaderException($"Unexpected property {propertyName}!");
				}

				replay.GetType().GetProperty(propertyName)?.SetValue(replay, value);
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

			writer.WriteKeyValue("Properties", replay.Properties, serializer);

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
