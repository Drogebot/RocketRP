using System.Text.Json;
using System.Text.Json.Serialization;

namespace RocketRP.Serializers
{
	public class SaveDataJsonSerializer
	{
		public string Serialize<T>(SaveData<T> obj, bool prettyPrint = true) where T : Actors.Core.Object
		{
			var options = new JsonSerializerOptions()
			{
				WriteIndented = prettyPrint,
				DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault,
			};
			options.Converters.Add(new JsonStringEnumConverter());
			options.Converters.Add(new JsonNameConverter());
			options.Converters.Add(new JsonSaveDataConverter<T>());
			options.Converters.Add(new JsonStructConverter(options));

			return JsonSerializer.Serialize(obj, options);
		}

		public SaveData<T> Deserialize<T>(string json) where T : Actors.Core.Object
		{

			var options = new JsonSerializerOptions();
			options.Converters.Add(new JsonStringEnumConverter());
			options.Converters.Add(new JsonNameConverter());
			options.Converters.Add(new JsonSaveDataConverter<T>());
			//options.Converters.Add(new JsonStructConverter(options));

			return JsonSerializer.Deserialize<SaveData<T>>(json, options) ?? throw new JsonException("SaveData JSON deserialization failed");
		}
	}
}
