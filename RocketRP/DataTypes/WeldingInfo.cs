using RocketRP.Actors.TAGame;

namespace RocketRP.DataTypes
{
	public struct WeldingInfo
	{
		public ObjectTarget<RBActor_TA> RBActor { get; set; }
		public Vector Offset { get; set; }
		public float Mass { get; set; }
		public Rotator Rotation { get; set; }

		public WeldingInfo(ObjectTarget<RBActor_TA> rbActor, Vector offset, float mass, Rotator rotation)
		{
			RBActor = rbActor;
			Offset = offset;
			Mass = mass;
			Rotation = rotation;
		}

		public static WeldingInfo Deserialize(BitReader br, Replay replay)
		{
			var rbActor = ObjectTarget<RBActor_TA>.Deserialize(br);
			var offset = Vector.Deserialize(br, replay);
			var mass = br.ReadSingle();
			var rotation = Rotator.Deserialize(br);

			return new WeldingInfo(rbActor, offset, mass, rotation);
		}

		public readonly void Serialize(BitWriter bw, Replay replay)
		{
			RBActor.Serialize(bw);
			Offset.Serialize(bw, replay);
			bw.Write(Mass);
			Rotation.Serialize(bw);
		}
	}
}
