using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketRP
{
	public class BitReader
	{
		private BitArray Bits;
		public int Position { get; set; }
		public int Length => Bits.Length;

		public BitReader(byte[] data)
		{
			Bits = new BitArray(data);
		}

		public void Seek(int position)
		{
			if(position < 0 || position >= Length)
			{
				throw new ArgumentOutOfRangeException(nameof(position));
			}

			Position = position;
		}

		public bool ReadBit()
		{
			return Bits[Position++];
		}

		public byte ReadByteFromBits(int numBits)
		{
			byte value = 0;
			for(int i = 0; i < numBits; i++)
			{
				if(ReadBit()) value |= (byte)(1 << i);
			}
			return value;
		}

		public byte ReadByte()
		{
			return ReadByteFromBits(8);
		}

		public uint ReadUInt32Max(uint maxValue)
		{
			uint value = 0;
			for (int i = 0; (value | (1U << i)) < maxValue; i++)
			{
				if (ReadBit()) value |= 1U << i;
			}
			return value;
		}

		public uint ReadUInt32FromBits(int numBits)
		{
			uint value = 0;
			for (int i = 0; i < numBits; i++)
			{
				if (ReadBit()) value |= 1U << i;
			}
			return value;
		}

		public uint ReadUInt32()
		{
			return ReadUInt32FromBits(32);
		}

		public int ReadInt32Max(int maxValue)
		{
			return (int)ReadUInt32Max((uint)maxValue);
		}

		public int ReadInt32FromBits(int numBits)
		{
			return (int)ReadUInt32FromBits(numBits);
		}

		public int ReadInt32()
		{
			return ReadInt32FromBits(32);
		}

		public ulong ReadUInt64FromBits(int numBits)
		{
			ulong value = 0;
			for (int i = 0; i < numBits; i++)
			{
				if (ReadBit()) value |= 1UL << i;
			}
			return value;
		}

		public ulong ReadUInt64()
		{
			return ReadUInt64FromBits(64);
		}

		public long ReadInt64FromBits(int numBits)
		{
			return (long)ReadUInt64FromBits(numBits);
		}

		public long ReadInt64()
		{
			return ReadInt64FromBits(64);
		}

		public byte[] ReadBytes(int count)
		{
			var bytes = new byte[count];
			for (int i = 0; i < count; i++)
			{
				bytes[i] = ReadByte();
			}
			return bytes;
		}

		public float ReadSingle()
		{
			return BitConverter.ToSingle(ReadBytes(4), 0);
		}

		public string ReadString()
		{
			int length = ReadInt32();
			if (length > 0)
			{
				return CodePagesEncodingProvider.Instance.GetEncoding(1252).GetString(ReadBytes(length), 0, length - 1);
			}
			else if(length < 0)
			{
				return Encoding.Unicode.GetString(ReadBytes(length * -2), 0, length * -2 - 2);
			}
			return String.Empty;
		}
	}
}
