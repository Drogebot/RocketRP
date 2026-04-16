using System;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace RocketRP.Serializers
{
	public class JsonStructConverter : JsonConverterFactory
	{
		private readonly JsonSerializerOptions _options;

		public JsonStructConverter(JsonSerializerOptions options)
		{
			_options = new JsonSerializerOptions(options)
			{
				DefaultIgnoreCondition = JsonIgnoreCondition.Never,
			};
		}

		public override bool CanConvert(Type objectType)
		{
			return objectType.IsValueType && !objectType.IsPrimitive;
		}

		public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
		{
			var converterType = typeof(JsonStructConverterInner<>).MakeGenericType(typeToConvert);
			return (JsonConverter)(Activator.CreateInstance(converterType, BindingFlags.Instance | BindingFlags.Public, binder: null, args: [_options], culture: null)?? throw new InvalidOperationException($"Cannot create converter for {typeToConvert}"));
		}

		private class JsonStructConverterInner<T> : JsonConverter<T> where T : struct
		{
			private readonly JsonSerializerOptions _options;

			public JsonStructConverterInner(JsonSerializerOptions options)
			{
				_options = options;
			}

			public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
			{
				return JsonSerializer.Deserialize<T>(ref reader, _options);
			}

			public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
			{
				JsonSerializer.Serialize(writer, value, _options);
			}
		}
	}
}