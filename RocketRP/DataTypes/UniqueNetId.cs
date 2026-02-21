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

		public UniqueNetId(OnlinePlatform platform, ulong uid, SceNpId? npId, string? epicAccountId, byte splitscreenId)
		{
			Uid = uid;
			NpId = npId;
			EpicAccountId = epicAccountId;
			Platform = platform;
			SplitscreenID = splitscreenId;
		}

		public static UniqueNetId Deserialize(BinaryReader br)
		{
			return new UniqueNetId();
		}

		public static UniqueNetId Deserialize(BitReader br, Replay replay)
		{
			var platform = (OnlinePlatform)br.ReadByte();
			ulong uid = 0;
			SceNpId? npId = null;
			string? epicAccountId = null;
			byte splitscreenId = 0;

			switch (platform)
			{
				case OnlinePlatform.OnlinePlatform_Unknown:
					break;
				case OnlinePlatform.OnlinePlatform_Steam:
				case OnlinePlatform.OnlinePlatform_Dingo:
				case OnlinePlatform.OnlinePlatform_QQ:
					uid = br.ReadUInt64();
					break;
				case OnlinePlatform.OnlinePlatform_PS4:
					npId = SceNpId.Deserialize(br);
					if (replay.NetVersion >= 1) uid = br.ReadUInt64();
					break;
				case OnlinePlatform.OnlinePlatform_OldNNX:
					npId = SceNpId.Deserialize(br);
					break;
				case OnlinePlatform.OnlinePlatform_NNX:
					if (replay.NetVersion >= 10) uid = br.ReadUInt64();
					// I don't know if this ever happens, but better safe than sorry.
					// It's the same as the old NNX format, so I think new NNX was made at the same time they changed the format.
					else npId = SceNpId.Deserialize(br);
					break;
				case OnlinePlatform.OnlinePlatform_Epic:
					epicAccountId = br.ReadString();
					break;
				default:
					throw new Exception($"Unknown UniqueNetId Type {platform}");
			}
			
			if(platform != OnlinePlatform.OnlinePlatform_Unknown) splitscreenId = br.ReadByte();

			return new UniqueNetId(platform, uid, npId, epicAccountId, splitscreenId);
		}

		public void Serialize(BitWriter bw, Replay replay)
		{
			bw.Write((byte?)Platform);
			switch ((OnlinePlatform)Platform)
			{
				case OnlinePlatform.OnlinePlatform_Unknown:
					break;
				case OnlinePlatform.OnlinePlatform_Steam:
				case OnlinePlatform.OnlinePlatform_Dingo:
				case OnlinePlatform.OnlinePlatform_QQ:
					bw.Write(Uid);
					break;
				case OnlinePlatform.OnlinePlatform_PS4:
					NpId!.Value.Serialize(bw);
					if (replay.NetVersion >= 1) bw.Write(Uid);
					break;
				case OnlinePlatform.OnlinePlatform_OldNNX:
					NpId!.Value.Serialize(bw);
					break;
				case OnlinePlatform.OnlinePlatform_NNX:
					if (replay.NetVersion >= 10) bw.Write(Uid);
					else NpId!.Value.Serialize(bw);
					break;
				case OnlinePlatform.OnlinePlatform_Epic:
					bw.Write(EpicAccountId);
					break;
				default:
					throw new Exception($"Unknown UniqueNetId Type {Platform}");
			}

			if(Platform != OnlinePlatform.OnlinePlatform_Unknown) bw.Write(SplitscreenID);
		}
	}
}
