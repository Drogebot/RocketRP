using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketRP.DataTypes
{
	public struct SkillTierData
	{
		public int Tier { get; set; }

		public SkillTierData(int tier)
		{
			Tier = tier;
		}

		public static SkillTierData Deserialize(BitReader br)
		{
			var tier = br.ReadInt32Max(500);    // Not sure why 500 was chosen as the max value, I took it from other parsers

			return new SkillTierData(tier);
		}

		public void Serialize(BitWriter bw)
		{
			bw.Write(Tier, 500);
		}
	}
}
