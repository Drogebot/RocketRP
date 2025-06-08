using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketRP.Serializers
{
	public class ReplayJsonSerializer
	{
		public string Serialize(Replay obj, bool prettyPrint = true)
		{
			var converters = new JsonConverter[]{
				new ReplayJsonConverter(),
				new KeyFrameJsonConverter(),
				new ActorUpdateJsonConverter(),
				new ActorJsonConverter(),
				new DataTypesJsonConverter(),
				new ClassNetCacheJsonConverter(),
				new StringEnumConverter(),
			};

			return JsonConvert.SerializeObject(obj, prettyPrint ? Formatting.Indented : Formatting.None, converters);
		}

		public Replay Deserialize(string json)
		{
			var converters = new JsonConverter[]{
				new ReplayJsonConverter(),
				new KeyFrameJsonConverter(),
				new ActorUpdateJsonConverter(),
				new ActorJsonConverter(),
				new DataTypesJsonConverter(),
				new ClassNetCacheJsonConverter(),
				new StringEnumConverter(),
			};

			return JsonConvert.DeserializeObject<Replay>(json, converters) ?? throw new JsonException("Replay JSON deserialization failed");

		}
	}
}
