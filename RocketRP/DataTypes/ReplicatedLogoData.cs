namespace RocketRP.DataTypes
{
	public struct ReplicatedLogoData
	{
		public int LogoID { get; set; }
		public bool bSwapColors { get; set; }

		public ReplicatedLogoData(int logoID, bool bSwapColors)
		{
			LogoID = logoID;
			this.bSwapColors = bSwapColors;
		}

		public static ReplicatedLogoData Deserialize(BitReader br, Replay replay)
		{
			var logoID = br.ReadInt32();
			var bSwapColors = br.ReadBit();

			return new ReplicatedLogoData(logoID, bSwapColors);
		}

		public readonly void Serialize(BitWriter bw, Replay replay)
		{
			bw.Write(LogoID);
			bw.Write(bSwapColors);
		}
	}
}
