﻿using System;
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
			var id = (TeamEdition)br.ReadUInt32FromBits(31);

			return new ProductAttribute_TeamEdition_TA(id);
		}

		public override void Serialize(BitWriter bw, Replay replay)
		{
			base.Serialize(bw, replay);

			bw.WriteFixedBits((uint)Id, 31);
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
		Fnatic,
		Resolve,
		Elevate2024,
		G1,
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
