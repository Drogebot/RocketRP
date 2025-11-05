using RocketRP.Actors.Engine;
using RocketRP.DataTypes;
using RocketRP.DataTypes.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketRP.Actors.TAGame
{
	public class KeepUpIndicator_TA : Actor
	{
		public override bool HasInitialRotation => true;

		public ObjectTarget<BallKeepUpComponent_TA> ComponentOwner { get; set; }
	}
}
