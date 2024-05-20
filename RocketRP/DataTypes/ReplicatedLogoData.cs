using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketRP.DataTypes
{
	public struct ReplicatedLogoData
	{
		public int LogoID { get; set; }
		public bool bSwapColors { get; set; }

		public ReplicatedLogoData(int LogoID, bool bSwapColors)
		{
			this.LogoID = LogoID;
			this.bSwapColors = bSwapColors;
		}

		public static ReplicatedLogoData Deserialize(BitReader br)
		{
			var LogoID = br.ReadInt32();
			var bSwapColors = br.ReadBit();

			return new ReplicatedLogoData(LogoID, bSwapColors);
		}

		public void Serialize(BitWriter bw)
		{
			bw.Write(LogoID);
			bw.Write(bSwapColors);
		}
	}
}
