using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketRP.DataTypes
{
	public struct SkillTierData
	{
		public byte? Tier { get; set; }
		public byte? PlacementMatchesPlayed { get; set; }
		public float? MMR { get; set; }
		public float? PrevMMR { get; set; }
		public bool? bReplicated { get; set; }

		public SkillTierData(byte? tier, byte? placementMatchesPlayed, float? mmr, float? prevMMR, bool? bReplicated)
		{
			Tier = tier;
			PlacementMatchesPlayed = placementMatchesPlayed;
			MMR = mmr;
			PrevMMR = prevMMR;
			this.bReplicated = bReplicated;
		}

		public static SkillTierData Deserialize(BitReader br, Replay replay)
		{
			var tier = br.ReadByte();
			byte? placementMatchesPlayed = null;
			if(replay.NetVersion >= 10)  // Find the correct condition (SkillTierData doesn't seem to appear in replays anyway)
			{
				placementMatchesPlayed = br.ReadByte();
			}
			float? mmr = null;
			float? prevMMR = null;
			if(replay.NetVersion >= 11)
			{
				mmr = br.ReadSingle();
				prevMMR = br.ReadSingle();
			}
			var bReplicated = br.ReadBit();

			return new SkillTierData(tier, placementMatchesPlayed, mmr, prevMMR, bReplicated);
		}

		public void Serialize(BitWriter bw, Replay replay)
		{
			bw.Write(Tier);
			if (replay.NetVersion >= 10)
			{
				bw.Write(PlacementMatchesPlayed);
			}
			if (replay.NetVersion >= 11)
			{
				bw.Write(MMR);
				bw.Write(PrevMMR);
			}
			bw.Write(bReplicated);
		}
	}
}
