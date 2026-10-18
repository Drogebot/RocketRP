using System;

namespace RocketRP.Actors.TAGame
{
	public class ProductAttribute_Certified_TA : ProductAttribute_TA
	{
		public CertificationType StatId { get; set; }
		public int StatValue { get; set; }

		public ProductAttribute_Certified_TA() { }

		public ProductAttribute_Certified_TA(CertificationType statId, int value)
		{
			StatId = statId;
			StatValue = value;
		}

		public static ProductAttribute_Certified_TA DeserializeType(BitReader br, Replay replay)
		{
			CertificationType statId;
			if (replay.LicenseeVersion >= 18) statId = (CertificationType)br.ReadUInt32(1U << 31);
			else statId = (CertificationType)br.ReadUInt32((uint)CertificationType.Silencer);

			var value = br.ReadInt32();

			return new ProductAttribute_Certified_TA(statId, value);
		}

		public override void Serialize(BitWriter bw, Replay replay)
		{
			base.Serialize(bw, replay);

			if (replay.LicenseeVersion >= 18) bw.Write((uint)StatId, 1U << 31);
			else bw.Write((uint)StatId, (uint)CertificationType.Silencer);

			bw.Write(StatValue);
		}
	}

	public enum CertificationType : uint
	{
		None,
		Aviator,		//AerialGoals
		Playmaker,		//Assists
		ShowOff,		//BackwardsGoals
		Acrobat,		//BicycleGoals
		Tactician,		//Centers
		Sweeper,		//Clears
		Guardian,		//EpicSaves
		Scorer,			//Goals
		Juggler,		//Juggles
		Sniper,			//LongGoals
		Paragon,		//MVPs
		Goalkeeper,		//Saves
		Striker,		//ShotsOnGoal
		Turtle,			//TurtleGoals
		Victor,			//Wins
		Silencer,		//InfectedPlayersDefeated
		Reverberator,	//PlayersInfected
		Underdog,		//Losses
		Exploder,       //Demolished
		Title_S24_MVPs, //Title_S24_MVPs
		Title_S24_Wins, //Title_S24_Wins
		Title_S24_ShotsOnTarget,    //Title_S24_ShotsOnTarget
		Title_S24_ShotsAlmostOnTarget,  //Title_S24_ShotsAlmostOnTarget
		Underdog2,		//Losses2
		Exploder2,		//Demolished2
		MAX,
	}
}
