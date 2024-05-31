using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketRP.DataTypes
{
	public struct Vector
	{
		public int X { get; set; }
		public int Y { get; set; }
		public int Z { get; set; }

		public Vector(int x, int y, int z)
		{
			X = x;
			Y = y;
			Z = z;
		}

		public static Vector Deserialize(BitReader br, Replay replay)
		{
			var maxValuePerComponent = replay.NetVersion >= 7 ? 22 : 20;

			var numBits = br.ReadInt32Max(maxValuePerComponent);
			var bias = 1 << (numBits + 1);
			var max = numBits + 2;

			var x = (int)br.ReadUInt32FromBits(max) - bias;
			var y = (int)br.ReadUInt32FromBits(max) - bias;
			var z = (int)br.ReadUInt32FromBits(max) - bias;

			return new Vector(x, y, z);
		}

		public void Serialize(BitWriter bw, Replay replay)
		{
			/// This code was taken from https://github.com/jjbott/RocketLeagueReplayParser
			uint maxValuePerComponent = replay.NetVersion >= 7 ? 22U : 20U;

			Int32 maxValue = Math.Max(Math.Max(Math.Abs(X), Math.Abs(Y)), Math.Abs(Z));
			int numBitsForValue = (int)Math.Ceiling(Math.Log10(maxValue + 1) / Math.Log10(2));

			UInt32 Bits = (UInt32)Math.Min(Math.Max(1, numBitsForValue), maxValuePerComponent) - 1;

			bw.Write(Bits, maxValuePerComponent);

			Int32 Bias = 1 << (int)(Bits + 1);
			UInt32 Max = (UInt32)(1 << (int)(Bits + 2));
			UInt32 DX = (UInt32)(X + Bias);
			UInt32 DY = (UInt32)(Y + Bias);
			UInt32 DZ = (UInt32)(Z + Bias);

			if (DX >= Max) { DX = unchecked((Int32)DX) > 0 ? Max - 1 : 0; }
			if (DY >= Max) { DY = unchecked((Int32)DY) > 0 ? Max - 1 : 0; }
			if (DZ >= Max) { DZ = unchecked((Int32)DZ) > 0 ? Max - 1 : 0; }

			bw.Write(DX, Max);
			bw.Write(DY, Max);
			bw.Write(DZ, Max);
		}

		public static Vector DeserializeFixed(BitReader br)
		{
			var numBits = 16;
			var bias = 1 << (numBits - 1);

			var x = (int)br.ReadUInt32FromBits(numBits) - bias;
			var y = (int)br.ReadUInt32FromBits(numBits) - bias;
			var z = (int)br.ReadUInt32FromBits(numBits) - bias;

			return new Vector(x, y, z);
		}

		public void SerializeFixed(BitWriter bw)
		{
			var numBits = 16;
			var bias = 1 << (numBits - 1);

			bw.WriteFixedBits(X + bias, numBits);
			bw.WriteFixedBits(Y + bias, numBits);
			bw.WriteFixedBits(Z + bias, numBits);
		}
	}
}
