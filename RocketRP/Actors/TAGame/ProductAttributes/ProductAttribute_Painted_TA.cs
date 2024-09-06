﻿using System;
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
			if (replay.EngineVersion >= 868 && replay.LicenseeVersion >= 18) paintID = (PaintColor)br.ReadUInt32FromBits(31);
			else paintID = (PaintColor)br.ReadUInt32Max((uint)PaintColor.Gold);

			return new ProductAttribute_Painted_TA(paintID);
		}

		public override void Serialize(BitWriter bw, Replay replay)
		{
			base.Serialize(bw, replay);

			if (replay.EngineVersion >= 868 && replay.LicenseeVersion >= 18) bw.WriteFixedBits((uint)PaintID, 31);
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
		MAX,
	}
}