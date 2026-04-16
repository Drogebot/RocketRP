namespace RocketRP.DataTypes
{
	public struct ClientLoadoutDatas
	{
		[FixedArraySize(2)]
		public ClientLoadoutData[] Loadouts { get; set; }

		public ClientLoadoutDatas(ClientLoadoutData[] clientLoadouts)
		{
			Loadouts = clientLoadouts;
		}

		public static ClientLoadoutDatas Deserialize(BitReader br, Replay replay)
		{
			var loadouts = new ClientLoadoutData[2]
			{
				ClientLoadoutData.Deserialize(br, replay),
				ClientLoadoutData.Deserialize(br, replay),
			};

			return new ClientLoadoutDatas(loadouts);
		}

		public readonly void Serialize(BitWriter bw, Replay replay)
		{
			Loadouts[0].Serialize(bw, replay);
			Loadouts[1].Serialize(bw, replay);
		}
	}
}
