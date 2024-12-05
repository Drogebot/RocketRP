using RocketRP.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketRP.Actors.TAGame
{
	public class Car_TA : Vehicle_TA
	{
		public Name PostMatchAnim { get; set; }
		public float ReplicatedCarMaxLinearSpeedScale { get; set; }
		public float AddedCarForceMultiplier { get; set; }
		public float AddedBallForceMultiplier { get; set; }
		public ObjectTarget RumblePickups { get; set; }
		public ObjectTarget AttachedPickup { get; set; }
		public DemolishDataGoalExplosion ReplicatedDemolishGoalExplosion { get; set; }
		public DemolishData ReplicatedDemolish { get; set; }
		public DemolishDataExtended ReplicatedDemolishExtended { get; set; }
		public float ReplicatedCarScale { get; set; }
		public ClubColorSet ClubColors { get; set; }
		public LoadoutTeamPaint TeamPaint { get; set; }
		public bool bOverrideBoostOn { get; set; }
		public bool bOverrideHandbrakeOn { get; set; }


		// These are old properties that were removed
		public DemolishData2 ReplicatedDemolish_CustomFX { get; set; }
	}
}
