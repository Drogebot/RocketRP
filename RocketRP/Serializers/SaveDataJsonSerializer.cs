using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketRP.Serializers
{
	public class SaveDataJsonSerializer
	{
		public string Serialize<T>(SaveData<T> obj, bool prettyPrint = true) where T : Actors.Core.Object
		{
			var converters = new JsonConverter[]{
				new SaveDataJsonConverter<T>(),
				new StringEnumConverter(),
			};

			return JsonConvert.SerializeObject(obj, prettyPrint ? Formatting.Indented : Formatting.None, converters);
		}

		public SaveData<T> Deserialize<T>(string json) where T : Actors.Core.Object
		{
			var converters = new JsonConverter[]{
				new SaveDataJsonConverter<T>(),
				new StringEnumConverter(),
			};

			return JsonConvert.DeserializeObject<SaveData<T>>(json, converters) ?? throw new JsonException("SaveData JSON deserialization failed");

		}
	}
}
