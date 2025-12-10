using RocketRP.Actors.Core;
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
	public class GameEvent_Soccar_TA : GameEvent_Team_TA
	{
		public ObjectTarget<ClassObject> ReplicatedStatEvent { get; set; }
		public ObjectTarget<ClassObject> SubRulesArchetype { get; set; }
		public int RoundNum { get; set; }
		public ObjectTarget<PRI_TA> MVP { get; set; }
		public EConnectionQualityState ReplicatedServerPerformanceState { get; set; }
		public byte ReplicatedScoredOnTeam { get; set; }
		public ETieBreakDecision TieBreakDecision { get; set; }
		public ObjectTarget<Team_TA> MatchWinner { get; set; }
		public ObjectTarget<Team_TA> GameWinner { get; set; }
		public int TotalGameBalls { get; set; }
		public ObjectTarget<Actor> ReplayDirector { get; set; } // Is of ReplayDirector_TA type, but those don't appear in replays
		public int WaitTimeRemaining { get; set; }
		public int SecondsRemaining { get; set; }
		public int MaxScore { get; set; }
		public int GameTime { get; set; }
		public int SeriesLength { get; set; }
		public bool bThistleMatch { get; set; }
		public bool bAllowHonorDuels { get; set; }
		public bool bCanDropOnlineRewards { get; set; }
		public bool bMatchCreatorAdminEnabled { get; set; }
		public bool bFullClubMatch { get; set; }
		public bool bClubMatch { get; set; }
		public bool bReadyToStartGame { get; set; }
		public bool bShowIntroScene { get; set; }
		public bool bDisableCrowdSound { get; set; }
		public bool bMatchEnded { get; set; }
		public bool bNoContest { get; set; }
		public bool bGoalsEnabled { get; set; }
		public bool bUnlimitedTime { get; set; }
		public bool bOverTime { get; set; }
		public bool bBallHasBeenHit { get; set; }
		public bool bShouldSpawnGoalIndicators { get; set; }


		// These are old properties that were removed
		public ReplicatedMusicStinger ReplicatedMusicStinger { get; set; }
	}
}
