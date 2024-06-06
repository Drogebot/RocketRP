using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketRP.DataTypes
{
	public struct Vector
	{
		public float X { get; set; }
		public float Y { get; set; }
		public float Z { get; set; }

		public Vector(float x, float y, float z)
		{
			X = x;
			Y = y;
			Z = z;
		}

		public static Vector operator *(Vector vector, int scalar)
		{
			vector.X *= scalar;
			vector.Y *= scalar;
			vector.Z *= scalar;
			return vector;
		}

		public static Vector operator /(Vector vector, int scalar)
		{
			vector.X /= scalar;
			vector.Y /= scalar;
			vector.Z /= scalar;
			return vector;
		}

		public static Vector Deserialize(BitReader br, Replay replay)
		{
			var maxValuePerComponent = replay.NetVersion >= 7 ? 22 : 20;

			var numBits = br.ReadInt32Max(maxValuePerComponent);
			var bias = 1 << (numBits + 1);
			var maxBits = numBits + 2;

			var x = br.ReadInt32FromBits(maxBits) - bias;
			var y = br.ReadInt32FromBits(maxBits) - bias;
			var z = br.ReadInt32FromBits(maxBits) - bias;

			return new Vector(x, y, z);
		}

		public void Serialize(BitWriter bw, Replay replay)
		{
			var maxValuePerComponent = replay.NetVersion >= 7 ? 22 : 20;

			var x = (int)Math.Round(X);
			var y = (int)Math.Round(Y);
			var z = (int)Math.Round(Z);

			var maxValue = Math.Max(Math.Max(Math.Abs(x), Math.Abs(y)), Math.Abs(z));
			var numBitsForValue = (int)Math.Ceiling(Math.Log2(maxValue + 1));
			var numBits = Math.Min(Math.Max(1, numBitsForValue), maxValuePerComponent) - 1;
			var bias = 1 << (numBits + 1);
			var maxBits = numBits + 2;
			var max = 1 << (numBits + 2);

			bw.Write(numBits, maxValuePerComponent);

			var dx = x + bias;
			var dy = y + bias;
			var dz = z + bias;

			if (dx >= max) dx = max - 1;
			if (dy >= max) dy = max - 1;
			if (dz >= max) dz = max - 1;

			bw.WriteFixedBits(dx, maxBits);
			bw.WriteFixedBits(dy, maxBits);
			bw.WriteFixedBits(dz, maxBits);
		}

		public static Vector DeserializeFixedPoint(BitReader br, Replay replay)
		{
			var vector = Deserialize(br, replay);
			
			vector /= 100;

			return vector;
		}

		public void SerializeFixedPoint(BitWriter bw, Replay replay)
		{
			this *= 100;

			Serialize(bw, replay);
		}

		public static Vector DeserializeFixed(BitReader br, int numBits)
		{
			var maxBitValue = (1 << (numBits - 1)) - 1;
			var bias = 1 << (numBits - 1);

			var x = br.ReadInt32FromBits(numBits) - bias;
			var y = br.ReadInt32FromBits(numBits) - bias;
			var z = br.ReadInt32FromBits(numBits) - bias;

			return new Vector(x, y, z) / maxBitValue;
		}

		public void SerializeFixed(BitWriter bw, int numBits)
		{
			var maxBitValue = (1 << (numBits - 1)) - 1;
			var bias = 1 << (numBits - 1);

			this *= maxBitValue;

			bw.WriteFixedBits((int)X + bias, numBits);
			bw.WriteFixedBits((int)Y + bias, numBits);
			bw.WriteFixedBits((int)Z + bias, numBits);
		}
	}
}
