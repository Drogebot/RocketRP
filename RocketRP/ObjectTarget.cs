using RocketRP.Actors.Core;
using System;
using System.Collections.Generic;

namespace RocketRP
{
	public struct ObjectTarget<TObject> : IObjectTarget where TObject : Actors.Core.Object
	{
		public bool IsActor { get; set; }
		public int TargetIndex { get; set; }

		public ObjectTarget(bool isActor, int targetIndex)
		{
			IsActor = isActor;
			TargetIndex = targetIndex;
		}

		public readonly TObject? GetObject(List<Actors.Core.Object> objects)
		{
			if (objects[TargetIndex] is TObject obj) return obj;
			else Console.WriteLine("ObjectTarget references wrong object type!");
			return null;
		}

		public readonly ClassObject? GetObject(List<string> objectTypes)
		{
			return new ClassObject(objectTypes[TargetIndex]);
		}

		public static ObjectTarget<TObject> Deserialize(BinaryReader br)
		{
			var targetIndex = br.ReadInt32();
			return new ObjectTarget<TObject>(false, targetIndex);
		}

		public readonly void Serialize(BinaryWriter bw)
		{
			bw.Write(TargetIndex);
		}

		public readonly TObject? GetObject(Dictionary<int, ActorUpdate> actorUpdates, List<string> objectTypes)
		{
			if (TargetIndex == 0) return null;
			if (IsActor)
			{
				if (actorUpdates[TargetIndex].Actor is TObject obj) return obj;
				else Console.WriteLine("ObjectTarget references wrong actor type!");
			}
			else
			{
				if (new ClassObject(objectTypes[TargetIndex]) is TObject obj) return obj;
				else Console.WriteLine("ObjectTarget references wrong object type!");
			}

			return null;
		}

		public static ObjectTarget<TObject> Deserialize(BitReader br, Replay replay)
		{
			var isActor = br.ReadBit();
			var targetIndex = br.ReadInt32();

			return new ObjectTarget<TObject>(isActor, targetIndex);
		}

		public readonly void Serialize(BitWriter bw, Replay replay)
		{
			bw.Write(IsActor);
			bw.Write(TargetIndex);
		}
	}

	public interface IObjectTarget
	{
		public bool IsActor { get; set; }
		public int TargetIndex { get; set; }
		
		public void Serialize(BinaryWriter bw);
		public void Serialize(BitWriter bw, Replay replay);
	}
}
