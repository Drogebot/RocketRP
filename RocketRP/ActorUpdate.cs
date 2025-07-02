using RocketRP.Actors.Core;
using RocketRP.Actors.Engine;
using RocketRP.DataTypes;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace RocketRP
{
	public class ActorUpdate
	{
		public int ChannelId { get; set; }
		public ChannelState State { get; set; }
		public int? NameId { get; set; }
		public string? Name { get; set; }
		public ObjectTarget<ClassObject> TypeId { get; set; }
		public string TypeName { get; set; } = null!;
		public int ObjectId { get; set; }
		public string ObjectName { get; set; } = null!;
		public Vector InitialPosition { get; set; }
		public Rotator InitialRotation { get; set; }

		public ClassNetCache ClassNetCache = null!;
		public Type ObjectType = null!;
		public Actor Actor = null!;
		[MemberNotNullWhen(true, nameof(ActorSnapshot))]
		public bool IsSnapshot => ActorSnapshot is not null;
		public Actor? ActorSnapshot { get; set; }
		public HashSet<(ClassNetCacheProperty Property, int Index)> SetProperties = [];
		public HashSet<string>? SetPropertyNames;   // Used for JSON serialization

		public static ActorUpdate Deserialize(BitReader br, Replay replay, Dictionary<int, ActorUpdate> openChannels, bool keepSnapshot = false)
		{
			var channelId = br.ReadInt32((uint)replay.MaxChannels);

			if (br.ReadBit())
			{
				if (br.ReadBit())
				{
					var actorUpdate = new ActorUpdate();
					actorUpdate.ChannelId = channelId;
					actorUpdate.State = ChannelState.Open;

					if ((replay.EngineVersion >= 868 && replay.LicenseeVersion >= 15) ||
						(replay.EngineVersion == 868 && replay.LicenseeVersion == 14 && replay.Properties.MatchType != "Lan"))  // Fixes RLCS 2 replays
					{
						actorUpdate.NameId = br.ReadInt32();
						actorUpdate.Name = replay.Names[actorUpdate.NameId.Value];
					}

					actorUpdate.TypeId = ObjectTarget<ClassObject>.Deserialize(br);
					if (actorUpdate.TypeId.IsActor)
					{
						Console.WriteLine("Warning: New Actor referenced existing Actor as type?");
					}

					actorUpdate.TypeName = replay.Objects[actorUpdate.TypeId.TargetIndex];
					if (!replay.TypeIdToClassNetCache.TryGetValue(actorUpdate.TypeId.TargetIndex, out actorUpdate.ClassNetCache!))
					{
						throw new KeyNotFoundException($"ClassNetCache for TypeId {actorUpdate.TypeId.TargetIndex} ({actorUpdate.TypeName}) not found in replay data. Maybe add it to {nameof(TypeIdToClassNetCacheMapper)}");
					}
					actorUpdate.ClassNetCache = replay.TypeIdToClassNetCache[actorUpdate.TypeId.TargetIndex];
					actorUpdate.ObjectId = actorUpdate.ClassNetCache.ObjectIndex;
					actorUpdate.ObjectName = replay.Objects[actorUpdate.ObjectId];
					actorUpdate.ObjectType = actorUpdate.ClassNetCache.ClassType;

					if (openChannels.TryGetValue(channelId, out var activeActor)) actorUpdate.Actor = activeActor.Actor;
					else actorUpdate.Actor = Actor.CreateInstance(actorUpdate.ObjectType);

					//Console.WriteLine($"Open {channelId} {actorUpdate.ObjectName}");

					if (actorUpdate.Actor.HasInitialPosition)
					{
						actorUpdate.InitialPosition = Vector.Deserialize(br, replay);

						if (actorUpdate.Actor.HasInitialRotation)
						{
							actorUpdate.InitialRotation = Rotator.Deserialize(br);
						}
					}

					return actorUpdate;
				}
				else
				{
					var state = ChannelState.Update;

					var activeActor = openChannels[channelId];

					var nameId = activeActor.NameId;
					var name = activeActor.Name;
					var typeId = activeActor.TypeId;
					var typeName = activeActor.TypeName;
					var classNetCache = activeActor.ClassNetCache;
					var objectId = activeActor.ObjectId;
					var objectName = activeActor.ObjectName;
					var objectType = activeActor.ObjectType;
					var actor = activeActor.Actor;
					//Console.WriteLine($"Update {channelId} {objectName}");

					Actor? actorSnapshot = null;
					if (keepSnapshot) actorSnapshot = Actor.CreateInstance(objectType);

					var setProperties = new HashSet<(ClassNetCacheProperty Property, int Index)>();
					var setActorProperties = actor.SetProperties;
					var maxNumProperties = (uint)classNetCache.NumProperties;

					while (br.ReadBit())
					{
						var propId = br.ReadInt32(maxNumProperties);
						var property = classNetCache.GetPropertyByPropertyId(propId);
						var propInfo = property.PropertyInfo ?? throw new NullReferenceException($"PropertyInfo for property {propId} ({replay.Objects[property.ObjectIndex]}) was not linked");
						var propType = propInfo.PropertyType;

						int valueIndex = 0;
						var propIsArray = propType.IsArray;
						if (propIsArray)
						{
							var arrLength = ((Array)propInfo.GetValue(actor)!).Length;
							valueIndex = br.ReadInt32((uint)arrLength);
						}

						//Console.WriteLine($"\t{propInfo.Name}");
						setProperties.Add((property, valueIndex));
						setActorProperties.Add((property, valueIndex));

						var value = DeserializeActorProperty(br, replay, propType);
						if (propIsArray) ((Array)propInfo.GetValue(actor)!).SetValue(value, valueIndex);
						else propInfo.SetValue(actor, value);
						if (keepSnapshot)
						{
							if (propIsArray) ((Array)propInfo.GetValue(actorSnapshot)!).SetValue(value, valueIndex);
							else propInfo.SetValue(actorSnapshot, value);
						}
					}

					var actorUpdate = new ActorUpdate
					{
						State = state,
						ChannelId = channelId,
						NameId = nameId,
						Name = name,
						TypeId = typeId,
						TypeName = typeName,
						ClassNetCache = classNetCache,
						ObjectId = objectId,
						ObjectName = objectName,
						ObjectType = objectType,
						Actor = actor,
						ActorSnapshot = actorSnapshot,
						SetProperties = setProperties,
					};
					return actorUpdate;
				}
			}
			else
			{
				var activeActor = openChannels[channelId];
				var actorUpdate = new ActorUpdate
				{
					State = ChannelState.Close,
					ChannelId = activeActor.ChannelId,
					NameId = activeActor.NameId,
					Name = activeActor.Name,
					TypeId = activeActor.TypeId,
					TypeName = activeActor.TypeName,
					ClassNetCache = activeActor.ClassNetCache,
					ObjectId = activeActor.ObjectId,
					ObjectName = activeActor.ObjectName,
					ObjectType = activeActor.ObjectType,
					Actor = activeActor.Actor,
				};
				//Console.WriteLine($"Close {channelId} {actorUpdate.ObjectName}");

				return actorUpdate;
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static object? DeserializeActorProperty(BitReader br, Replay replay, Type propertyType)
		{
			if (propertyType.IsArray)  //Fixed Size Array
			{
				propertyType = propertyType.GetElementType()!;
			}

			if (propertyType.IsEnum)
			{
				propertyType = propertyType.GetEnumUnderlyingType();
			}

			if (propertyType == typeof(bool)) return br.ReadBit();
			else if (propertyType == typeof(byte)) return br.ReadByte();
			else if (propertyType == typeof(int)) return br.ReadInt32();
			else if (propertyType == typeof(uint)) return br.ReadUInt32();
			else if (propertyType == typeof(long)) return br.ReadInt64();
			else if (propertyType == typeof(ulong)) return br.ReadUInt64();
			else if (propertyType == typeof(float)) return br.ReadSingle();
			else if (propertyType == typeof(string)) return br.ReadString();
			else
			{
				var methodInfo = propertyType.GetMethod("Deserialize", BindingFlags.Static | BindingFlags.Public, [typeof(BitReader)]) ?? propertyType.GetMethod("Deserialize", BindingFlags.Static | BindingFlags.Public, [typeof(BitReader), typeof(Replay)]);
				if (methodInfo is null) throw new MissingMethodException($"Deserialize method in {propertyType.Name} not found");
				else if (methodInfo.GetParameters().Length == 1) return methodInfo.Invoke(null, [br]);
				else if (methodInfo.GetParameters().Length == 2) return methodInfo.Invoke(null, [br, replay]);
				else throw new MissingMethodException($"Deserialize method in {propertyType.Name} does not have the correct parameters");
			}
		}

		public void Serialize(BitWriter bw, Replay replay)
		{
			bw.Write(ChannelId, (uint)replay.MaxChannels);

			if (State == ChannelState.Open)
			{
				bw.Write(true);
				bw.Write(true);

				if ((replay.EngineVersion >= 868 && replay.LicenseeVersion >= 15) ||
					(replay.EngineVersion == 868 && replay.LicenseeVersion == 14 && replay.Properties.MatchType != "Lan"))  // Fixes RLCS 2 replays
				{
					bw.Write(NameId ?? 0);
				}

				TypeId.Serialize(bw);

				if (Actor.HasInitialPosition)
				{
					InitialPosition.Serialize(bw, replay);

					if (Actor.HasInitialRotation)
					{
						InitialRotation.Serialize(bw);
					}
				}

				return;
			}
			else if (State == ChannelState.Update)
			{
				bw.Write(true);
				bw.Write(false);

				ClassNetCache ??= replay.TypeIdToClassNetCache[TypeId.TargetIndex];
				var maxPropId = (uint)ClassNetCache.NumProperties;

				foreach (var property in SetProperties)
				{
					object? value = property.Property.PropertyInfo.GetValue(ActorSnapshot);
					if (property.Property.PropertyInfo.PropertyType.IsArray)
					{
						var arr = (Array)value!;
						value = arr.GetValue(property.Index)!;
						// This check could be removed if I ever remove nullability of struct properties
						if (value.Equals(Activator.CreateInstance(property.Property.PropertyInfo.PropertyType.GetElementType()!))) continue;
						bw.Write(true);
						bw.Write(property.Property.PropertyId, maxPropId);
						bw.Write(property.Index, (uint)arr.Length);
					}
					else
					{
						bw.Write(true);
						bw.Write(property.Property.PropertyId, maxPropId);
					}
					SerializeActorProperty(bw, replay, property.Property.PropertyInfo.PropertyType, value);
				}

				bw.Write(false);
				return;
			}
			else if (State == ChannelState.Close)
			{
				bw.Write(false);
				return;
			}

			throw new Exception($"Unknown channel state: {State}");
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void SerializeActorProperty(BitWriter bw, Replay replay, Type valueType, object? value)
		{
			if (valueType.IsArray)
			{
				valueType = valueType.GetElementType()!;
			}

			if (valueType.IsEnum)
			{
				valueType = valueType.GetEnumUnderlyingType();
			}

			if (valueType == typeof(bool)) bw.Write((bool)value!);
			else if (valueType == typeof(byte)) bw.Write((byte)value!);
			else if (valueType == typeof(int)) bw.Write((int)value!);
			else if (valueType == typeof(uint)) bw.Write((uint)value!);
			else if (valueType == typeof(long)) bw.Write((long)value!);
			else if (valueType == typeof(ulong)) bw.Write((ulong)value!);
			else if (valueType == typeof(float)) bw.Write((float)value!);
			else if (valueType == typeof(string)) bw.Write((string?)value);
			else
			{
				var methodInfo = valueType.GetMethod("Serialize", BindingFlags.Instance | BindingFlags.Public, [typeof(BitWriter)]) ?? valueType.GetMethod("Serialize", BindingFlags.Instance | BindingFlags.Public, [typeof(BitWriter), typeof(Replay)]);
				if (methodInfo is null) throw new MissingMethodException($"Serialize method in {valueType.Name} not found");
				else if (methodInfo.GetParameters().Length == 1) methodInfo.Invoke(value, [bw]);
				else if (methodInfo.GetParameters().Length == 2) methodInfo.Invoke(value, [bw, replay]);
				else throw new MissingMethodException($"Serialize method in {valueType.Name} does not have the correct parameters");
			}
		}
	}

	public enum ChannelState : byte
	{
		Close,
		Update,
		Unknown,
		Open,
	}
}
