using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketRP.Serializers
{
	public class ClassNetCacheJsonConverter : JsonConverter
	{
		public override bool CanConvert(Type objectType)
		{
			return objectType == typeof(ClassNetCache);
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

			var classNetCache = (ClassNetCache)value;

			writer.WriteStartObject();

			writer.WriteKeyValue("ObjectIndex", classNetCache.ObjectIndex, serializer);
			writer.WriteKeyValue("MinPropertyId", classNetCache.MinPropertyId, serializer);
			writer.WriteKeyValue("MaxPropertyId", classNetCache.MaxPropertyId, serializer);
			writer.WriteKeyValue("Properties", classNetCache.Properties, serializer);

			writer.WriteEndObject();
		}
	}
}
