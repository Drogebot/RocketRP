using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace RocketRP.DataTypes
{
	public struct CustomMatchSettings
	{
		public string? GameTags { get; set; }
		public Name? MapName { get; set; }
		public byte? GameMode { get; set; }
		public int? MaxPlayerCount { get; set; }
		public string? ServerName { get; set; }
		public string? Password { get; set; }
		public bool? bPublic { get; set; }
		public CustomMatchTeamSettings[]? TeamSettings { get; set; }

		public CustomMatchSettings(string gameTags, Name mapName, byte gameMode, int maxPlayerCount, string serverName, string password, bool bPublic, CustomMatchTeamSettings[] teamSettings)
		{
			GameTags = gameTags;
			MapName = mapName;
			GameMode = gameMode;
			MaxPlayerCount = maxPlayerCount;
			ServerName = serverName;
			Password = password;
			this.bPublic = bPublic;
			TeamSettings = teamSettings;
		}

		public static CustomMatchSettings Deserialize(BitReader br, Replay replay)
		{
			var gameTags = br.ReadString();
			var mapName = Name.Deserialize(br, replay);
			var gameMode = (byte)0;
			//	This property hasn't been encountered in any replays yet, so I'm not sure where the cutoff is. Probably when `GameEvent_SoccarPrivate_TA` was removed as well
			//	The following table should return false for the condition, but I'm not sure what the correct condition is
			//	EngineVersion | licenseeVersion | NetVersion | Changelist
			//	     868      |       10        |     0      |     0     
			if (false)	// Find the correct condition
			{
				gameMode = br.ReadByte();
			}
			var maxPlayerCount = br.ReadInt32();
			var serverName = br.ReadString();
			var password = br.ReadString();
			var bPublic = br.ReadBit();
			var teamSettings = new CustomMatchTeamSettings[2];
			if (false)	// Find the correct condition
			{
				teamSettings[0] = CustomMatchTeamSettings.Deserialize(br);
				teamSettings[1] = CustomMatchTeamSettings.Deserialize(br);
			}

			return new CustomMatchSettings(gameTags, mapName, gameMode, maxPlayerCount, serverName, password, bPublic, teamSettings);
		}

		public void Serialize(BitWriter bw, Replay replay)
		{
			bw.Write(GameTags);
			MapName.Value.Serialize(bw);
			if (false)	// Find the correct condition
			{
				bw.Write(GameMode.Value);
			}
			bw.Write(MaxPlayerCount.Value);
			bw.Write(ServerName);
			bw.Write(Password);
			bw.Write(bPublic.Value);
			if (false)	// Find the correct condition
			{
				TeamSettings[0].Serialize(bw);
				TeamSettings[1].Serialize(bw);
			}
		}
	}
}
