using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketRP.DataTypes
{
	public struct DemolishDataExtended //class extended from DemolishDataGoalExplosion
	{
		public ObjectTarget AttackerPRI { get; set; }
		public ObjectTarget SelfDemoFX { get; set; }
		public bool bSelfDemolish { get; set; }
		public ObjectTarget GoalExplosionOwner { get; set; }
		public ObjectTarget Attacker { get; set; }
		public ObjectTarget Victim { get; set; }
		public Vector AttackerVelocity { get; set; }
		public Vector VictimVelocity { get; set; }

		public DemolishDataExtended(ObjectTarget attackerPRI, ObjectTarget selfDemoFX, bool bSelfDemolish, ObjectTarget goalExplosionOwner, ObjectTarget attacker, ObjectTarget victim, Vector attackerVelocity, Vector victimVelocity)
		{
			this.AttackerPRI = attackerPRI;
			this.SelfDemoFX = selfDemoFX;
			this.bSelfDemolish = bSelfDemolish;
			this.GoalExplosionOwner = goalExplosionOwner;
			this.Attacker = attacker;
			this.Victim = victim;
			this.AttackerVelocity = attackerVelocity;
			this.VictimVelocity = victimVelocity;
		}

		public static DemolishDataExtended Deserialize(BitReader br, Replay replay)
		{
			var attackerPRI = ObjectTarget.Deserialize(br);
			var selfDemoFX = ObjectTarget.Deserialize(br);
			var bSelfDemolish = br.ReadBit();
			var goalExplosionOwner = ObjectTarget.Deserialize(br);
			var attacker = ObjectTarget.Deserialize(br);
			var victim = ObjectTarget.Deserialize(br);
			var attackerVelocity = Vector.Deserialize(br, replay);
			var victimVelocity = Vector.Deserialize(br, replay);

			return new DemolishDataExtended(attackerPRI, selfDemoFX, bSelfDemolish, goalExplosionOwner, attacker, victim, attackerVelocity, victimVelocity);
		}

		public void Serialize(BitWriter bw, Replay replay)
		{
			AttackerPRI.Serialize(bw);
			SelfDemoFX.Serialize(bw);
			bw.Write(bSelfDemolish);
			GoalExplosionOwner.Serialize(bw);
			Attacker.Serialize(bw);
			Victim.Serialize(bw);
			AttackerVelocity.Serialize(bw, replay);
			VictimVelocity.Serialize(bw, replay);
		}
	}
}
