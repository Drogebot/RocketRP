using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketRP.Actors.TAGame
{
	public class ProductAttribute_Painted_TA : ProductAttribute_TA
	{
		public PaintColor? PaintID { get; set; }

		public ProductAttribute_Painted_TA() { }

		public ProductAttribute_Painted_TA(PaintColor paintID)
		{
			PaintID = paintID;
		}

		public static ProductAttribute_Painted_TA DeserializeType(BitReader br, Replay replay)
		{
			PaintColor paintID;
			if (replay.EngineVersion >= 868 && replay.LicenseeVersion >= 18) paintID = (PaintColor)br.ReadUInt32(1U << 31);
			else paintID = (PaintColor)br.ReadUInt32((uint)PaintColor.Gold);

			return new ProductAttribute_Painted_TA(paintID);
		}

		public override void Serialize(BitWriter bw, Replay replay)
		{
			base.Serialize(bw, replay);

			if (replay.EngineVersion >= 868 && replay.LicenseeVersion >= 18) bw.Write((uint)PaintID, 1U << 31);
			else bw.Write((uint)PaintID, (uint)PaintColor.Gold);
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
		SkyBlueGlow,
		CobaltGlow,
		BurtSiennaGlow,
		ForestGreenGlow,
		LimeGlow,
		OrangeGlow,
		PinkGlow,
		PurpleGlow,
		CrimsonGlow,
		TitaniumWhiteGlow,
		SaffronGlow,
		MAX,
	}
}
