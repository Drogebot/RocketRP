using RocketRP.Actors.Engine;
using RocketRP.DataTypes.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketRP.Actors.TAGame
{
	public class ViralItemActor_TA : Actor
	{
		public EInfectedType ClientFXInfectedType { get; set; }
		public EInfectedType InfectedStatus { get; set; }
	}
}
