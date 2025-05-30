using RocketRP.Actors.Core;
using RocketRP.Actors.TAGame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketRP.DataTypes
{
	public struct MemberTitleStat
	{
		public ObjectTarget<ClassObject> Category { get; set; }
		public ObjectTarget<ClassObject> Title { get; set; }
		public int PointsEarned { get; set; }
		public int StatCount { get; set; }
		public ObjectTarget<PRI_TA> MemberPRI { get; set; }

		public MemberTitleStat(ObjectTarget<ClassObject> category, ObjectTarget<ClassObject> title, int pointsEarned, int statCount, ObjectTarget<PRI_TA> memberPRI)
		{
			Category = category;
			Title = title;
			PointsEarned = pointsEarned;
			StatCount = statCount;
			MemberPRI = memberPRI;
		}

		public static MemberTitleStat Deserialize(BitReader br)
		{
			var category = ObjectTarget<ClassObject>.Deserialize(br);
			var title = ObjectTarget<ClassObject>.Deserialize(br);
			var pointsEarned = br.ReadInt32();
			var statCount = br.ReadInt32();
			var memberPRI = ObjectTarget<PRI_TA>.Deserialize(br);

			return new MemberTitleStat(category, title, pointsEarned, statCount, memberPRI);
		}

		public void Serialize(BitWriter bw)
		{
			Category.Serialize(bw);
			Title.Serialize(bw);
			bw.Write(PointsEarned);
			bw.Write(StatCount);
			MemberPRI.Serialize(bw);
		}
	}
}
