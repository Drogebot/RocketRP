using CommandLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace RocketRP.TrainingCLI
{
	public class Options
	{
		[Option('f', "training", Required = true, HelpText = "Path to the Training File/Directory")]
		public string TrainingPath { get; set; }

		[Option('o', "output", HelpText = "Path to the output Directory")]
		public string? OutputPath { get; set; }

		[Option('d', "directory", Default = false, HelpText = "Process entire Directory")]
		public bool DirectoryMode { get; set; }

		[Option('t', "threads", Default = 10, HelpText = "Number of Threads to run in Directory Mode")]
		public int Threads { get; set; }

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
