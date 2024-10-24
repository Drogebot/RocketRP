using RocketRP.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketRP.Actors.TAGame
{
	public class Replay_TA : Core.Object
	{
		public string? ReplayName { get; set; }
		public int? ReplayVersion { get; set; }
		public int? ReplayLastSaveVersion { get; set; }
		public int? GameVersion { get; set; }
		public int? BuildID { get; set; }
		public int? Changelist { get; set; }
		public string? BuildVersion { get; set; }
		public int? ReserveMegabytes { get; set; }
		public float? RecordFPS { get; set; }
		public float? KeyframeDelay { get; set; }
		public int? MaxChannels { get; set; }
		public int? MaxReplaySizeMB { get; set; }
		public string? Id { get; set; }
		public string? MatchGuid { get; set; }
		public Name? MapName { get; set; }
		public string? Date { get; set; }
		public int? NumFrames { get; set; }
		public Name? MatchType { get; set; }
		public string? PlayerName { get; set; }
	}
}
