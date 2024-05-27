﻿using CommandLine;
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
		ParseReplay(opts.ReplayPath, opts.OutputPath, opts.ParseNetstream, opts.EnforceCRC, opts.PrettyPrint, opts.Mode);
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
			ParseReplay(replayFile.FullName, opts.OutputPath, opts.ParseNetstream, opts.EnforceCRC, opts.PrettyPrint, opts.Mode);
		}
	}
});

static void ParseReplay(string replayPath, string outputPath, bool parseNetstream, bool enforceCRC, bool prettyPrint, SerializationMode mode)
{
	var serializer = new JsonSerializer();

	if (mode == SerializationMode.Deserialize)
	{
		try
		{
			Console.WriteLine($"Parsing replay: {replayPath}...");
			var replay = Replay.Deserialize(replayPath, parseNetstream, enforceCRC);

			var outputFilePath = Path.Combine(outputPath, Path.GetFileNameWithoutExtension(replayPath) + ".json");

			Console.WriteLine($"Converting to JSON...");
			var jsonData = serializer.Serialize(replay, prettyPrint);
			File.WriteAllText(outputFilePath, jsonData);

			Console.WriteLine($"Parsed replay: {outputFilePath}!");
		}
		catch (Exception e)
		{
			Console.WriteLine($"Failed to parse replay: {e.Message}");
			return;
		}
	}
	else if(mode == SerializationMode.Serialize)
	{
		try
		{
			Console.WriteLine($"Parsing JSON: {replayPath}...");
			var jsonData = File.ReadAllText(replayPath);
			var replay = (Replay)serializer.Deserialize(jsonData);

			var outputFilePath = Path.Combine(outputPath, Path.GetFileNameWithoutExtension(replayPath) + "_RocketRP.replay");

			Console.WriteLine($"Converting to replay...");
			replay.Serialize(outputFilePath);

			Console.WriteLine($"Converted to replay: {outputFilePath}!");
		}
		catch (Exception e)
		{
			Console.WriteLine($"Failed to serialize replay: {e.Message}");
			return;
		}
	}
	else
	{
		Console.WriteLine("Invalid mode!");
	}
}