using RocketRP.Actors.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketRP.Actors.TAGame
{
    public abstract class ProductAttribute_TA : Core.Object
    {
        public ObjectTarget<ClassObject>? ObjectTarget { get; set; }
        public string? ClassName { get; set; }

        public static ProductAttribute_TA Deserialize(BitReader br, Replay replay)
        {
            var objectTarget = RocketRP.ObjectTarget<ClassObject>.Deserialize(br);
            var className = replay.Objects[objectTarget.TargetIndex];

            var type = Type.GetType($"RocketRP.Actors.{className}");
            var attribute = (ProductAttribute_TA?)type?.GetMethod("DeserializeType")?.Invoke(null, [br, replay]) ?? throw new NullReferenceException();

            attribute.ObjectTarget = objectTarget;
			attribute.ClassName = className;

			return attribute;
		}

		public virtual void Serialize(BitWriter bw, Replay replay)
		{
			ObjectTarget!.Value.Serialize(bw);
		}
    }
}
