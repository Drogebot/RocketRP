using RocketRP.Actors.TAGame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketRP.DataTypes
{
	public struct DemolishDataGoalExplosion //class extended from DemolishData
	{
		public ObjectTarget<PRI_TA>? GoalExplosionOwner { get; set; }
		public ObjectTarget<RBActor_TA>? Attacker { get; set; }
		public ObjectTarget<Car_TA>? Victim { get; set; }
		public Vector? AttackerVelocity { get; set; }
		public Vector? VictimVelocity { get; set; }

		public DemolishDataGoalExplosion(ObjectTarget<PRI_TA>? goalExplosionOwner, ObjectTarget<RBActor_TA>? attacker, ObjectTarget<Car_TA>? victim, Vector? attackerVelocity, Vector? victimVelocity)
		{
			GoalExplosionOwner = goalExplosionOwner;
			Attacker = attacker;
			Victim = victim;
			AttackerVelocity = attackerVelocity;
			VictimVelocity = victimVelocity;
		}

		public static DemolishDataGoalExplosion Deserialize(BitReader br, Replay replay)
		{
			var goalExplosionOwner = ObjectTarget<PRI_TA>.Deserialize(br);
			var attacker = ObjectTarget<RBActor_TA>.Deserialize(br);
			var victim = ObjectTarget<Car_TA>.Deserialize(br);
			var attackerVelocity = Vector.Deserialize(br, replay);
			var victimVelocity = Vector.Deserialize(br, replay);

			return new DemolishDataGoalExplosion(goalExplosionOwner, attacker, victim, attackerVelocity, victimVelocity);
		}

		public void Serialize(BitWriter bw, Replay replay)
		{
			GoalExplosionOwner!.Value.Serialize(bw);
			Attacker!.Value.Serialize(bw);
			Victim!.Value.Serialize(bw);
			AttackerVelocity!.Value.Serialize(bw, replay);
			VictimVelocity!.Value.Serialize(bw, replay);
		}
	}
}
