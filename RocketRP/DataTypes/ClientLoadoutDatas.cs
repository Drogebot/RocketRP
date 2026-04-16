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

		public static ClientLoadoutDatas Deserialize(BitReader br)
		{
			var loadouts = new ClientLoadoutData[2]
			{
				ClientLoadoutData.Deserialize(br),
				ClientLoadoutData.Deserialize(br),
			};

			return new ClientLoadoutDatas(loadouts);
		}

		public readonly void Serialize(BitWriter bw)
		{
			Loadouts[0].Serialize(bw);
			Loadouts[1].Serialize(bw);
		}
	}
}
