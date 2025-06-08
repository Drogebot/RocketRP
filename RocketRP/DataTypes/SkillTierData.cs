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
		public bool? bReplicated { get; set; }

		public SkillTierData(byte? tier, byte? placementMatchesPlayed, bool? bReplicated)
		{
			Tier = tier;
			PlacementMatchesPlayed = placementMatchesPlayed;
			this.bReplicated = bReplicated;
		}

		public static SkillTierData Deserialize(BitReader br)
		{
			var tier = br.ReadByte();
			byte? placementMatchesPlayed = null;
			if(false)  // Find the correct condition
			{
				placementMatchesPlayed = br.ReadByte();
			}
			var bReplicated = br.ReadBit();

			return new SkillTierData(tier, placementMatchesPlayed, bReplicated);
		}

		public void Serialize(BitWriter bw)
		{
			bw.Write(Tier);
			bw.Write(bReplicated);
		}
	}
}
