using RocketRP.DataTypes;
using RocketRP.DataTypes.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketRP.Actors.TAGame
{
	public class GameEvent_Soccar_TA : GameEvent_Team_TA
	{
		public ObjectTarget ReplicatedStatEvent { get; set; }
		public ObjectTarget SubRulesArchetype { get; set; }
		public int RoundNum { get; set; }
		public ObjectTarget MVP { get; set; }
		public EConnectionQualityState ReplicatedServerPerformanceState { get; set; }
		public byte ReplicatedScoredOnTeam { get; set; }
		public ETieBreakDecision TieBreakDecision { get; set; }
		public ObjectTarget MatchWinner { get; set; }
		public ObjectTarget GameWinner { get; set; }
		public ObjectTarget ReplayDirector { get; set; }
		public bool bAllowHonorDuels { get; set; }
		public bool bCanDropOnlineRewards { get; set; }
		public bool bClubMatch { get; set; }
		public bool bShowIntroScene { get; set; }
		public bool bMatchEnded { get; set; }
		public bool bNoContest { get; set; }
		public bool bUnlimitedTime { get; set; }
		public bool bOverTime { get; set; }
		public bool bBallHasBeenHit { get; set; }
		public int WaitTimeRemaining { get; set; }
		public int SecondsRemaining { get; set; }
		public int MaxScore { get; set; }
		public int GameTime { get; set; }
		public int SeriesLength { get; set; }


		// These are old properties that were removed
		public ReplicatedMusicStinger ReplicatedMusicStinger { get; set; }
	}
}
