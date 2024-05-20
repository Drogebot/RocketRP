using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketRP.DataTypes
{
	public struct DemolishDataGoalExplosion
	{
		public ObjectTarget GoalExplosionOwner { get; set; }
		public ObjectTarget Attacker { get; set; }
		public ObjectTarget Victim { get; set; }
		public Vector AttackerVelocity { get; set; }
		public Vector VictimVelocity { get; set; }

		public DemolishDataGoalExplosion(ObjectTarget goalExplosionOwner, ObjectTarget attacker, ObjectTarget victim, Vector attackerVelocity, Vector victimVelocity)
		{
			this.GoalExplosionOwner = goalExplosionOwner;
			this.Attacker = attacker;
			this.Victim = victim;
			this.AttackerVelocity = attackerVelocity;
			this.VictimVelocity = victimVelocity;
		}

		public static DemolishDataGoalExplosion Deserialize(BitReader br, Replay replay)
		{
			var goalExplosionOwner = ObjectTarget.Deserialize(br);
			var attacker = ObjectTarget.Deserialize(br);
			var victim = ObjectTarget.Deserialize(br);
			var attackerVelocity = Vector.Deserialize(br, replay);
			var victimVelocity = Vector.Deserialize(br, replay);

			return new DemolishDataGoalExplosion(goalExplosionOwner, attacker, victim, attackerVelocity, victimVelocity);
		}

		public void Serialize(BitWriter bw, Replay replay)
		{
			GoalExplosionOwner.Serialize(bw);
			Attacker.Serialize(bw);
			Victim.Serialize(bw);
			AttackerVelocity.Serialize(bw, replay);
			VictimVelocity.Serialize(bw, replay);
		}
	}
}
