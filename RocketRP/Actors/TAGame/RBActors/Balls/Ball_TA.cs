using RocketRP.Actors.Core;
using RocketRP.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketRP.Actors.TAGame
{
	public class Ball_TA : RBActor_TA
	{
		public ExplosionDataExtended ReplicatedExplosionDataExtended { get; set; }
		public ExplosionData ReplicatedExplosionData { get; set; }
		public ObjectTarget<GameEvent_Soccar_TA> GameEvent { get; set; }
		public int GameBallIndex { get; set; }
		public ObjectTarget<ClassObject> ReplicatedPhysMatOverride { get; set; }
		public float AdditionalCarGroundBounceScaleXY { get; set; }
		public float AdditionalCarGroundBounceScaleZ { get; set; }
		public float ReplicatedAddedCarBounceScale { get; set; }
		public float ReplicatedBallMaxLinearSpeedScale { get; set; }
		public float ReplicatedBallGravityScale { get; set; }
		public float ReplicatedWorldBounceScale { get; set; }
		public ObjectTarget<ClassObject> ReplicatedBallMesh { get; set; }
		public float ReplicatedBallScale { get; set; }
		public byte HitTeamNum { get; set; }
		public float BallHitSpinScale { get; set; }
		public float MagnusMinSpeed { get; set; }
		public Vector MagnusCoefficient { get; set; }
		public bool bPossessionEnabled { get; set; }
		public bool bWarnBallReset { get; set; }
		public bool bEndOfGameHidden { get; set; }
	}
}
