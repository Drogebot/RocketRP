using System;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace RocketRP.Serializers
{
	public class ReplayJsonSerializer
	{
		public string Serialize(Replay replay, bool prettyPrint = true)
		{
			var options = new JsonSerializerOptions()
			{
				WriteIndented = prettyPrint,
				DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault,
			};
			options.Converters.Add(new JsonStringEnumConverter());
			options.Converters.Add(new JsonActorUpdateConverter());
			options.Converters.Add(new JsonNameConverter());
			options.Converters.Add(new JsonRigidBodyConverter());
			options.Converters.Add(new JsonGameServerIDConverter());
			options.Converters.Add(new JsonGameModeConverter());
			options.Converters.Add(new JsonStructConverter(options));

			return JsonSerializer.Serialize(replay, options);
		}

		public Replay Deserialize(string json)
		{
			var options = new JsonSerializerOptions();
			options.Converters.Add(new JsonStringEnumConverter());
			options.Converters.Add(new JsonActorUpdateConverter());
			options.Converters.Add(new JsonNameConverter());
			options.Converters.Add(new JsonGameServerIDConverter());
			options.Converters.Add(new JsonGameModeConverter());
			//options.Converters.Add(new JsonRigidBodyConverter());
			//options.Converters.Add(new JsonStructConverter(options));

			var replay = JsonSerializer.Deserialize<Replay>(json, options) ?? throw new JsonException("Replay JSON deserialization failed");

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
						?? throw new System.Text.Json.JsonException($"ClassNetCache for ObjectId {actorUpdate.ObjectId} not found in replay.");
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
