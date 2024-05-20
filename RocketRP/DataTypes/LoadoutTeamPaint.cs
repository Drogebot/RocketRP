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
		public byte TeamId { get; set; }
		public byte TeamColorId { get; set; }
		public byte CustomColorId { get; set; }
		public uint TeamFinishId { get; set; }
		public uint CustomFinishId { get; set; }

		public LoadoutTeamPaint(byte teamId, byte teamColorId, byte customColorId, uint teamFinishId, uint customFinishId)
		{
			TeamId = teamId;
			TeamColorId = teamColorId;
			CustomColorId = customColorId;
			TeamFinishId = teamFinishId;
			CustomFinishId = customFinishId;
		}

		public static LoadoutTeamPaint Deserialize(BitReader br)
		{
			var teamId = br.ReadByte();
			var teamColorId = br.ReadByte();
			var customColorId = br.ReadByte();
			var teamFinishId = br.ReadUInt32();
			var customFinishId = br.ReadUInt32();
			return new LoadoutTeamPaint(teamId, teamColorId, customColorId, teamFinishId, customFinishId);
		}

		public void Serialize(BitWriter bw)
		{
			bw.Write(TeamId);
			bw.Write(TeamColorId);
			bw.Write(CustomColorId);
			bw.Write(TeamFinishId);
			bw.Write(CustomFinishId);
		}
	}
}
