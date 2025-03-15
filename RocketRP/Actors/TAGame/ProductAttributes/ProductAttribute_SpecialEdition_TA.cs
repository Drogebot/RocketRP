using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketRP.Actors.TAGame
{
	public class ProductAttribute_SpecialEdition_TA : ProductAttribute_TA
	{
		public SpecialEdition? EditionID { get; set; }

		public ProductAttribute_SpecialEdition_TA() { }

		public ProductAttribute_SpecialEdition_TA(SpecialEdition editionID)
		{
			EditionID = editionID;
		}

		public static ProductAttribute_TA DeserializeType(BitReader br, Replay replay)
		{
			var editionID = (SpecialEdition)br.ReadUInt32FromBits(31);

			return new ProductAttribute_SpecialEdition_TA(editionID);
		}

		public override void Serialize(BitWriter bw, Replay replay)
		{
			base.Serialize(bw, replay);

			bw.WriteFixedBits((uint)EditionID, 31);
		}
	}

	public enum SpecialEdition : uint
	{
		None,
		Holographic,
		Infinite,
		Inverted,

		Remixed = 5,
		ColorMatch,
		Flare,
		GlowTrim,
		Roasted,
		Scematized,
		MAX,
	}
}
