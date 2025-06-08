using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketRP.Actors.TAGame
{
	public class ProductAttribute_TeamEdition_TA : ProductAttribute_TA
	{
		public TeamEdition? Id { get; set; }

		public ProductAttribute_TeamEdition_TA() { }

		public ProductAttribute_TeamEdition_TA(TeamEdition id)
		{
			Id = id;
		}

		public static ProductAttribute_TeamEdition_TA DeserializeType(BitReader br, Replay replay)
		{
			var id = (TeamEdition)br.ReadUInt32(1U << 31);

			return new ProductAttribute_TeamEdition_TA(id);
		}

		public override void Serialize(BitWriter bw, Replay replay)
		{
			base.Serialize(bw, replay);

			bw.Write((uint?)Id, 1U << 31);
		}
	}

	public enum TeamEdition : uint
	{
		None,

		Cloud9 = 2,
		Dignitas,
		EvilGeniuses,

		G2 = 7,
		GhostGaming,
		Mousesports,
		NRG,		// The old version of NRG
		PSG,

		RenaultVitality = 13,

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
		NRG_Season8,
		Alpine,
		Endpoint,
		Guild,
		OxyGen,
		Solary,
		SusquehannaSoniqs,
		TeamBDS,
		TeamEnvy,
		TeamLiquid,
		Giants,
		KarmineCorp,
		RixGG,
		SemperEsports,
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
		Misfits,
		OpticGaming,
		Luminosity,
		MoistEsports,
		TeamFalcons,
		KansasCityPioneers,
		TeamSecret,
		GenG,
		Quadrant,
		Tundra,
		EndGame,
		Fnatic,
		Resolve,
		Elevate,
		G1,
		GentleMates,
		KRU,
		M80,
		NIP,
		OG,
		RuleOne,
		VirtusPro,
		Limitless,
		MAX,
	}
}
