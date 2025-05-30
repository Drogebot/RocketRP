using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketRP.DataTypes
{
	public struct ExplosionDataExtended
	{
		public ObjectTarget Goal { get; set; }
		public Vector Location { get; set; }
		public ObjectTarget Scorer { get; set; }

		public ExplosionDataExtended(ObjectTarget goal, Vector location, ObjectTarget scorer)
		{
			Goal = goal;
			Location = location;
			Scorer = scorer;
		}

		public static ExplosionDataExtended Deserialize(BitReader br, Replay replay)
		{
			var goal = ObjectTarget.Deserialize(br);
			var location = Vector.Deserialize(br, replay);
			var scorer = ObjectTarget.Deserialize(br);

			return new ExplosionDataExtended(goal, location, scorer);
		}

		public void Serialize(BitWriter bw, Replay replay)
		{
			Goal.Serialize(bw);
			Location.Serialize(bw, replay);
			Scorer.Serialize(bw);
		}
	}
}
