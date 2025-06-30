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
				new ActorUpdateJsonConverter(),
				new DataTypesJsonConverter(),
				new StringEnumConverter(),
				new ReplayJsonConverter(),
			};

			return JsonConvert.SerializeObject(obj, prettyPrint ? Formatting.Indented : Formatting.None, converters);
		}

		public Replay Deserialize(string json)
		{
			var converters = new JsonConverter[]{
				new ActorUpdateJsonConverter(),
				new DataTypesJsonConverter(),
				new StringEnumConverter(),
				new ReplayJsonConverter(),
			};

			var replay = JsonConvert.DeserializeObject<Replay>(json, converters) ?? throw new JsonException("Replay JSON deserialization failed");

			foreach (var classNetCache in replay.ClassNetCaches)
			{
				classNetCache.CalculateParent(replay);
			}
			var shouldThrow = false;
			foreach (var classNetCache in replay.ClassNetCaches)
			{
				var didLinkError = !classNetCache.LinkTypeAndPropertyInfos(replay.Objects);
				if (didLinkError)
				{
					Console.ForegroundColor = ConsoleColor.Yellow;
					Console.WriteLine($"Warning: Error linking ClassNetCache {classNetCache.ObjectIndex} ({replay.Objects[classNetCache.ObjectIndex]})");
					Console.ForegroundColor = ConsoleColor.Gray;
				}
				shouldThrow |= didLinkError;
			}
			//if (shouldThrow) throw new Exception("Some ClassNetCaches could not be linked, check the console for more information.");

			foreach (var frame in replay.Frames)
			{
				foreach (var actorUpdate in frame.ActorUpdates)
				{
					if (actorUpdate.SetPropertyNames is null) continue;
					actorUpdate.ClassNetCache = replay.ClassNetCaches.FirstOrDefault(c => c.ClassType == actorUpdate.ObjectType)
						?? throw new JsonException($"ClassNetCache for ObjectId {actorUpdate.ObjectId} not found in replay.");
					actorUpdate.SetProperties = actorUpdate.SetPropertyNames!.SelectMany(name =>
					{
						var prop = actorUpdate.ClassNetCache.GetPropertyByName(name);
						if (prop!.PropertyInfo.PropertyType.IsArray)
						{
							var arr = new (ClassNetCacheProperty, int)[((Array?)prop.PropertyInfo.GetValue(actorUpdate.ActorSnapshot))?.Length ?? 0];
							for (int i = 0; i < arr.Length; i++)
							{
								arr[i] = (prop, i);
							}
							return arr;
						}
						return [(prop, 0)];
					}).ToHashSet();
				}
			}

			return replay;
		}
	}
}
