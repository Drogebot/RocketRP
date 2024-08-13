using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketRP
{
    public struct ObjectTarget
    {
        public bool IsActor { get; set; }
        public int TargetIndex { get; set; }

        public static ObjectTarget Deserialize(BinaryReader br)
        {
            return new ObjectTarget
            {
                IsActor = false,
                TargetIndex = br.ReadInt32(),
            };
        }

        public void Serialize(BinaryWriter bw)
        {
            bw.Write(TargetIndex);
        }

        public static ObjectTarget Deserialize(BitReader br)
        {
            var objectTarget = new ObjectTarget();

            objectTarget.IsActor = br.ReadBit();
            objectTarget.TargetIndex = br.ReadInt32();

            return objectTarget;
        }

        public void Serialize(BitWriter bw)
		{
			bw.Write(IsActor);
			bw.Write(TargetIndex);
		}
	}
}
