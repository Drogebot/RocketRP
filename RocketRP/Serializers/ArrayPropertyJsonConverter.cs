using Newtonsoft.Json;
using RocketRP.Actors.Engine;
using RocketRP.DataTypes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RocketRP.Serializers
{
	public class ArrayPropertyJsonConverter : JsonConverter
	{
		public override bool CanConvert(Type objectType)
		{
			if (Nullable.GetUnderlyingType(objectType) != null) objectType = objectType.GenericTypeArguments[0];
			return objectType.GetInterface("IArrayProperty") == typeof(IArrayProperty);
		}

		public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, Newtonsoft.Json.JsonSerializer serializer)
		{
			if (Nullable.GetUnderlyingType(objectType) != null) objectType = objectType.GenericTypeArguments[0];
			if (reader.TokenType != JsonToken.StartArray) throw new JsonReaderException("Expected StartArray token!");

			var type = typeof(List<>).MakeGenericType(objectType.GenericTypeArguments[0]);
			var values = (IList)serializer.Deserialize(reader, type);

			var arrayProperty = (IArrayProperty)existingValue ?? (IArrayProperty)Activator.CreateInstance(objectType, values);

			return arrayProperty;
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
