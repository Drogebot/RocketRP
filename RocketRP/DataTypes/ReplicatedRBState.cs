using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketRP.DataTypes
{
	public struct ReplicatedRBState
	{
		public bool Sleeping { get; set; }
		public Vector Position { get; set; }
		public object Rotation { get; set; }	// Can be Vector or Quat
		public Vector LinearVelocity { get; set; }
		public Vector AngularVelocity { get; set; }

		public ReplicatedRBState(bool sleeping, Vector position, object rotation, Vector linearVelocity, Vector angularVelocity)
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
			
			Vector position;
			if(replay.NetVersion >= 5) position = Vector.DeserializeFixedPoint(br, replay);
			else position = Vector.Deserialize(br, replay);

			object rotation;
			if(replay.NetVersion >= 7) rotation = Quat.Deserialize(br);
			else rotation = Vector.DeserializeFixed(br, 16);

			Vector linearVelocity = default, angularVelocity = default;
			if (!sleeping)
			{
				linearVelocity = Vector.Deserialize(br, replay);
				angularVelocity = Vector.Deserialize(br, replay);
			}

			return new ReplicatedRBState(sleeping, position, rotation, linearVelocity, angularVelocity);
		}

		public void Serialize(BitWriter bw, Replay replay)
		{
			bw.Write(Sleeping);

			if (replay.NetVersion >= 5) Position.SerializeFixedPoint(bw, replay);
			else Position.Serialize(bw, replay);

			if (replay.NetVersion >= 7) ((Quat)Rotation).Serialize(bw);
			else ((Vector)Rotation).SerializeFixed(bw, 16);

			if (!Sleeping)
			{
				LinearVelocity.Serialize(bw, replay);
				AngularVelocity.Serialize(bw, replay);
			}
		}
	}
}
