using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketRP.DataTypes
{
	public struct LoadoutTeamPaint
	{
		public int? TeamFinishID { get; set; }
		public int? CustomFinishID { get; set; }
		public byte? Team { get; set; }
		public byte? TeamColorID { get; set; }
		public byte? CustomColorID { get; set; }
		public bool? bSet { get; set; }

		public LoadoutTeamPaint(byte? team, byte? teamColorId, byte? customColorId, int? teamFinishId, int? customFinishId)
		{
			Team = team;
			TeamColorID = teamColorId;
			CustomColorID = customColorId;
			TeamFinishID = teamFinishId;
			CustomFinishID = customFinishId;
			bSet = true;
		}

		public static LoadoutTeamPaint Deserialize(BitReader br)
		{
			var team = br.ReadByte();
			var teamColorId = br.ReadByte();
			var customColorId = br.ReadByte();
			var teamFinishId = br.ReadInt32();
			var customFinishId = br.ReadInt32();
			return new LoadoutTeamPaint(team, teamColorId, customColorId, teamFinishId, customFinishId);
		}

		public void Serialize(BitWriter bw)
		{
			bw.Write(Team);
			bw.Write(TeamColorID);
			bw.Write(CustomColorID);
			bw.Write(TeamFinishID);
			bw.Write(CustomFinishID);
		}
	}
}
