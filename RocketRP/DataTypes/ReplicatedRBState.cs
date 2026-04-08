using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketRP.DataTypes
{
	public struct ReplicatedRBState
	{
		public bool? Sleeping { get; set; }
		public Vector? Position { get; set; }
		public Quat? Rotation { get; set; }
		public Vector? LinearVelocity { get; set; }
		public Vector? AngularVelocity { get; set; }

		public ReplicatedRBState(bool? sleeping, Vector? position, Quat? rotation, Vector? linearVelocity, Vector? angularVelocity)
		{
			Sleeping = sleeping;
			Position = position;
			Rotation = rotation;
			LinearVelocity = linearVelocity;
			AngularVelocity = angularVelocity;
		}

		public static ReplicatedRBState Deserialize(BitReader br, Replay replay)
		{
			var sleeping = br.ReadBit();

			Vector position = Vector.Deserialize(br, replay);
			if (replay.NetVersion >= 5) position /= 100f;

			Quat rotation = replay.NetVersion >= 7
				? Quat.Deserialize(br, replay)
				: new Quat(Rotator.DeserializeUncompressed(br, replay));

			Vector linearVelocity = default;
			Vector angularVelocity = default;
			if (!sleeping)
			{
				var velocityScalar = replay.NetVersion >= 5 ? 100f : 10f;
				linearVelocity = Vector.Deserialize(br, replay) / velocityScalar;
				angularVelocity = Vector.Deserialize(br, replay) / velocityScalar;
			}

			return new ReplicatedRBState(sleeping, position, rotation, linearVelocity, angularVelocity);
		}

		public void Serialize(BitWriter bw, Replay replay)
		{
			bw.Write(Sleeping);

			var position = Position;
			if (replay.NetVersion >= 5) position *= 100;
			position!.Value.Serialize(bw, replay);

			if (replay.NetVersion >= 7) Rotation!.Value.Serialize(bw, replay);
			else (new Rotator(Rotation!.Value)).SerializeUncompressed(bw, replay);

			if (!Sleeping.Value)
			{
				var velocityScalar = replay.NetVersion >= 5 ? 100f : 10f;
				(LinearVelocity!.Value * velocityScalar).Serialize(bw, replay);
				(AngularVelocity!.Value * velocityScalar).Serialize(bw, replay);
			}
		}
	}
}
