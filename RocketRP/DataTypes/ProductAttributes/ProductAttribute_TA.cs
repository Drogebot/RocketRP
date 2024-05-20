using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketRP.DataTypes.ProductAttributes
{
    public abstract class ProductAttribute_TA
    {
        public ObjectTarget ObjectTarget { get; set; }
        public string ClassName { get; set; }

        public static ProductAttribute_TA Deserialize(BitReader br, Replay replay)
        {
            var objectTarget = ObjectTarget.Deserialize(br);
            var className = replay.Objects[br.ReadInt32()];

            var type = Type.GetType($"RocketRP.Actors.{className}");
            var attribute = (ProductAttribute_TA)type.GetMethod("Deserialize").Invoke(null, [br, replay]);

            attribute.ObjectTarget = objectTarget;
			attribute.ClassName = className;

			return attribute;
		}

		public virtual void Serialize(BitWriter bw, Replay replay)
		{
			ObjectTarget.Serialize(bw);
		}
    }
}
