using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketRP.DataTypes
{
	/// This type only exists because GameServerID was changed from long to string at some point
	public struct GameServerID
	{
		public string Value { get; set; }

		public GameServerID(string value)
		{
			Value = value;
		}

		public static GameServerID Deserialize(BitReader br, Replay	replay)
		{
			var value = "";
			if(replay.Changelist >= 406184) value = br.ReadString();
			else value = br.ReadInt64().ToString();

			return new GameServerID(value);
		}

		public void Serialize(BitWriter bw, Replay replay)
		{
			if (replay.Changelist >= 406184) bw.Write(Value);
			else bw.Write(Int64.Parse(Value));
		}
	}
}
