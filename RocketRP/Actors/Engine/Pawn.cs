using RocketRP.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketRP.Actors.Engine
{
	public class Pawn : Actor
	{
		public Vector RootMotionInterpCurveLastValue { get; set; }
		public float RootMotionInterpCurrentTime { get; set; }
		public float RootMotionInterpRate { get; set; }
		public ObjectTarget<PlayerReplicationInfo> PlayerReplicationInfo { get; set; }
		public float AirControl { get; set; }
		public float JumpZ { get; set; }
		public float AccelRate { get; set; }
		public float AirSpeed { get; set; }
		public float GroundSpeed { get; set; }
		public byte RemoteViewPitch { get; set; }
		public bool bFastAttachedMove { get; set; }
		public bool bRootMotionFromInterpCurve { get; set; }
		public bool bUsedByMatinee { get; set; }
		public bool bCanSwatTurn { get; set; }
		public bool bSimulateGravity { get; set; }
		public bool bIsCrouched { get; set; }
		public bool bIsWalking { get; set; }
		public ObjectTarget<Actor> Controller { get; set; }	// Is of Controller type, but those don't appear in replays


		// These are old properties that were removed
		public uint HealthMax { get; set; }
	}
}
