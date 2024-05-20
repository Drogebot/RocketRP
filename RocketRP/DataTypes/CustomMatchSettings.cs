using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace RocketRP.DataTypes
{
	/// This class hasn't been encountered in any replays yet, so I'm sure it's entirely accurate
	public struct CustomMatchSettings
	{
		public string GameTags { get; set; }
		public Name MapName { get; set; }
		public byte GameMode { get; set; }
		public int MaxPlayerCount { get; set; }
		public string ServerName { get; set; }
		public string Password { get; set; }
		public bool bPublic { get; set; }
		public CustomMatchTeamSettings[] TeamSettings { get; set; }

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
			var gameMode = br.ReadByte();
			var maxPlayerCount = br.ReadInt32();
			var serverName = br.ReadString();
			var password = br.ReadString();
			var bPublic = br.ReadBit();
			var teamSettings = new CustomMatchTeamSettings[2];
			teamSettings[0] = CustomMatchTeamSettings.Deserialize(br);
			teamSettings[1] = CustomMatchTeamSettings.Deserialize(br);

			return new CustomMatchSettings(gameTags, mapName, gameMode, maxPlayerCount, serverName, password, bPublic, teamSettings);
		}

		public void Serialize(BitWriter bw, Replay replay)
		{
			bw.Write(GameTags);
			MapName.Serialize(bw);
			bw.Write(GameMode);
			bw.Write(MaxPlayerCount);
			bw.Write(ServerName);
			bw.Write(Password);
			bw.Write(bPublic);
			TeamSettings[0].Serialize(bw);
			TeamSettings[1].Serialize(bw);
		}
	}
}
