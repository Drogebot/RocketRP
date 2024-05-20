using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketRP.DataTypes
{
	/// This type only exists because GameServerID was changed from EnumMax to byte at some point
	public struct GameMode
	{
		public byte Value { get; set; }

		public GameMode(byte value)
		{
			Value = value;
		}

		public static GameMode Deserialize(BitReader br, Replay replay)
		{
			Byte value;
			if(replay.EngineVersion >= 868 && replay.LicenseeVersion >= 12) value = br.ReadByte();
			else value = (byte)br.ReadUInt32Max(4);

			return new GameMode(value);
		}

		public void Serialize(BitWriter bw, Replay replay)
		{
			if (replay.EngineVersion >= 868 && replay.LicenseeVersion >= 12) bw.Write(Value);
			else bw.Write(Value, 4);
		}
	}
}
