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
		public ObjectTarget<PRI_TA>? AttackerPRI { get; set; }
		public ObjectTarget<ClassObject>? SelfDemoFX { get; set; }
		public bool? bSelfDemolish { get; set; }
		public ObjectTarget<PRI_TA>? GoalExplosionOwner { get; set; }
		public ObjectTarget<RBActor_TA>? Attacker { get; set; }
		public ObjectTarget<Car_TA>? Victim { get; set; }
		public Vector? AttackerVelocity { get; set; }
		public Vector? VictimVelocity { get; set; }

		public DemolishDataExtended(ObjectTarget<PRI_TA>? attackerPRI, ObjectTarget<ClassObject>? selfDemoFX, bool? bSelfDemolish, ObjectTarget<PRI_TA>? goalExplosionOwner, ObjectTarget<RBActor_TA>? attacker, ObjectTarget<Car_TA>? victim, Vector? attackerVelocity, Vector? victimVelocity)
		{
			AttackerPRI = attackerPRI;
			SelfDemoFX = selfDemoFX;
			this.bSelfDemolish = bSelfDemolish;
			GoalExplosionOwner = goalExplosionOwner;
			Attacker = attacker;
			Victim = victim;
			AttackerVelocity = attackerVelocity;
			VictimVelocity = victimVelocity;
		}

		public static DemolishDataExtended Deserialize(BitReader br, Replay replay)
		{
			var attackerPRI = ObjectTarget<PRI_TA>.Deserialize(br, replay);
			var selfDemoFX = ObjectTarget<ClassObject>.Deserialize(br, replay);
			var bSelfDemolish = br.ReadBit();
			var goalExplosionOwner = ObjectTarget<PRI_TA>.Deserialize(br, replay);
			var attacker = ObjectTarget<RBActor_TA>.Deserialize(br, replay);
			var victim = ObjectTarget<Car_TA>.Deserialize(br, replay);
			var attackerVelocity = Vector.Deserialize(br, replay);
			var victimVelocity = Vector.Deserialize(br, replay);

			return new DemolishDataExtended(attackerPRI, selfDemoFX, bSelfDemolish, goalExplosionOwner, attacker, victim, attackerVelocity, victimVelocity);
		}

		public void Serialize(BitWriter bw, Replay replay)
		{
			AttackerPRI!.Value.Serialize(bw, replay);
			SelfDemoFX!.Value.Serialize(bw, replay);
			bw.Write(bSelfDemolish);
			GoalExplosionOwner!.Value.Serialize(bw, replay);
			Attacker!.Value.Serialize(bw, replay);
			Victim!.Value.Serialize(bw, replay);
			AttackerVelocity!.Value.Serialize(bw, replay);
			VictimVelocity!.Value.Serialize(bw, replay);
		}
	}
}
