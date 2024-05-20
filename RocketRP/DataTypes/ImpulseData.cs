using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketRP.DataTypes
{
	public struct ImpulseData
	{
		public int CompressedRotation { get; set; }
		public float ImpulseSpeed { get; set; }

		public ImpulseData(int compressedRotation, float impulseSpeed)
		{
			CompressedRotation = compressedRotation;
			ImpulseSpeed = impulseSpeed;
		}

		public static ImpulseData Deserialize(BitReader br)
		{
			var compressedRotation = br.ReadInt32();
			var impulseSpeed = br.ReadSingle();

			return new ImpulseData(compressedRotation, impulseSpeed);
		}

		public void Serialize(BitWriter bw)
		{
			bw.Write(CompressedRotation);
			bw.Write(ImpulseSpeed);
		}
	}
}
