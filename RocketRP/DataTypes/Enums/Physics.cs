using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketRP.DataTypes.Enums
{
	public enum Physics : byte
	{
		None,
		Walking,
		Falling,
		Swimming,
		Flying,
		Rotating,
		Projectile,
		Interpolating,
		Spider,
		RigidBody,
		SoftBody,
		NavMeshWalking,
		Unused,
		Custom,
		END,
	}
}
