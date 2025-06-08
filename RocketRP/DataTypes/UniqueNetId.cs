using Newtonsoft.Json;
using RocketRP.DataTypes.Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketRP.DataTypes
{
    public struct UniqueNetId
	{
		public ulong? Uid { get; set; }
		public SceNpId? NpId { get; set; }
		public string? EpicAccountId { get; set; }
		public OnlinePlatform? Platform { get; set; }
		public byte? SplitscreenID { get; set; }
		public string? Id { get; set; }

		public UniqueNetId(OnlinePlatform type, string Id, byte playerNumber)
		{
			this.Uid = 0;
			this.NpId = null;
			this.EpicAccountId = null;
			this.Platform = type;
			this.Id = Id;
			this.SplitscreenID = playerNumber;
		}

		public static UniqueNetId Deserialize(BinaryReader br)
		{
			return new UniqueNetId();
		}

		public static UniqueNetId Deserialize(BitReader br, Replay replay)
		{
			var type = (OnlinePlatform)br.ReadByte();
			string id;
			switch (type)
			{
				case OnlinePlatform.OnlinePlatform_Unknown:
					return new UniqueNetId(OnlinePlatform.OnlinePlatform_Unknown, "", 0);
				case OnlinePlatform.OnlinePlatform_Steam:
				case OnlinePlatform.OnlinePlatform_Dingo:
				case OnlinePlatform.OnlinePlatform_QQ:
					id = br.ReadUInt64().ToString();
					break;
				case OnlinePlatform.OnlinePlatform_PS4:
					if(replay.NetVersion >= 1) id = Convert.ToHexString(br.ReadBytes(40));
					else id = Convert.ToHexString(br.ReadBytes(32));
					break;
				case OnlinePlatform.OnlinePlatform_OldNNX:
					id = Convert.ToHexString(br.ReadBytes(32));
					break;
				case OnlinePlatform.OnlinePlatform_NNX:
					if(replay.NetVersion >= 10) id = br.ReadUInt64().ToString();
					else id = Convert.ToHexString(br.ReadBytes(32));
					break;
				case OnlinePlatform.OnlinePlatform_Epic:
					id = br.ReadString()!;
					break;
				default:
					throw new Exception($"Unknown UniqueNetId Type {type}");
			}
			var playerNumber = br.ReadByte();

			return new UniqueNetId(type, id, playerNumber);
		}

		public void Serialize(BitWriter bw, Replay replay)
		{
			bw.Write((byte?)Platform);
			switch ((OnlinePlatform)Platform)
			{
				case OnlinePlatform.OnlinePlatform_Unknown:
					return;
				case OnlinePlatform.OnlinePlatform_Steam:
				case OnlinePlatform.OnlinePlatform_Dingo:
				case OnlinePlatform.OnlinePlatform_QQ:
					bw.Write(UInt64.Parse(Id!));
					break;
				case OnlinePlatform.OnlinePlatform_PS4:
				case OnlinePlatform.OnlinePlatform_OldNNX:
					bw.Write(Convert.FromHexString(Id!));
					break;
				case OnlinePlatform.OnlinePlatform_NNX:
					if (replay.NetVersion >= 10) bw.Write(UInt64.Parse(Id!));
					else bw.Write(Convert.FromHexString(Id!));
					break;
				case OnlinePlatform.OnlinePlatform_Epic:
					bw.Write(Id!);
					break;
				default:
					throw new Exception($"Unknown UniqueNetId Type {Platform}");
			}
			bw.Write(SplitscreenID);
		}
	}
}
