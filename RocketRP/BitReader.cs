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
		private static readonly byte[] GShift = { 0x01, 0x02, 0x04, 0x08, 0x10, 0x20, 0x40, 0x80 };
		private static readonly byte[] GMask = { 0x00, 0x01, 0x03, 0x07, 0x0f, 0x1f, 0x3f, 0x7f };

		private byte[] Buffer;
		private int Pos;
		private int Max;

		public bool AtEnd => Pos >= Max;
		public int NumBytes => (Pos + 7) >> 3;
		public int Position => Pos;
		public int Length => Max;

		public BitReader(byte[] data, int? countBits = null)
		{
			Max = countBits ?? data.Length << 3;
			Buffer = data;
		}

		public unsafe void SetData(BitReader src, int countBits)
		{
			if (countBits <= 0) return;
			Max = countBits;
			Pos = 0;
			Buffer = new byte[(countBits + 7) >> 3];
			fixed (byte* addr = &Buffer[0])
				src.SerializeBits(addr, countBits);
		}

		private unsafe void SerializeBits(void* dest, int lengthBits)
		{
			//Array.Clear(*(byte[]*)dest, 0, (lengthBits + 7) >> 3);
			if (lengthBits == 1)
			{
				if ((Buffer[Pos >> 3] & GShift[Pos & 7]) > 0)
				{
					((byte*)dest)[0] |= 1;
				}
				Pos++;
			}
			else
			{
				if (Pos + lengthBits > Max) throw new OverflowException("Buffer overflow");
				fixed (byte* addr = &Buffer[0])
					BitArray.BitsCopy((byte*)dest, 0, addr, Pos, lengthBits);
				Pos += lengthBits;
			}
		}

		private void SerializeInt(ref UInt32 value, UInt32 valueMax)
		{
			value = 0;
			for (UInt32 mask = 1; (value | mask) < valueMax && mask > 0; mask <<= 1, Pos++)
			{
				if ((Buffer[Pos >> 3] & GShift[Pos & 7]) > 0)
				{
					value |= mask;
				}
			}
		}

		public unsafe Boolean ReadBit()
		{
			Boolean value = (Buffer[Pos >> 3] & GShift[Pos & 7]) > 0;
			Pos++;
			return value;
		}

		public unsafe Byte ReadByte()
		{
			Byte value;
			SerializeBits(&value, sizeof(Byte) << 3);
			return value;
		}

		public Int32 ReadInt32(UInt32 valueMax)
		{
			UInt32 value = 0;
			SerializeInt(ref value, valueMax);
			return (Int32)value;
		}

		public UInt32 ReadUInt32(UInt32 valueMax)
		{
			UInt32 value = 0;
			SerializeInt(ref value, valueMax);
			return value;
		}

		public unsafe Int32 ReadInt32()
		{
			Int32 value = 0;
			SerializeBits(&value, sizeof(Int32) << 3);
			return value;
		}

		public unsafe UInt32 ReadUInt32()
		{
			UInt32 value = 0;
			SerializeBits(&value, sizeof(UInt32) << 3);
			return value;
		}

		public unsafe Int64 ReadInt64()
		{
			Int64 value = 0;
			SerializeBits(&value, sizeof(Int64) << 3);
			return value;
		}

		public unsafe UInt64 ReadUInt64()
		{
			UInt64 value = 0;
			SerializeBits(&value, sizeof(UInt64) << 3);
			return value;
		}

		public unsafe Single ReadSingle()
		{
			Single value = 0;
			SerializeBits(&value, sizeof(Single) << 3);
			return value;
		}

		public unsafe Byte[] ReadBytes(int count)
		{
			var bytes = new Byte[count];
			if (count <= 0) return bytes;
			fixed (byte* addr = &bytes[0])
				SerializeBits(addr, (sizeof(Byte) * count) << 3);
			return bytes;
		}

		public unsafe string ReadString(int? fixedLength = null)
		{
			Int32 length = fixedLength ?? ReadInt32();
			if (length < 0)
			{
				length = length * -1 - 1;
				var value = new string(new Char[length]);
				fixed (void* addr = &value.GetPinnableReference())
					SerializeBits(addr, (sizeof(Char) * length) << 3);
				Pos += 16; // Skip the null terminator
				return value;
			}
			else if (length > 0)
			{
				length = length - 1;
				var value = CodePagesEncodingProvider.Instance.GetEncoding(1252).GetString(ReadBytes(length));
				Pos += 8; // Skip the null terminator
				return value;
			}

			return null;
		}
	}
}
