using Newtonsoft.Json;
using RocketRP.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketRP.Serializers
{
	public class ArrayPropertyJsonConverter : JsonConverter
	{
		public override bool CanConvert(Type objectType)
		{
			return objectType.GetInterface("IArrayProperty") == typeof(IArrayProperty);
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

			var arrayProperty = (IArrayProperty)value;

			serializer.Serialize(writer, arrayProperty.GetValues());
		}
	}
}
