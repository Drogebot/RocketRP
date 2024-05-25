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
			throw new NotImplementedException();
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
