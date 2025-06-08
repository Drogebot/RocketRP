using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace RocketRP.DataTypes
{
	/// This type no longer exists inside Rocket League, so I have no clue what the official names are. CustomMatchSettings takes its place in the current version of Rocket League.
	public struct PrivateMatchSettings
	{
		public string? GameTags { get; set; }
		public Name? MapName { get; set; }
		public int? MaxPlayerCount { get; set; }
		public string? ServerName { get; set; }
		public string? Password { get; set; }
		public bool? bPublic { get; set; }

		public PrivateMatchSettings(string? gameTags, Name? mapName, int? maxPlayerCount, string? serverName, string? password, bool? bPublic)
		{
			GameTags = gameTags;
			MapName = mapName;
			MaxPlayerCount = maxPlayerCount;
			ServerName = serverName;
			Password = password;
			this.bPublic = bPublic;
		}

		public static PrivateMatchSettings Deserialize(BitReader br, Replay replay)
		{
			var gameTags = br.ReadString();
			var mapName = Name.Deserialize(br, replay);
			var maxPlayerCount = br.ReadInt32();
			var serverName = br.ReadString();
			var password = br.ReadString();
			var bPublic = br.ReadBit();

			return new PrivateMatchSettings(gameTags, mapName, maxPlayerCount, serverName, password, bPublic);
		}

		public void Serialize(BitWriter bw, Replay replay)
		{
			bw.Write(GameTags);
			MapName!.Value.Serialize(bw, replay);
			bw.Write(MaxPlayerCount);
			bw.Write(ServerName);
			bw.Write(Password);
			bw.Write(bPublic);
		}
	}
}
