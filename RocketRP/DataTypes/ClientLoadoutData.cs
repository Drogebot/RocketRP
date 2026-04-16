using System;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace RocketRP.DataTypes
{
	/// This is originally defined as an array of int, but other parsers do it like this
	public struct ClientLoadoutData
	{
		public byte Version { get; set; }
		public int[] Products { get; set; }
		#region ProductSlots
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenReading)]
		public readonly int BodyProductId
		{
			get => Products.Length > 0 ? Products[0] : 0;
			set => Products[0] = value;
		}
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenReading)]
		public readonly int SkinProductId
		{
			get => Products.Length > 1 ? Products[1] : 0;
			set => Products[1] = value;
		}
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenReading)]
		public readonly int WheelProductId
		{
			get => Products.Length > 2 ? Products[2] : 0;
			set => Products[2] = value;
		}
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenReading)]
		public readonly int BoostProductId
		{
			get => Products.Length > 3 ? Products[3] : 0;
			set => Products[3] = value;
		}
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenReading)]
		public readonly int AntennaProductId
		{
			get => Products.Length > 4 ? Products[4] : 0;
			set => Products[4] = value;
		}
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenReading)]
		public readonly int HatProductId
		{
			get => Products.Length > 5 ? Products[5] : 0;
			set => Products[5] = value;
		}
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenReading)]
		public readonly int PaintFinish
		{
			get => Products.Length > 6 ? Products[6] : 0;
			set => Products[6] = value;
		}
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenReading)]
		public readonly int CustomFinish
		{
			get => Products.Length > 7 ? Products[7] : 0;
			set => Products[7] = value;
		}
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenReading)]
		public readonly int EngineAudioProductId
		{
			get => Products.Length > 8 ? Products[8] : 0;
			set => Products[8] = value;
		}
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenReading)]
		public readonly int SupersonicTrailProductId
		{
			get => Products.Length > 9 ? Products[9] : 0;
			set => Products[9] = value;
		}
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenReading)]
		public readonly int GoalExplosionProductId
		{
			get => Products.Length > 10 ? Products[10] : 0;
			set => Products[10] = value;
		}
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenReading)]
		public readonly int PlayerBannerProductId
		{
			get => Products.Length > 11 ? Products[11] : 0;
			set => Products[11] = value;
		}
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenReading)]
		public readonly int MusicStinger
		{
			get => Products.Length > 12 ? Products[12] : 0;
			set => Products[12] = value;
		}
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenReading)]
		public readonly int Unknown1
		{
			get => Products.Length > 13 ? Products[13] : 0;
			set => Products[13] = value;
		}
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenReading)]
		public readonly int PlayerAvatarBorder
		{
			get => Products.Length > 14 ? Products[14] : 0;
			set => Products[14] = value;
		}
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenReading)]
		public readonly int PlayerTitle
		{
			get => Products.Length > 15 ? Products[15] : 0;
			set => Products[15] = value;
		}
		#endregion

		public ClientLoadoutData([NotNull] int[] products)
		{
			Version = ItemCountToVersion(products.Length);
			Products = products;
		}

		public ClientLoadoutData(int bodyProductId, int skinProductId, int wheelProductId, int boostProductId, int antennaProductId, int hatProductId, int paintFinish)
			: this([bodyProductId, skinProductId, wheelProductId, boostProductId, antennaProductId, hatProductId, paintFinish]) { }

		public ClientLoadoutData(int bodyProductId, int skinProductId, int wheelProductId, int boostProductId, int antennaProductId, int hatProductId, int paintFinish, int customFinish)
			: this([bodyProductId, skinProductId, wheelProductId, boostProductId, antennaProductId, hatProductId, paintFinish, customFinish]) { }

		public ClientLoadoutData(int bodyProductId, int skinProductId, int wheelProductId, int boostProductId, int antennaProductId, int hatProductId, int paintFinish, int customFinish, int engineAudioProductId, int supersonicTrailProductId, int goalExplosionProductId)
			: this([bodyProductId, skinProductId, wheelProductId, boostProductId, antennaProductId, hatProductId, paintFinish, customFinish, engineAudioProductId, supersonicTrailProductId, goalExplosionProductId]) { }

		public ClientLoadoutData(int bodyProductId, int skinProductId, int wheelProductId, int boostProductId, int antennaProductId, int hatProductId, int paintFinish, int customFinish, int engineAudioProductId, int supersonicTrailProductId, int goalExplosionProductId, int playerBannerProductId)
			: this([bodyProductId, skinProductId, wheelProductId, boostProductId, antennaProductId, hatProductId, paintFinish, customFinish, engineAudioProductId, supersonicTrailProductId, goalExplosionProductId, playerBannerProductId]) { }

		public ClientLoadoutData(int bodyProductId, int skinProductId, int wheelProductId, int boostProductId, int antennaProductId, int hatProductId, int paintFinish, int customFinish, int engineAudioProductId, int supersonicTrailProductId, int goalExplosionProductId, int playerBannerProductId, int musicStinger)
			: this([bodyProductId, skinProductId, wheelProductId, boostProductId, antennaProductId, hatProductId, paintFinish, customFinish, engineAudioProductId, supersonicTrailProductId, goalExplosionProductId, playerBannerProductId, musicStinger]) { }

		public ClientLoadoutData(int bodyProductId, int skinProductId, int wheelProductId, int boostProductId, int antennaProductId, int hatProductId, int paintFinish, int customFinish, int engineAudioProductId, int supersonicTrailProductId, int goalExplosionProductId, int playerBannerProductId, int musicStinger, int unknown1, int playerAvatarBorder, int playerTitle)
			: this([bodyProductId, skinProductId, wheelProductId, boostProductId, antennaProductId, hatProductId, paintFinish, customFinish, engineAudioProductId, supersonicTrailProductId, goalExplosionProductId, playerBannerProductId, musicStinger, unknown1, playerAvatarBorder, playerTitle]) { }

		public static int VersionToItemCount(byte version) => version switch
		{
			<= 10 => 7,
			<= 15 => 8,
			<= 16 => 11,
			<= 18 => 12,
			<= 21 => 13,
			_ => 16,
		};

		/// <returns>The highest known valid version for the item count</returns>
		public static byte ItemCountToVersion(int count) => count switch
		{
			7 => 10,
			8 => 15,
			11 => 16,
			12 => 18,
			13 => 21,
			16 => 28,
			_ => throw new ArgumentOutOfRangeException(nameof(count), "Loadout Item Count must be one of [7,8,11,12,13,16]"),
		};

		public static ClientLoadoutData Deserialize(BitReader br)
		{
			byte version = br.ReadByte();
			var products = new int[VersionToItemCount(version)];
			for (int i = 0; i < products.Length; i++)
			{
				products[i] = br.ReadInt32();
			}

			return new ClientLoadoutData(products);
		}

		public readonly void Serialize(BitWriter bw)
		{
			bw.Write(Version);
			for (int i = 0; i < VersionToItemCount(Version); i++)
			{
				bw.Write(Products[i]);
			}
		}
	}
}
