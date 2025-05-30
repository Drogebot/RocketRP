using RocketRP.Actors.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketRP.Actors.TAGame
{
	public class Stunlock_TA : Actor
	{
		public float MashTime { get; set; }
		public float StunTimeRemaining { get; set; }
		public float MaxStunTime { get; set; }
		public ObjectTarget<Car_KnockOut_TA> Car { get; set; }
	}
}
