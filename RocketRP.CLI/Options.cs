using CommandLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketRP.CLI
{
	public class Options
	{
		[Option('r', "replay", Required = true, HelpText = "Path to the Replay File/Directory")]
		public string ReplayPath { get; set; }

		[Option('o', "output", HelpText = "Path to the output Directory")]
		public string? OutputPath { get; set; }

		[Option('d', "directory", Default = false, HelpText = "Process entire Directory")]
		public bool DirectoryMode { get; set; }

		[Option('f', "fast", Default = false, HelpText = "Skips the Netstream")]
		public bool Fast { get; set; }

		[Option('c', "enforce-crc", Default = false, HelpText = "Fail if CRCs don't match the data")]
		public bool EnforceCRC { get; set; }

		[Option('p', "pretty", Default = false, HelpText = "Output JSON pretty-printed")]
		public bool PrettyPrint { get; set; }

		[Option('m', "mode", Default = SerializationMode.Deserialize, HelpText = $"{"Deserialize"} to JSON or {"Serialize"} from JSON")]
		public SerializationMode Mode { get; set; }
	}

	public enum SerializationMode
	{
		Deserialize,
		Serialize,
	}
}
