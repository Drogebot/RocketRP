using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketRP.Serializers
{
	public class JsonSerializer
	{
		public string Serialize(Replay replay, bool prettyPrint = true)
		{
			var converters = new JsonConverter[]{
				new ReplayJsonConverter(),
				new PropertyDictionaryJsonConverter(),
				new KeyFrameJsonConverter(),
				new ActorUpdateJsonConverter(),
				new ActorJsonConverter(),
				new ArrayPropertyJsonConverter(),
				new DataTypesJsonConverter(),
				new ClassNetCacheJsonConverter(),
			};

			return JsonConvert.SerializeObject(replay, prettyPrint ? Formatting.Indented : Formatting.None, converters);
			
		}
	}
}
