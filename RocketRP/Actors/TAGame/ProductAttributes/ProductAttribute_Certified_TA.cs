using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketRP.Actors.TAGame
{
	public class ProductAttribute_Certified_TA : ProductAttribute_TA
	{
		public CertificationType? StatId { get; set; }
		public int? StatValue { get; set; }

		public ProductAttribute_Certified_TA() { }

		public ProductAttribute_Certified_TA(CertificationType statId)
		{
			StatId = statId;
		}

		public static ProductAttribute_Certified_TA DeserializeType(BitReader br, Replay replay)
		{
			throw new NotImplementedException();
		}

		public override void Serialize(BitWriter bw, Replay replay)
		{
			base.Serialize(bw, replay);

			throw new NotImplementedException();
		}
	}

	public enum CertificationType : uint
	{
		None,
		Aviator,      //AerialGoals
		Playmaker,    //Assists
		ShowOff,      //BackwardsGoals
		Acrobat,      //BicycleGoals
		Tactician,    //Centers
		Sweeper,      //Clears
		Guardian,     //EpicSaves
		Scorer,       //Goals
		Juggler,      //Juggles
		Sniper,       //LongGoals
		Paragon,      //MVPs
		Goalkeeper,   //Saves
		Striker,      //ShotsOnGoal
		Turtle,       //TurtleGoals
		Victor,       //Wins
		Silencer,     //InfectedPlayersDefeated
		Reverberator, //PlayersInfected
		Underdog,     //Losses
		Exploder,     //Demolished
		MAX,
	}
}
