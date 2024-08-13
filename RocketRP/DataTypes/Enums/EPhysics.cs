using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketRP.DataTypes.Enums
{
	public enum EPhysics : byte
	{
		PHYS_None,
		PHYS_Walking,
		PHYS_Falling,
		PHYS_Swimming,
		PHYS_Flying,
		PHYS_Rotating,
		PHYS_Projectile,
		PHYS_Interpolating,
		PHYS_Spider,
		PHYS_RigidBody,
		PHYS_SoftBody,
		PHYS_NavMeshWalking,
		PHYS_Unused,
		PHYS_Custom,
		PHYS_END,
	}
}
