using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketRP.Actors.TAGame
{
	public class Replay_TA : Core.Object
	{
		public StrProperty? ReplayName { get; set; }

		public IntProperty? ReplayVersion { get; set; }

		public IntProperty? ReplayLastSaveVersion { get; set; }

		public IntProperty? GameVersion { get; set; }

		public IntProperty? BuildID { get; set; }

		public IntProperty? Changelist { get; set; }

		public StrProperty? BuildVersion { get; set; }

		public IntProperty? ReserveMegabytes { get; set; }

		public FloatProperty? RecordFPS { get; set; }

		public FloatProperty? KeyframeDelay { get; set; }

		public IntProperty? MaxChannels { get; set; }

		public IntProperty? MaxReplaySizeMB { get; set; }

		public StrProperty? Id { get; set; }

		public NameProperty? MapName { get; set; }

		public StrProperty? Date { get; set; }

		public IntProperty? NumFrames { get; set; }

		public NameProperty? MatchType { get; set; }

		public StrProperty? PlayerName { get; set; }
	}
}
