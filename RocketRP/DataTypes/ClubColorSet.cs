using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketRP.DataTypes
{
	/// The original version in RL does first the 2 bytes and then the 2 bools, so this might be wrong?
	public struct ClubColorSet
	{
		public bool? bTeamColorSet { get; set; }
		public byte? TeamColorID { get; set; }
		public bool? bCustomColorSet { get; set; }
		public byte? CustomColorID { get; set; }

		public ClubColorSet(bool? bTeamColorSet, byte? teamColorID, bool? bCustomColorSet, byte? customColorID)
		{
			this.bTeamColorSet = bTeamColorSet;
			TeamColorID = teamColorID;
			this.bCustomColorSet = bCustomColorSet;
			CustomColorID = customColorID;
		}

		public static ClubColorSet Deserialize(BitReader br)
		{
			bool bTeamColorSet = br.ReadBit();
			byte TeamColorID = br.ReadByte();
			bool bCustomColorSet = br.ReadBit();
			byte CustomColorID = br.ReadByte();

			return new ClubColorSet(bTeamColorSet, TeamColorID, bCustomColorSet, CustomColorID);
		}

		public void Serialize(BitWriter bw)
		{
			bw.Write(bTeamColorSet);
			bw.Write(TeamColorID);
			bw.Write(bCustomColorSet);
			bw.Write(CustomColorID);
		}
	}
}
