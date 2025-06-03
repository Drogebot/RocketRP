using RocketRP.DataTypes.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketRP.DataTypes
{
	public struct Reservation
	{
		public UniqueNetId PlayerId { get; set; }
		public string PlayerName { get; set; }
		public ReservationStatus Status { get; set; }

		public Reservation(UniqueNetId playerId, string playerName, ReservationStatus status)
		{
			PlayerId = playerId;
			PlayerName = playerName;
			Status = status;
		}

		public static Reservation Deserialize(BitReader br, Replay replay)
		{
			var playerId = UniqueNetId.Deserialize(br, replay);
			var playerName = br.ReadString();

			ReservationStatus status;
			if (replay.EngineVersion >= 868 && replay.LicenseeVersion >= 12) status = (ReservationStatus)br.ReadByte();
			else status = (ReservationStatus)br.ReadUInt32((uint)ReservationStatus.END);

			return new Reservation(playerId, playerName, status);
		}

		public void Serialize(BitWriter bw, Replay replay)
		{
			PlayerId.Serialize(bw, replay);
			bw.Write(PlayerName);

			if (replay.EngineVersion >= 868 && replay.LicenseeVersion >= 12) bw.Write((byte)Status);
			else bw.Write((uint)Status, (uint)ReservationStatus.END);
		}
	}

	public enum ReservationStatus : byte
	{
		None,
		Reserved,
		Joining,
		InGame,
		END,
	}
}
