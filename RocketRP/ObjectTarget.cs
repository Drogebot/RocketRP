using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketRP
{
    public class ObjectTarget
    {
        public bool IsActor { get; set; }
        public int TargetIndex { get; set; }

        public static ObjectTarget Deserialize(BitReader br)
        {
            var objectTarget = new ObjectTarget();

            objectTarget.IsActor = br.ReadBit();
            objectTarget.TargetIndex = br.ReadInt32();

            return objectTarget;
        }
    }
}
