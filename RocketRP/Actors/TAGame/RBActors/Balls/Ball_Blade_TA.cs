using RocketRP.Actors.Core;
using RocketRP.DataTypes;
using System;
using System.Collections.Generic;
using System.Text;

namespace RocketRP.Actors.TAGame
{
	public class Ball_Blade_TA : Ball_Fire_TA
	{
		public float ExtraSpeed { get; set; }
		public ObjectTarget<PRI_TA> ClosestTargetPRI { get; set; }
		public ObjectTarget<PRI_TA> TargetPRI { get; set; }
	}
}
