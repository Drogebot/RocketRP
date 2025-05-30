using RocketRP.Actors.TAGame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketRP.DataTypes
{
	public struct WeldingInfo
	{
		public ObjectTarget<RBActor_TA> RBActor;
		Vector Offset;
		float Mass;
		Rotator Rotation;

		public WeldingInfo(ObjectTarget<RBActor_TA> rbActor, Vector offset, float mass, Rotator rotation)
		{
			this.RBActor = rbActor;
			this.Offset = offset;
			this.Mass = mass;
			this.Rotation = rotation;
		}

		public static WeldingInfo Deserialize(BitReader br, Replay replay)
		{
			var rbActor = ObjectTarget<RBActor_TA>.Deserialize(br);
			var offset = Vector.Deserialize(br, replay);
			var mass = br.ReadSingle();
			var rotation = Rotator.Deserialize(br);

			return new WeldingInfo(rbActor, offset, mass, rotation);
		}

		public void Serialize(BitWriter bw, Replay replay)
		{
			RBActor.Serialize(bw);
			Offset.Serialize(bw, replay);
			bw.Write(Mass);
			Rotation.Serialize(bw);
		}
	}
}
