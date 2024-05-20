using RocketRP.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketRP.Actors.TAGame
{
	public class CarComponent_Dodge_TA : CarComponent_AirActivate_TA
	{
		public Vector DodgeTorque { get; set; }


		// These are old properties that were removed
		public Vector DodgeImpulse { get; set; }
	}
}
