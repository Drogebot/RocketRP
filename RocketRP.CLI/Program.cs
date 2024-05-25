using CommandLine;
using RocketRP;
using RocketRP.CLI;
using RocketRP.Serializers;
using System.Text;

if (!args.Any())
{
	args = ["--help"];
}

Parser.Default.ParseArguments<Options>(args)
.WithParsed<Options>(opts =>
{
	var replayFileInfo = new FileInfo(opts.ReplayPath);
	opts.ReplayPath = replayFileInfo.FullName;
	opts.OutputPath = opts.OutputPath ?? replayFileInfo?.Directory?.FullName;
	if (opts.OutputPath == null)
	{
		Console.WriteLine("Output path could not be determined!");
		return;
	}

	if (!opts.DirectoryMode)
	{
		ParseReplay(opts.ReplayPath, opts.OutputPath, opts.ParseNetstream, opts.EnforceCRC, opts.PrettyPrint);
	}
	else
	{
		if(replayFileInfo?.Directory == null)
		{
			Console.WriteLine("Invalid replay path!");
			return;
		}

		var replayFiles = replayFileInfo.Directory.GetFiles("*.replay", SearchOption.TopDirectoryOnly);

		foreach (var replayFile in replayFiles)
		{
			ParseReplay(replayFile.FullName, opts.OutputPath, opts.ParseNetstream, opts.EnforceCRC, opts.PrettyPrint);
		}
	}
});

static void ParseReplay(string replayPath, string outputPath, bool parseNetstream, bool enforceCRC, bool prettyPrint)
{
	Console.WriteLine($"Parsing replay {replayPath}...");
	try
	{
		var replay = Replay.Deserialize(replayPath, parseNetstream, enforceCRC);

		Console.WriteLine($"Converting to JSON...");
		var serializer = new JsonSerializer();
		var outputData = serializer.Serialize(replay, prettyPrint);

		var outputFilePath = Path.Combine(outputPath, Path.GetFileNameWithoutExtension(replayPath) + ".json");

		File.WriteAllText(outputFilePath, outputData);

		Console.WriteLine($"Parsed replay {replayPath}!");
	}
	catch (Exception e)
	{
		Console.WriteLine($"Failed to parse replay: {e.Message}");
		return;
	}
}