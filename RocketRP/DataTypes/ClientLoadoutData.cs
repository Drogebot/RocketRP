using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketRP.DataTypes
{
	/// This is originally defined as an array of int, but other parsers do it like this
	public struct ClientLoadoutData
	{
		public byte? Version { get; set; }
		public int[]? Products { get; set; }
		#region ProductSlots
		public int? BodyProductId
		{
			get => Products?.Length > 0 ? Products[0] : null;
			set => Products![0] = (int)value!;
		}
		public int? SkinProductId
		{
			get => Products?.Length > 1 ? Products[1] : null;
			set => Products![1] = (int)value!;
		}
		public int? WheelProductId
		{
			get => Products?.Length > 2 ? Products[2] : null;
			set => Products![2] = (int)value!;
		}
		public int? BoostProductId
		{
			get => Products?.Length > 3 ? Products[3] : null;
			set => Products![3] = (int)value!;
		}
		public int? AntennaProductId
		{
			get => Products?.Length > 4 ? Products[4] : null;
			set => Products![4] = (int)value!;
		}
		public int? HatProductId
		{
			get => Products?.Length > 5 ? Products[5] : null;
			set => Products![5] = (int)value!;
		}
		public int? PaintFinish
		{
			get => Products?.Length > 6 ? Products[6] : null;
			set => Products![6] = (int)value!;
		}
		public int? CustomFinish
		{
			get => Products?.Length > 7 ? Products[7] : null;
			set => Products![7] = (int)value!;
		}
		public int? EngineAudioProductId
		{
			get => Products?.Length > 8 ? Products[8] : null;
			set => Products![8] = (int)value!;
		}
		public int? SupersonicTrailProductId
		{
			get => Products?.Length > 9 ? Products[9] : null;
			set => Products![9] = (int)value!;
		}
		public int? GoalExplosionProductId
		{
			get => Products?.Length > 10 ? Products[10] : null;
			set => Products![10] = (int)value!;
		}
		public int? PlayerBannerProductId
		{
			get => Products?.Length > 11 ? Products[11] : null;
			set => Products![11] = (int)value!;
		}
		public int? MusicStinger
		{
			get => Products?.Length > 12 ? Products[12] : null;
			set => Products![12] = (int)value!;
		}
		public int? Unknown1
		{
			get => Products?.Length > 13 ? Products[13] : null;
			set => Products![13] = (int)value!;
		}
		public int? PlayerAvatarBorder
		{
			get => Products?.Length > 14 ? Products[14] : null;
			set => Products![14] = (int)value!;
		}
		public int? PlayerTitle
		{
			get => Products?.Length > 15 ? Products[15] : null;
			set => Products![15] = (int)value!;
		}
		#endregion

		public ClientLoadoutData(byte? version, int[]? products)
		{
			Version = version;
			Products = products;
			if (Products is null) return;
			var itemCountVersion = ItemCountToVersion(Products.Length);
			Version = version ?? itemCountVersion;
			var versionItemCount = VersionToItemCount(Version.Value);
			if (Products.Length != versionItemCount) throw new IndexOutOfRangeException($"Given product count({Products.Length}) does not match the given version({versionItemCount})");
			Products = products;
		}

		public ClientLoadoutData(byte? version, params int?[] products)
			: this(version, products.TakeWhile(id => id is not null).Select(id => (int)id!).ToArray()) { }

		public ClientLoadoutData(byte? version, int? bodyProductId, int? skinProductId, int? wheelProductId, int? boostProductId, int? antennaProductId, int? hatProductId, int? paintFinish, int? customFinish, int? engineAudioProductId, int? supersonicTrailProductId, int? goalExplosionProductId, int? playerBannerProductId, int? musicStinger, int? unknown1, int? playerAvatarBorder, int? playerTitle)
			: this(version, [bodyProductId, skinProductId, wheelProductId, boostProductId, antennaProductId, hatProductId, paintFinish, customFinish, engineAudioProductId, supersonicTrailProductId, goalExplosionProductId, playerBannerProductId, musicStinger, unknown1, playerAvatarBorder, playerTitle]) { }

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

			return new ClientLoadoutData(version, products);
		}

		public void Serialize(BitWriter bw)
		{
			bw.Write(Version);
			for (int i = 0; i < VersionToItemCount(Version.Value); i++)
			{
				bw.Write(Products![i]);
			}
		}
	}
}
