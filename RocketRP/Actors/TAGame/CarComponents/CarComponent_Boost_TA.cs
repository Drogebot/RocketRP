using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketRP.Actors.TAGame
{
	public class CarComponent_Boost_TA : CarComponent_AirActivate_TA
	{
		public byte ReplicatedBoostAmount { get; set; }
		public int UnlimitedBoostRefCount { get; set; }
		public float RechargeDelay { get; set; }
		public float RechargeRate { get; set; }
		public bool bNoBoost { get; set; }
		public bool bRechargeGroundOnly { get; set; }
		public float BoostModifier { get; set; }
		public float CurrentBoostAmount { get; set; }
		public float StartBoostAmount { get; set; }


		// These are old properties that were removed
		public bool bUnlimitedBoost { get; set; }
		public int ReplicatedBoost { get; set; }
	}
}
