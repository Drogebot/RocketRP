namespace RocketRP.DataTypes
{
	public struct ClientLoadoutOnlineDatas
	{
		[FixedArraySize(2)]
		public ClientLoadoutOnlineData[] Loadouts { get; set; }
		public bool bLoadoutSet { get; set; }
		public bool bDepricated { get; set; }

		public ClientLoadoutOnlineDatas(ClientLoadoutOnlineData[] loadouts, bool bLoadoutSet, bool bDepricated)
		{
			Loadouts = loadouts;
			this.bLoadoutSet = bLoadoutSet;
			this.bDepricated = bDepricated;
		}

		public static ClientLoadoutOnlineDatas Deserialize(BitReader br, Replay replay)
		{
			var loadouts = new ClientLoadoutOnlineData[2]
			{
				ClientLoadoutOnlineData.Deserialize(br, replay),
				ClientLoadoutOnlineData.Deserialize(br, replay),
			};
			var bLoadoutSet = br.ReadBit();
			var bDepricated = br.ReadBit();

			return new ClientLoadoutOnlineDatas(loadouts, bLoadoutSet, bDepricated);
		}

		public readonly void Serialize(BitWriter bw, Replay replay)
		{
			Loadouts[0].Serialize(bw, replay);
			Loadouts[1].Serialize(bw, replay);
			bw.Write(bLoadoutSet);
			bw.Write(bDepricated);
		}
	}
}
