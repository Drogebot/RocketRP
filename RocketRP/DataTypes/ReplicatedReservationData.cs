namespace RocketRP.DataTypes
{
	public struct ReplicatedReservationData
	{
		public UniqueNetId PlayerID { get; set; }
		public string? PlayerName { get; set; }
		public EReservationStatus Status { get; set; }

		public ReplicatedReservationData(UniqueNetId playerId, string? playerName, EReservationStatus status)
		{
			PlayerID = playerId;
			PlayerName = playerName;
			Status = status;
		}

		public static ReplicatedReservationData Deserialize(BitReader br, Replay replay)
		{
			var playerId = UniqueNetId.Deserialize(br, replay);
			var playerName = br.ReadString();

			EReservationStatus status;
			if (replay.LicenseeVersion >= 12) status = (EReservationStatus)br.ReadByte();
			else status = (EReservationStatus)br.ReadUInt32((uint)EReservationStatus.END);

			return new ReplicatedReservationData(playerId, playerName, status);
		}

		public readonly void Serialize(BitWriter bw, Replay replay)
		{
			PlayerID.Serialize(bw, replay);
			bw.Write(PlayerName);

			if (replay.LicenseeVersion >= 12) bw.Write((byte)Status);
			else bw.Write((uint)Status, (uint)EReservationStatus.END);
		}
	}

	public enum EReservationStatus : byte
	{
		None,
		Reserved,
		Joining,
		InGame,
		END,
	}
}
