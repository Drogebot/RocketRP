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
	public class BallKeepUpComponent_TA : ReplicatedActor_ORS
	{
		public EKeepUpState KeepUpState { get; set; }
		public ObjectTarget<Ball_TA> BallOwner { get; set; }
		public int Score { get; set; }
	}
}
