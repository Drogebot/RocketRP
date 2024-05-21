using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketRP.DataTypes.TAGame
{
	public class ProductAttribute_SpecialEdition_TA : ProductAttribute_TA
	{
		public SpecialEdition EditionId { get; set; }

		public ProductAttribute_SpecialEdition_TA(SpecialEdition editionId)
		{
			EditionId = editionId;
		}

		public static ProductAttribute_TA Deserialize(BitReader br, Replay replay)
		{
			var editionId = (SpecialEdition)br.ReadUInt32FromBits(31);

			return new ProductAttribute_SpecialEdition_TA(editionId);
		}

		public override void Serialize(BitWriter bw, Replay replay)
		{
			base.Serialize(bw, replay);

			bw.WriteFixedBits((uint)EditionId, 31);
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
		MAX,
	}
}
