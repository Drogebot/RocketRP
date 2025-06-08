using RocketRP.Actors.TAGame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketRP.DataTypes
{
	public struct DemolishData
	{
		public ObjectTarget<RBActor_TA>? Attacker { get; set; }
		public ObjectTarget<Car_TA>? Victim { get; set; }
		public Vector? AttackerVelocity { get; set; }
		public Vector? VictimVelocity { get; set; }

		public DemolishData(ObjectTarget<RBActor_TA>? attacker, ObjectTarget<Car_TA>? victim, Vector? attackerVelocity, Vector? victimVelocity)
		{
			Attacker = attacker;
			Victim = victim;
			AttackerVelocity = attackerVelocity;
			VictimVelocity = victimVelocity;
		}

		public static DemolishData Deserialize(BitReader br, Replay replay)
		{
			var attacker = ObjectTarget<RBActor_TA>.Deserialize(br);
			var victim = ObjectTarget<Car_TA>.Deserialize(br);
			var attackerVelocity = Vector.Deserialize(br, replay);
			var victimVelocity = Vector.Deserialize(br, replay);

			return new DemolishData(attacker, victim, attackerVelocity, victimVelocity);
		}

		public void Serialize(BitWriter bw, Replay replay)
		{
			Attacker!.Value.Serialize(bw);
			Victim!.Value.Serialize(bw);
			AttackerVelocity!.Value.Serialize(bw, replay);
			VictimVelocity!.Value.Serialize(bw, replay);
		}
	}
}
