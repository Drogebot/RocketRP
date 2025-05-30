using RocketRP.Actors.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketRP.DataTypes
{
	/// This type no longer exists inside Rocket League, so I have no clue what the official names are
	public struct ReplicatedMusicStinger
	{
		public ObjectTarget<ClassObject> ObjectTarget { get; set; }
		public byte Unknown1 { get; set; }

		public ReplicatedMusicStinger(ObjectTarget<ClassObject> objectTarget, byte unknown1)
		{
			ObjectTarget = objectTarget;
			Unknown1 = unknown1;
		}

		public static ReplicatedMusicStinger Deserialize(BitReader br)
		{
			var objectTarget = ObjectTarget<ClassObject>.Deserialize(br);
			var unknown1 = br.ReadByte();

			return new ReplicatedMusicStinger(objectTarget, unknown1);
		}

		public void Serialize(BitWriter bw)
		{
			ObjectTarget.Serialize(bw);
			bw.Write(Unknown1);
		}
	}
}
