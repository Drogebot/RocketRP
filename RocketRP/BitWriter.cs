using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketRP
{
	public class BitWriter
	{
		private BitArray Bits;
		public int Position { get; set; }
		public int Length => Bits.Length;

		public BitWriter(int initialCapacity = 1024)
		{
			Bits = new BitArray(initialCapacity);
			Position = 0;
		}

		public void Seek(int position)
		{
			if (position < 0 || position >= Length)
			{
				throw new ArgumentOutOfRangeException(nameof(position));
			}

			Position = position;
		}

		public void Write(bool value)
		{
			while (Position >= Bits.Length)
			{
				if (Bits.Length <= 0) Bits.Length = 1024;
				else Bits.Length *= 2;
			}

			Bits[Position++] = value;
		}

		public void Write(byte value)
		{
			WriteFixedBits(value, 8);
		}

		public void WriteFixedBits(byte value, int maxBits)
		{
			for(int i = 0; i < maxBits; i++)
			{
				Write((value & 1U) == 1U);
				value >>= 1;
			}
		}

		public void Write(uint value, uint maxValue)
		{
			var writtenValue = 0U;
			for (int i = 0; (writtenValue | (1U << i)) < maxValue; i++)
			{
				var bit = value & (1U << i);
				writtenValue |= bit;
				Write(bit > 0);
			}
		}

		public void WriteFixedBits(uint value, int numBits)
		{
			for (int i = 0; i < numBits; i++)
			{
				Write((value & 1U) == 1U);
				value >>= 1;
			}
		}

		public void Write(uint value)
		{
			WriteFixedBits(value, 32);
		}

		public void Write(int value, int maxValue)
		{
			Write((uint)value, (uint)maxValue);
		}

		public void WriteFixedBits(int value, int numBits)
		{
			WriteFixedBits((uint)value, numBits);
		}

		public void Write(int value)
		{
			Write((uint)value);
		}

		public void WriteFixedBits(ulong value, int numBits)
		{
			for (int i = 0; i < numBits; i++)
			{
				Write((value & 1U) == 1U);
				value >>= 1;
			}
		}

		public void Write(ulong value)
		{
			WriteFixedBits(value, 64);
		}

		public void ReadInt64FromBits(long value, int numBits)
		{
			WriteFixedBits((ulong)value, numBits);
		}

		public void Write(long value)
		{
			Write((ulong)value);
		}

		public void Write(byte[] bytes)
		{
			foreach(var value in bytes)
			{
				Write(value);
			}
		}

		public void Write(float value)
		{
			Write(BitConverter.GetBytes(value));
		}

		public void Write(string value)
		{
			var length = value.Length + 1;
			bool isUnicode = value.Any(c => c > 255);
			if (isUnicode)
			{
				length *= -1;
			}
			Write(length);

			if (!isUnicode)
			{
				Write(CodePagesEncodingProvider.Instance.GetEncoding(1252).GetBytes(value));
				Write((byte)0);
			}
			else
			{
				Write(Encoding.Unicode.GetBytes(value));
				Write((byte)0);
				Write((byte)0);
			}
		}

		public byte[] GetAllBytes()
		{

			var bytes = new byte[((Position - 1) >> 3) + ((Position & 7) > 0 ? 1 : 0)];
			if (Length < (bytes.Length << 3)) Bits.Length = bytes.Length << 3;

			for (var byteIndex = 0; byteIndex < bytes.Length; byteIndex++)
			{
				byte value = 0;
				for (int i = 0; i < 8; i++)
				{
					if (Bits[(byteIndex << 3) + i]) value |= (byte)(1 << i);
				}
				bytes[byteIndex] = value;
			}
			return bytes;
		}
	}
}
