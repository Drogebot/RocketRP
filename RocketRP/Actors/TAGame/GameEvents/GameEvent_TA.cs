using RocketRP.Actors.Engine;
using RocketRP.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace RocketRP.Actors.TAGame
{
	public class GameEvent_TA : Actor
	{
		public int ReplicatedRoundCountDownNumber { get; set; }
		public CustomMatchSettings MatchSettings { get; set; }  // This property hasn't been encountered in any replays yet, so the type it's code might not be entirely accurate
		public ObjectTarget GameOwner { get; set; }
		public int ReplicatedGameStateTimeRemaining { get; set; }
		public Name ReplicatedStateName { get; set; }
		public byte ReplicatedStateIndex { get; set; }
		public ObjectTarget ActivatorCar { get; set; }
		public float BotSkill { get; set; }
		public bool bIsBotMatch { get; set; }
		public bool bCanVoteToForfeit { get; set; }
		public bool bHasLeaveMatchPenalty { get; set; }
		public bool bAllowReadyUp { get; set; }
		public ObjectTarget MatchTypeClass { get; set; }


		// These are old properties that were removed
		public GameMode GameMode { get; set; }	// The type was changed from MaxEnum to byte at some point
	}
}
