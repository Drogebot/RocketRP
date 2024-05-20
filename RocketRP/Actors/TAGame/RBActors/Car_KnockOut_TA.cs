using RocketRP.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketRP.Actors.TAGame
{
	public class Car_KnockOut_TA : Car_TA
	{
		public ObjectTarget UsedAttackComponent { get; set; }
		public ImpulseData ReplicatedImpulse { get; set; }
		public byte ReplicatedStateChanged { get; set; }
		public Name ReplicatedStateName { get; set; }
	}
}
