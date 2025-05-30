using RocketRP.Actors.Core;
using RocketRP.Actors.TAGame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketRP.DataTypes
{
	public struct DemolishData2 //class extended from DemolishData
	{
		public ObjectTarget<ClassObject> CustomDemoFX { get; set; }
		public ObjectTarget<RBActor_TA> Attacker { get; set; }
		public ObjectTarget<Car_TA> Victim { get; set; }
		public Vector AttackerVelocity { get; set; }
		public Vector VictimVelocity { get; set; }

		public DemolishData2(ObjectTarget<ClassObject> customDemoFX, ObjectTarget<RBActor_TA> attacker, ObjectTarget<Car_TA> victim, Vector attackerVelocity, Vector victimVelocity)
		{
			CustomDemoFX = customDemoFX;
			Attacker = attacker;
			Victim = victim;
			AttackerVelocity = attackerVelocity;
			VictimVelocity = victimVelocity;
		}

		public static DemolishData2 Deserialize(BitReader br, Replay replay)
		{
			var customDemoFX = ObjectTarget<ClassObject>.Deserialize(br);
			var attacker = ObjectTarget<RBActor_TA>.Deserialize(br);
			var victim = ObjectTarget<Car_TA>.Deserialize(br);
			var attackerVelocity = Vector.Deserialize(br, replay);
			var victimVelocity = Vector.Deserialize(br, replay);

			return new DemolishData2(customDemoFX, attacker, victim, attackerVelocity, victimVelocity);
		}

		public void Serialize(BitWriter bw, Replay replay)
		{
			CustomDemoFX.Serialize(bw);
			Attacker.Serialize(bw);
			Victim.Serialize(bw);
			AttackerVelocity.Serialize(bw, replay);
			VictimVelocity.Serialize(bw, replay);
		}
	}
}
