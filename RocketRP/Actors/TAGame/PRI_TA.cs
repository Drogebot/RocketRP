﻿using RocketRP.Actors.ProjectX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml;
using RocketRP.DataTypes;
using RocketRP.DataTypes.Enums;

namespace RocketRP.Actors.TAGame
{
    public class PRI_TA : PRI_X
	{
		public string CurrentVoiceRoom { get; set; }
		public ObjectTarget PickupTimer { get; set; }
		public ObjectTarget StayAsPartyVoteYes { get; set; }
		public ObjectTarget StayAsPartyVoter { get; set; }
		public int SpectatorShortcut { get; set; }
		public ulong ClubID { get; set; }
		public int BotBannerProductID { get; set; }
		public int BotAvatarProductID { get; set; }
		public Name BotProductName { get; set; }
		public MemberTitleStat SecondaryTitle { get; set; }
		public MemberTitleStat PrimaryTitle { get; set; }
		public ObjectTarget ReplacingBotPRI { get; set; }
		public float SteeringSensitivity { get; set; }
		public SkillTierData SkillTier { get; set; }
		public Name Title { get; set; }
		public PartyLeader PartyLeader { get; set; }    // This is normally a UniqueNetId but works differently in this context...
		public ESeverityType QuitSeverity { get; set; }
		public EConnectionQualityState ReplicatedWorstNetQualityBeyondLatency { get; set; }
		public HistoryKey PlayerHistoryKey { get; set; }   // In the original code, this is a byte[0x40]
		public EPawnType PawnType { get; set; }
		public ObjectTarget PersistentCamera { get; set; }
		public ClientLoadoutOnlineDatas ClientLoadoutsOnline { get; set; }
		public ClientLoadoutDatas ClientLoadouts { get; set; }
		public ClientLoadoutOnlineData ClientLoadoutOnline { get; set; }
		public ClientLoadoutData ClientLoadout { get; set; }
		public ObjectTarget ReplicatedGameEvent { get; set; }
		public bool bAbleToStart { get; set; }
		public bool bIdleBanned { get; set; }
		public bool PlayerHistoryValid { get; set; }
		public bool bUsingItems { get; set; }
		public bool bStartVoteToForfeitDisabled { get; set; }
		public bool bIsInSplitScreen { get; set; }
		public bool bIsDistracted { get; set; }
		public bool bReady { get; set; }
		public bool bOnlineLoadoutSet { get; set; }
		public bool bMatchAdmin { get; set; }
		public bool bMatchMVP { get; set; }
		public int MatchBreakoutDamage { get; set; }
		public int MatchShots { get; set; }
		public int MatchSaves { get; set; }
		public int MatchAssists { get; set; }
		public int MatchGoals { get; set; }
		public int MatchScore { get; set; }


		// These are old properties that were removed
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
