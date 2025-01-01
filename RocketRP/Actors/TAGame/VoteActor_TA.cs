using RocketRP.Actors.Engine;
using RocketRP.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketRP.Actors.TAGame
{
	/// <summary>
	/// This class doesn't appear in replays anymore because it isn't relevant for the replay itself
	/// </summary>
	public class VoteActor_TA : Actor
	{
		//public ArrayProperty<Voter> ReplicatedVoter { get; set; } = new ArrayProperty<Voter>(0x8);
		public bool bFinished { get; set; }


		// These are old properties that were removed
		public int TimeRemaining { get; set; }
		public byte Counts { get; set; }
	}
}
