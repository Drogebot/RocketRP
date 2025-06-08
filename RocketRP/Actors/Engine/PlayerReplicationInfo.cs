using RocketRP.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace RocketRP.Actors.Engine
{
	public class PlayerReplicationInfo : ReplicationInfo
	{
		public string? RemoteUserData { get; set; }
		public UniqueNetId UniqueId { get; set; }
		public bool bTimedOut { get; set; }
		public bool bFromPreviousLevel { get; set; }
		public bool bIsInactive { get; set; }
		public bool bBot { get; set; }
		public bool bOutOfLives { get; set; }
		public bool bReadyToPlay { get; set; }
		public bool bWaitingPlayer { get; set; }
		public bool bOnlySpectator { get; set; }
		public bool bIsSpectator { get; set; }
		public bool bAdmin { get; set; }
		public ObjectTarget<TeamInfo> Team { get; set; }
		public int PlayerID { get; set; }
		public string? PlayerName { get; set; }
		public byte Ping { get; set; }
		public int Deaths { get; set; }
		public int Score { get; set; }
	}
}
