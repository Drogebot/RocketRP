using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketRP.Actors.TAGame
{
	public class CarComponent_FlipCar_TA : CarComponent_TA
	{
		public bool bFlipRight { get; set; }
		public float FlipCarTime { get; set; }
	}
}
