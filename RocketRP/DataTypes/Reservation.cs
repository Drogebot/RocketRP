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

			string playerName = "";
			if (playerId.Type != PlatformId.Unknown) playerName = br.ReadString();
			else
			{
				// I've made this to match UniqueNetId.Deserialize()
				// But it could instead be (replay.LicenseeVersion >= 18 && replay.LicenseeVersion <= 19) or just (replay.LicenseeVersion == 18)
				if (replay.LicenseeVersion >= 18 && replay.NetVersion == 0)
				{
					playerName = br.ReadString();
				}
			}

			ReservationStatus status;
			if (replay.EngineVersion >= 868 && replay.LicenseeVersion >= 12) status = (ReservationStatus)br.ReadByte();
			else status = (ReservationStatus)br.ReadUInt32Max((uint)ReservationStatus.END);

			return new Reservation(playerId, playerName, status);
		}

		public void Serialize(BitWriter bw, Replay replay)
		{
			PlayerId.Serialize(bw, replay);

			if (PlayerId.Type != PlatformId.Unknown) bw.Write(PlayerName);
			else
			{
				// I've made this to match UniqueNetId.Deserialize()
				// But it could instead be (replay.LicenseeVersion >= 18 && replay.LicenseeVersion <= 19) or just (replay.LicenseeVersion == 18)
				if (replay.LicenseeVersion >= 18 && replay.NetVersion == 0)
				{
					bw.Write(PlayerName);
				}
			}

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
