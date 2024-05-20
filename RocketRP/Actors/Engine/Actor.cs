using RocketRP.Actors.Core;
using RocketRP.DataTypes;
using RocketRP.DataTypes.Enums;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketRP.Actors.Engine
{
	public class Actor : UEObject
	{
		public virtual bool HasInitialPosition { get => true; }
		public virtual bool HasInitialRotation { get => false; }

		public Vector InitialPosition { get; set; }
		public Rotator InitialRotation { get; set; }

		public Rotator RelativeRotation { get; set; }
		public Vector RelativeLocation { get; set; }
		public Vector Velocity { get; set; }
		public ObjectTarget Instigator { get; set; }
		public bool bNetOwner { get; set; }
		public bool bBlockActors { get; set; }
		public bool bCollideWorld { get; set; }
		public bool bCollideActors { get; set; }
		public bool bHardAttach { get; set; }
		public bool bTearOff { get; set; }
		public bool bHidden { get; set; }
		public ObjectTarget Base { get; set; }
		public ObjectTarget Owner { get; set; }
		public CollisionType ReplicatedCollisionType { get; set; }
		public Role Role { get; set; }
		public Role RemoteRole { get; set; }
		public Physics Physics { get; set; }
		public float DrawScale { get; set; }
		public Rotator Rotation { get; set; }
		public Vector Location { get; set; }
	}
}
