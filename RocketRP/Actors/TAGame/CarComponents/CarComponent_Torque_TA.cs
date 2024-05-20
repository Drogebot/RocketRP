using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketRP.Actors.TAGame
{
	public class CarComponent_Torque_TA : CarComponent_TA
	{
		public int ReplicatedTorqueInput { get; set; }
		public float TorqueScale { get; set; }
	}
}
