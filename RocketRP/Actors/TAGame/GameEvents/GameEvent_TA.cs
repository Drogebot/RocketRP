﻿using RocketRP.Actors.Core;
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
		public int MatchTotalSecondsPlayed { get; set; }
		public ulong MatchStartEpoch { get; set; }
		public int ReplicatedRoundCountDownNumber { get; set; }
		public string? RichPresenceString { get; set; }
		public CustomMatchSettings MatchSettings { get; set; }
		public ObjectTarget<PRI_TA> GameOwner { get; set; }
		public int ReplicatedGameStateTimeRemaining { get; set; }
		public Name ReplicatedStateName { get; set; }
		public byte ReplicatedStateIndex { get; set; }
		public ObjectTarget<Car_TA> ActivatorCar { get; set; }
		public float BotSkill { get; set; }
		public bool bAlwaysShowMatchTypeLabel { get; set; }
		public bool bIsBotMatch { get; set; }
		public bool bCanVoteToForfeit { get; set; }
		public bool bHasLeaveMatchPenalty { get; set; }
		public bool bAllowReadyUp { get; set; }
		public bool bAllowQueueSaveReplay { get; set; }
		public ObjectTarget<ClassObject> MatchTypeClass { get; set; }


		// These are old properties that were removed
		public GameMode GameMode { get; set; }	// The type was changed from MaxEnum to byte at some point
	}
}
