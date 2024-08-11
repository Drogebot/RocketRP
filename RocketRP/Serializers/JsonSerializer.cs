using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketRP.Serializers
{
	public class JsonSerializer<T>
	{
		public string Serialize(T obj, bool prettyPrint = true)
		{
			var converters = new JsonConverter[]{
				new ReplayJsonConverter(),
				new PropertyJsonConverter(),
				new KeyFrameJsonConverter(),
				new ActorUpdateJsonConverter(),
				new ActorJsonConverter(),
				new ArrayPropertyJsonConverter(),
				new DataTypesJsonConverter(),
				new ClassNetCacheJsonConverter(),
			};

			return JsonConvert.SerializeObject(obj, prettyPrint ? Formatting.Indented : Formatting.None, converters);
		}

		public T Deserialize(string json)
		{
			var converters = new JsonConverter[]{
				new ReplayJsonConverter(),
				new PropertyJsonConverter(),
				new KeyFrameJsonConverter(),
				new ActorUpdateJsonConverter(),
				new ActorJsonConverter(),
				new ArrayPropertyJsonConverter(),
				new DataTypesJsonConverter(),
				new ClassNetCacheJsonConverter(),
			};

			return JsonConvert.DeserializeObject<T>(json, converters);

		}
	}
}
