using RocketRP.Actors.Engine;
using RocketRP.Actors.ProjectX;
using RocketRP.DataTypes;
using RocketRP.DataTypes.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace RocketRP.Actors.TAGame
{
	public class PRI_TA : PRI_X
	{
		public int KeepUpPossessions { get; set; }
		public int KeepUpClears { get; set; }
		public int KeepUpDenials { get; set; }
		public int SelfDemolitions { get; set; }
		public int BallDemolitions { get; set; }
		public int CarDemolitions { get; set; }
		public int BallDemolitionSaves { get; set; }
		public int PossessionClears { get; set; }
		public int PossessionDenials { get; set; }
		public int PossessionSteals { get; set; }
		public string? CurrentVoiceRoom { get; set; }
		public ObjectTarget<ViralItemActor_TA> ViralItemActor { get; set; }
		public ObjectTarget<PickupTimer_TA> PickupTimer { get; set; }
		public ObjectTarget<Actor> StayAsPartyVoteYes { get; set; } // Is of StayAsPartyVoteYes_TA type, but those don't appear in replays
		public ObjectTarget<Actor> StayAsPartyVoter { get; set; } // Is of StayAsPartyVoter_TA type, but those don't appear in replays
		public int SpectatorShortcut { get; set; }
		public ulong ClubID { get; set; }
		public int BotBannerProductID { get; set; }
		public int BotAvatarProductID { get; set; }
		public Name BotProductName { get; set; }
		public MemberTitleStat SecondaryTitle { get; set; }
		public MemberTitleStat PrimaryTitle { get; set; }
		public ObjectTarget<PRI_TA> ReplacingBotPRI { get; set; }
		public float SteeringSensitivity { get; set; }
		public SkillTierData SkillTier { get; set; }
		public Name Title { get; set; }
		public UniqueNetId PartyLeader { get; set; }
		public ESeverityType QuitSeverity { get; set; }
		public EConnectionQualityState ReplicatedWorstNetQualityBeyondLatency { get; set; }
		[FixedArraySize(0x40)]
		public byte[] PlayerHistoryKey { get; set; } = new byte[0x40];
		public EPawnType PawnType { get; set; }
		public ObjectTarget<CameraSettingsActor_TA> PersistentCamera { get; set; }
		public float TotalGameTimePlayed { get; set; }
		public ClientLoadoutOnlineDatas ClientLoadoutsOnline { get; set; }
		public ClientLoadoutDatas ClientLoadouts { get; set; }
		public ClientLoadoutOnlineData ClientLoadoutOnline { get; set; }
		public ClientLoadoutData ClientLoadout { get; set; }
		public ObjectTarget<GameEvent_TA> ReplicatedGameEvent { get; set; }
		public bool bAbleToStart { get; set; }
		public bool bIdleBanned { get; set; }
		public bool bUsingItems { get; set; }
		public bool bStartVoteToForfeitDisabled { get; set; }
		public bool bIsInSplitScreen { get; set; }
		public bool bIsDistracted { get; set; }
		public bool bReady { get; set; }
		public bool bOnlineLoadoutSet { get; set; }
		public bool bMatchAdmin { get; set; }
		public bool bMatchMVP { get; set; }
		public float TotalIdleTime { get; set; }
		public int MatchBreakoutDamage { get; set; }
		public int MatchDemolishes { get; set; }
		public int MatchShots { get; set; }
		public int MatchSaves { get; set; }
		public int MatchAssists { get; set; }
		public int MatchGoals { get; set; }
		public int MatchScore { get; set; }


		// These are old properties that were removed
		public bool PlayerHistoryValid { get; set; }	// v2.51
		public bool bUsingBehindView { get; set; }
		public bool bUsingSecondaryCamera { get; set; }
		public int RespawnTimeRemaining { get; set; }
		public uint TotalXP { get; set; }
		public int MaxTimeTillItem { get; set; }
		public RepStatTitle RepStatTitles { get; set; }
		public bool bOnlineLoadoutsSet { get; set; }
		public bool bBusy { get; set; }
		public bool bUsingFreecam { get; set; }
		public byte CameraYaw { get; set; }
		public byte CameraPitch { get; set; }
		public int TimeTillItem { get; set; }
		public ProfileCameraSettings CameraSettings { get; set; }
	}
}
