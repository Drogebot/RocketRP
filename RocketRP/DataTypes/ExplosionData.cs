using RocketRP.Actors.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketRP.DataTypes
{
	public struct ExplosionData
	{
		public ObjectTarget<ClassObject>? Goal { get; set; }
		public Vector? Location { get; set; }

		public ExplosionData(ObjectTarget<ClassObject>? goal, Vector? location)
		{
			Goal = goal;
			Location = location;
		}

		public static ExplosionData Deserialize(BitReader br, Replay replay)
		{
			var goal = ObjectTarget<ClassObject>.Deserialize(br);
			var location = Vector.Deserialize(br, replay);

			return new ExplosionData(goal, location);
		}

		public void Serialize(BitWriter bw, Replay replay)
		{
			Goal!.Value.Serialize(bw);
			Location!.Value.Serialize(bw, replay);
		}
	}
}
