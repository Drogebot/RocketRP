using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketRP.DataTypes.TAGame
{
	public class ProductAttribute_Painted_TA : ProductAttribute_TA
	{
		public PaintColor PaintId { get; set; }

		public ProductAttribute_Painted_TA(PaintColor paintId)
		{
			PaintId = paintId;
		}

		public static new ProductAttribute_Painted_TA Deserialize(BitReader br, Replay replay)
		{
			PaintColor paintId;
			if (replay.EngineVersion >= 868 && replay.LicenseeVersion >= 18) paintId = (PaintColor)br.ReadUInt32FromBits(31);
			else paintId = (PaintColor)br.ReadUInt32Max((uint)PaintColor.Gold);

			return new ProductAttribute_Painted_TA(paintId);
		}

		public override void Serialize(BitWriter bw, Replay replay)
		{
			base.Serialize(bw, replay);

			if (replay.EngineVersion >= 868 && replay.LicenseeVersion >= 18) bw.WriteFixedBits((uint)PaintId, 31);
			else bw.Write((uint)PaintId, (uint)PaintColor.Gold);
		}
	}

	public enum PaintColor : uint
	{
		None,
		Crimson,
		Lime,
		Black,
		SkyBlue,
		Cobalt,
		BurntSienna,
		ForestGreen,
		Purple,
		Pink,
		Orange,
		Grey,
		TitaniumWhite,
		Saffron,
		Gold,
		RoseGold,
		WhiteGold,
		Onyx,
		Platinum,
		MAX,
	}
}
