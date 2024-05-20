using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketRP.Actors.Engine
{
	public class GameReplicationInfo : ReplicationInfo
	{
		public ObjectTarget Winner { get; set; }
		public string ServerName { get; set; }
		public int TimeLimit { get; set; }
		public int GoalScore { get; set; }
		public int RemainingMinute { get; set; }
		public int ElapsedTime { get; set; }
		public int RemainingTime { get; set; }
		public bool bMatchIsOver { get; set; }
		public bool bMatchHasBegun { get; set; }
		public bool bStopCountDown { get; set; }
		public ObjectTarget GameClass { get; set; }
	}
}
