using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketRP.DataTypes
{
	public enum PlatformId : byte
	{
		Unknown,
		Steam,
		PS4,
		PS3,
		XboxOne,
		QQ,
		OldSwitch,
		Switch,
		Psynet,
		Deleted,
		WeGame,
		Epic,
	}

	public struct UniqueNetId
	{
		public PlatformId Type { get; set; }
		public string Id { get; set; }
		public byte PlayerNumber { get; set; }

		public UniqueNetId(PlatformId type, string Id, byte playerNumber)
		{
			this.Type = type;
			this.Id = Id;
			this.PlayerNumber = playerNumber;
		}
		public static UniqueNetId Deserialize(BitReader br, Replay replay)
		{
			return Deserialize2(br, replay, false);
		}

		public static UniqueNetId Deserialize2(BitReader br, Replay replay, bool isPartyLeaderProperty = false)
		{
			var type = (PlatformId)br.ReadByte();
			string id;
			switch (type)
			{
				case PlatformId.Unknown:
					if (replay.LicenseeVersion >= 18 && replay.NetVersion == 0 || isPartyLeaderProperty) return new UniqueNetId(PlatformId.Unknown, "", 0);
					else id = Convert.ToHexString(br.ReadBytes(3));
					break;
				case PlatformId.Steam:
				case PlatformId.XboxOne:
					id = br.ReadUInt64().ToString();
					break;
				case PlatformId.PS4:
					if(replay.NetVersion >= 1) id = Convert.ToHexString(br.ReadBytes(40));
					else id = Convert.ToHexString(br.ReadBytes(32));
					break;
				case PlatformId.OldSwitch:
					id = Convert.ToHexString(br.ReadBytes(32));
					break;
				case PlatformId.Switch:
					if(replay.NetVersion >= 10) id = br.ReadUInt64().ToString();
					else id = Convert.ToHexString(br.ReadBytes(32));
					break;
				case PlatformId.Epic:
					id = br.ReadString();
					break;
				default:
					throw new Exception($"Unknown UniqueNetId Type {type}");
			}
			var playerNumber = br.ReadByte();

			return new UniqueNetId(type, id, playerNumber);
		}

		public void Serialize(BitWriter bw, Replay replay)
		{
			Serialize2(bw, replay, false);
		}

		public void Serialize2(BitWriter bw, Replay replay, bool isPartyLeaderProperty = false)
		{
			bw.Write((byte)Type);
			switch (Type)
			{
				case PlatformId.Unknown:
					if (replay.LicenseeVersion >= 18 && replay.NetVersion == 0 || isPartyLeaderProperty) return;
					else bw.Write(Convert.FromHexString(Id));
					break;
				case PlatformId.Steam:
				case PlatformId.XboxOne:
					bw.Write(UInt64.Parse(Id));
					break;
				case PlatformId.PS4:
				case PlatformId.OldSwitch:
					bw.Write(Convert.FromHexString(Id));
					break;
				case PlatformId.Switch:
					if (replay.NetVersion >= 10) bw.Write(UInt64.Parse(Id));
					else bw.Write(Convert.FromHexString(Id));
					break;
				case PlatformId.Epic:
					bw.Write(Id);
					break;
				default:
					throw new Exception($"Unknown UniqueNetId Type {Type}");
			}
			bw.Write(PlayerNumber);
		}
	}
}
