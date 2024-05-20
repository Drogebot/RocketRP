using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketRP.DataTypes
{
	public struct ExplosionData
	{
		public ObjectTarget Goal { get; set; }
		public Vector Location { get; set; }

		public ExplosionData(ObjectTarget goal, Vector location)
		{
			Goal = goal;
			Location = location;
		}

		public static ExplosionData Deserialize(BitReader br, Replay replay)
		{
			var goal = ObjectTarget.Deserialize(br);
			var location = Vector.Deserialize(br, replay);

			return new ExplosionData(goal, location);
		}

		public void Serialize(BitWriter bw, Replay replay)
		{
			Goal.Serialize(bw);
			Location.Serialize(bw, replay);
		}
	}
}
