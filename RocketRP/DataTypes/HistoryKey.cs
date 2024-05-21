using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketRP.DataTypes
{
	/// IDK why this typing is so different from the original code, but it works
	public struct HistoryKey
	{
		public uint Value { get; set; }

		public HistoryKey(uint value)
		{
			Value = value;
		}

		public static HistoryKey Deserialize(BitReader br)
		{
			var value = br.ReadUInt32FromBits(14);  // This might instead be ReadUInt32Max with replay.Properties["NumFrames"]

			return new HistoryKey(value);
		}

		public void Serialize(BitWriter bw)
		{
			bw.WriteFixedBits(Value, 14);
		}
	}
}
