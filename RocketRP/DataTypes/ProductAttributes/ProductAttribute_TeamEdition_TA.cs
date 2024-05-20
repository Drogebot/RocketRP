using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketRP.DataTypes.ProductAttributes
{
	public class ProductAttribute_TeamEdition_TA : ProductAttribute_TA
	{
		public TeamEdition EditionId { get; set; }

		public ProductAttribute_TeamEdition_TA(TeamEdition editionId)
		{
			EditionId = editionId;
		}

		public static new ProductAttribute_TeamEdition_TA Deserialize(BitReader br, Replay replay)
		{
			var editionId = (TeamEdition)br.ReadUInt32FromBits(31);

			return new ProductAttribute_TeamEdition_TA(editionId);
		}

		public override void Serialize(BitWriter bw, Replay replay)
		{
			base.Serialize(bw, replay);

			bw.WriteFixedBits((uint)EditionId, 31);
		}
	}

	public enum TeamEdition : uint
	{
		None,

		Cloud9 = 2,
		Dignitas,
		EvilGeniuses,

		G2Esports = 7,
		GhostGaming,
		Mousesports,
		NRG,		// The old version of NRG
		PSGEsports,

		TeamVitality = 13,

		Splyce = 15,

		Rogue = 17,
		TSM,
		Complexity,
		SpacestationGaming,
		FCBarcelona,
		PittsburghKnights,
		Reciprocity,
		Veloce,
		eUnited,
		NRGEsports,
		AlpineEsports,
		TeamEndpoint,
		Guild,
		OxygenEsports,
		Solary,
		SusquehannaSoniqs,
		TeamBDS,
		TeamEnvy,
		TeamLiquid,
		Giants,
		KarmineCorp,
		RixGG,
		SMPREsports,
		SKGaming,
		TeamQueso,
		TeamSingularity,
		FaZeClan,
		PWR,
		Rebellion,
		Torrent,
		TrueNeutral,
		Version1,
		XSET,
		Furia,
		GroundZeroGaming,
		Renegades,
		MisfitsGaming,
		OpTicGaming,
		Luminosity,
		MoistEsports,
		TeamFalcons,
		KCPioneers,
		TeamSecret,
		GenG,
		Quadrant,
		Tundra,
		EndGame,
		Unknown1,   //???
		Resolve,
		Elevate2024,
		Unknown2,   //???
		GentleMates2024,
		KRU2024,
		M802024,
		NinjasinPyjamas,
		OG2024,
		RuleOne2024,
		Virtuspro2024,
		Limitless2024,
		MAX,
	}
}
