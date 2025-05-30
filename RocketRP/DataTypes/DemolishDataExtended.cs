using RocketRP.Actors.Core;
using RocketRP.Actors.TAGame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketRP.DataTypes
{
	public struct DemolishDataExtended //class extended from DemolishDataGoalExplosion
	{
		public ObjectTarget<PRI_TA> AttackerPRI { get; set; }
		public ObjectTarget<ClassObject> SelfDemoFX { get; set; }
		public bool bSelfDemolish { get; set; }
		public ObjectTarget<PRI_TA> GoalExplosionOwner { get; set; }
		public ObjectTarget<RBActor_TA> Attacker { get; set; }
		public ObjectTarget<Car_TA> Victim { get; set; }
		public Vector AttackerVelocity { get; set; }
		public Vector VictimVelocity { get; set; }

		public DemolishDataExtended(ObjectTarget<PRI_TA> attackerPRI, ObjectTarget<ClassObject> selfDemoFX, bool bSelfDemolish, ObjectTarget<PRI_TA> goalExplosionOwner, ObjectTarget<RBActor_TA> attacker, ObjectTarget<Car_TA> victim, Vector attackerVelocity, Vector victimVelocity)
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
			var attackerPRI = ObjectTarget<PRI_TA>.Deserialize(br);
			var selfDemoFX = ObjectTarget<ClassObject>.Deserialize(br);
			var bSelfDemolish = br.ReadBit();
			var goalExplosionOwner = ObjectTarget<PRI_TA>.Deserialize(br);
			var attacker = ObjectTarget<RBActor_TA>.Deserialize(br);
			var victim = ObjectTarget<Car_TA>.Deserialize(br);
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
