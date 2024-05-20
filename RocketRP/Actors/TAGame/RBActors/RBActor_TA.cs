using RocketRP.Actors.ProjectX;
using RocketRP.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketRP.Actors.TAGame
{
	public class RBActor_TA : Pawn_X
	{
		public override bool HasInitialRotation => true;

		public byte TeleportCounter { get; set; }
		public float ReplicatedCollisionScale { get; set; }
		public float ReplicatedGravityScale { get; set; }
		public WeldingInfo WeldedInfo { get; set; }
		public ReplicatedRBState ReplicatedRBState { get; set; }
		public bool bIgnoreSyncing { get; set; }
		public bool bFrozen { get; set; }
		public bool bReplayActor { get; set; }
		public float MaxAngularSpeed { get; set; }
		public float MaxLinearSpeed { get; set; }
	}
}
