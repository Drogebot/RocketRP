using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketRP.Serializers
{
	public static class JsonWriterExtension
	{
		public static void WriteKeyValue(this JsonWriter writer, string key, object? value, Newtonsoft.Json.JsonSerializer serializer)
		{
			writer.WritePropertyName(key);
			serializer.Serialize(writer, value);
		}
	}
}
