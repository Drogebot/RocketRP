using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketRP.Serializers
{
	public class KeyFrameJsonConverter : JsonConverter
	{
		public override bool CanConvert(Type objectType)
		{
			return objectType == typeof(KeyFrame);
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

			var keyFrame = (KeyFrame)value;

			writer.WriteStartObject();

			writer.WriteKeyValue("Time", keyFrame.Time, serializer);
			writer.WriteKeyValue("Frame", keyFrame.Frame, serializer);

			writer.WriteEndObject();
		}
	}
}
