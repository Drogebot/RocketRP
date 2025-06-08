using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketRP.DataTypes
{
	/// This class hasn't been encountered in any replays yet, so I'm sure it's entirely accurate
	public struct CustomMatchTeamSettings
	{
		public string? Name { get; set; }
		public ClubColorSet? Colors { get; set; }
		//public int? GameScore { get; set; }	//This seems to go unused

		public CustomMatchTeamSettings(string? name, ClubColorSet? colors)
		{
			Name = name;
			Colors = colors;
		}

		public static CustomMatchTeamSettings Deserialize(BitReader br)
		{
			var name = br.ReadString();
			var colors = ClubColorSet.Deserialize(br);

			return new CustomMatchTeamSettings(name, colors);
		}

		public void Serialize(BitWriter bw)
		{
			bw.Write(Name);
			Colors!.Value.Serialize(bw);
		}
	}
}
