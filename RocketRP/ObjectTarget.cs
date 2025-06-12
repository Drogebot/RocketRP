using RocketRP.Actors.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketRP
{
    public struct ObjectTarget<TObject> : IObjectTarget where TObject : Actors.Core.Object
    {
        public bool IsActor { get; set; }
        public int TargetIndex { get; set; }

		public TObject? GetObject(List<Actors.Core.Object> objects)
		{
			if (objects[TargetIndex] is TObject obj) return obj;
			else Console.WriteLine("ObjectTarget references wrong object type!");
			return null;
		}

		public ClassObject? GetObject(List<string> objectTypes)
		{
            return new ClassObject(objectTypes[TargetIndex]);
		}

		public static ObjectTarget<TObject> Deserialize(BinaryReader br)
		{
			return new ObjectTarget<TObject>
            {
                IsActor = false,
                TargetIndex = br.ReadInt32(),
            };
        }

        public void Serialize(BinaryWriter bw)
        {
            bw.Write(TargetIndex);
        }

        public TObject? GetObject(Dictionary<int, ActorUpdate> actorUpdates, List<string> objectTypes)
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

		public static ObjectTarget<TObject> Deserialize(BitReader br)
        {
            return new ObjectTarget<TObject>
            {
                IsActor = br.ReadBit(),
                TargetIndex = br.ReadInt32(),
            };
        }

        public void Serialize(BitWriter bw)
		{
			bw.Write(IsActor);
			bw.Write(TargetIndex);
        }
    }

    public interface IObjectTarget
    {
        public void Serialize(BinaryWriter bw);
    }
}
