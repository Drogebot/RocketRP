using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketRP.DataTypes
{
	/// This is originally defined as an array of int, but other parsers do it like this
	public struct ClientLoadoutData
	{
		public byte Version { get; set; }
		public int BodyProductId { get; set; }
		public int SkinProductId { get; set; }
		public int WheelProductId { get; set; }
		public int BoostProductId { get; set; }
		public int AntennaProductId { get; set; }
		public int HatProductId { get; set; }
		public int PaintFinish { get; set; }
		public int CustomFinish { get; set; }
		public int EngineAudioProductId { get; set; }
		public int SupersonicTrailProductId { get; set; }
		public int GoalExplosionProductId { get; set; }
		public int PlayerBannerProductId { get; set; }
		public int MusicStinger { get; set; }
		public int Unknown1 { get; set; }
		public int PlayerAvatarBorder { get; set; }
		public int PlayerTitle { get; set; }

		public ClientLoadoutData(byte version, int bodyProductId, int skinProductId, int wheelProductId, int boostProductId, int antennaProductId, int hatProductId, int paintFinish, int customFinish, int engineAudioProductId, int supersonicTrailProductId, int goalExplosionProductId, int playerBannerProductId, int musicStinger, int unknown1, int playerAvatarBorder, int playerTitle)
		{
			this.Version = version;
			this.BodyProductId = bodyProductId;
			this.SkinProductId = skinProductId;
			this.WheelProductId = wheelProductId;
			this.BoostProductId = boostProductId;
			this.AntennaProductId = antennaProductId;
			this.HatProductId = hatProductId;
			this.PaintFinish = paintFinish;
			this.CustomFinish = customFinish;
			this.EngineAudioProductId = engineAudioProductId;
			this.SupersonicTrailProductId = supersonicTrailProductId;
			this.GoalExplosionProductId = goalExplosionProductId;
			this.PlayerBannerProductId = playerBannerProductId;
			this.MusicStinger = musicStinger;
			this.Unknown1 = unknown1;
			this.PlayerAvatarBorder = playerAvatarBorder;
			this.PlayerTitle = playerTitle;
		}

		public static ClientLoadoutData Deserialize(BitReader br)
		{
			byte version = br.ReadByte();
			int bodyProductId = br.ReadInt32();
			int skinProductId = br.ReadInt32();
			int wheelProductId = br.ReadInt32();
			int boostProductId = br.ReadInt32();
			int antennaProductId = br.ReadInt32();
			int hatProductId = br.ReadInt32();
			int paintFinish = br.ReadInt32();

			int customFinish = 0;
			if (version >= 11)
			{
				customFinish = br.ReadInt32();
			}

			int engineAudioProductId = 0, supersonicTrailProductId = 0, goalExplosionProductId = 0;
			if (version >= 16)
			{
				engineAudioProductId = br.ReadInt32();
				supersonicTrailProductId = br.ReadInt32();
				goalExplosionProductId = br.ReadInt32();
			}

			int playerBannerProductId = 0;
			if(version >= 17)
			{
				playerBannerProductId = br.ReadInt32();
			}

			int musicStinger = 0;
			if (version >= 19)
			{
				musicStinger = br.ReadInt32();
			}

			int unknown1 = 0, playerAvatarBorder = 0, playerTitle = 0;
			if (version >= 22)
			{
				unknown1 = br.ReadInt32();
				playerAvatarBorder = br.ReadInt32();
				playerTitle = br.ReadInt32();
			}

			return new ClientLoadoutData(version, bodyProductId, skinProductId, wheelProductId, boostProductId, antennaProductId, hatProductId, paintFinish, customFinish, engineAudioProductId, supersonicTrailProductId, goalExplosionProductId, playerBannerProductId, musicStinger, unknown1, playerAvatarBorder, playerTitle);
		}

		public void Serialize(BitWriter bw)
		{
			bw.Write(Version);
			bw.Write(BodyProductId);
			bw.Write(SkinProductId);
			bw.Write(WheelProductId);
			bw.Write(BoostProductId);
			bw.Write(AntennaProductId);
			bw.Write(HatProductId);
			bw.Write(PaintFinish);

			if (Version < 11) return;
			bw.Write(CustomFinish);

			if (Version < 16) return;
			bw.Write(EngineAudioProductId);
			bw.Write(SupersonicTrailProductId);
			bw.Write(GoalExplosionProductId);

			if (Version < 17) return;
			bw.Write(PlayerBannerProductId);

			if (Version < 19) return;
			bw.Write(MusicStinger);

			if (Version < 22) return;
			bw.Write(Unknown1);
			bw.Write(PlayerAvatarBorder);
			bw.Write(PlayerTitle);
		}
	}
}
