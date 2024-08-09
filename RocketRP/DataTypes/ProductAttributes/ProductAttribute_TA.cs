using RocketRP.Actors.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketRP.DataTypes.TAGame
{
    public abstract class ProductAttribute_TA : Actors.Core.Object
    {
        public ObjectTarget ObjectTarget { get; set; }
        public string ClassName { get; set; }

        public static ProductAttribute_TA Deserialize(BitReader br, Replay replay)
        {
            var objectTarget = ObjectTarget.Deserialize(br);
            var className = replay.Objects[objectTarget.TargetIndex];

            var type = Type.GetType($"RocketRP.DataTypes.{className}");
            var attribute = (ProductAttribute_TA)type.GetMethod("DeserializeType").Invoke(null, new object[] { br, replay });

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
