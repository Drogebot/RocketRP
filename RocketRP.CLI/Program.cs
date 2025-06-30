using CommandLine;
using RocketRP;
using RocketRP.CLI;
using RocketRP.Serializers;
using System.Text;

if (args.Length <= 0)
{
	args = ["--help"];
}

Parser.Default.ParseArguments<Options>(args)
.WithParsed<Options>(opts =>
{
	var replayFileInfo = new FileInfo(opts.ReplayPath);
	opts.ReplayPath = replayFileInfo.FullName;
	opts.OutputPath ??= replayFileInfo?.Directory?.FullName;
	if (opts.OutputPath == null)
	{
		Console.WriteLine("Output path could not be determined!");
		return;
	}

	if (!opts.DirectoryMode)
	{
		ParseReplay(opts.ReplayPath, opts.OutputPath, !opts.Fast, opts.EnforceCRC, opts.PrettyPrint, opts.Mode);
	}
	else
	{
		if (replayFileInfo?.Directory == null)
		{
			Console.WriteLine("Invalid replay path!");
			return;
		}

		var replayFiles = opts.Mode == SerializationMode.Deserialize ? replayFileInfo.Directory.GetFiles("*.replay", SearchOption.TopDirectoryOnly) : replayFileInfo.Directory.GetFiles("*.json", SearchOption.TopDirectoryOnly);
		replayFiles = replayFiles.OrderByDescending(f => f.LastWriteTime).ToArray();

		var threads = new List<Thread>();
		foreach (var replayFile in replayFiles)
		{
			var thread = new Thread(() => ParseReplay(replayFile.FullName, opts.OutputPath, !opts.Fast, opts.EnforceCRC, opts.PrettyPrint, opts.Mode));
			thread.Start();
			threads.Add(thread);

			if (threads.Count >= opts.Threads)
			{
				bool waiting = true;
				while (waiting)
				{
					foreach (var th in threads)
					{
						if (th.ThreadState == ThreadState.Stopped)
						{
							th.Join();
							threads.Remove(th);
							waiting = false;
							break;
						}
					}
				}
			}
		}
	}
});

static void ParseReplay(string replayPath, string outputPath, bool parseNetstream, bool enforceCRC, bool prettyPrint, SerializationMode mode)
{
	var serializer = new ReplayJsonSerializer();

	if (mode == SerializationMode.Deserialize)
	{
		var outputFilePath = Path.Combine(outputPath, Path.GetFileNameWithoutExtension(replayPath) + ".json");
		try
		{
			//if (File.Exists(outputFilePath)) return;
			Console.WriteLine($"Parsing replay: {replayPath}...");
			var replay = Replay.Deserialize(replayPath, parseNetstream, enforceCRC);

			Console.WriteLine($"Converting to JSON...");
			var jsonData = serializer.Serialize(replay, prettyPrint);
			var dirName = Path.GetDirectoryName(outputFilePath);
			if (!string.IsNullOrEmpty(dirName)) Directory.CreateDirectory(dirName);
			File.WriteAllText(outputFilePath, jsonData);

			Console.WriteLine($"Parsed replay: {replayPath}: {outputFilePath}!");
		}
		catch (Exception e)
		{
			Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine($"Failed to parse replay: {e.Message}");
			//var replayOutputPath = Path.Combine(outputPath + "\\failedReplays", Path.GetFileName(replayPath));
			//Directory.CreateDirectory(Path.GetDirectoryName(replayOutputPath));
			//if (File.Exists(replayOutputPath)) return;
			//File.Copy(replayPath, replayOutputPath, false);
			return;
		}
	}
	else if(mode == SerializationMode.Serialize)
	{
		var outputFilePath = Path.Combine(outputPath, Path.GetFileNameWithoutExtension(replayPath) + "_RocketRP.replay");
		try
		{
			//if (File.Exists(outputFilePath)) return;
			Console.WriteLine($"Parsing JSON: {replayPath}...");
			var jsonData = File.ReadAllText(replayPath);
			var replay = serializer.Deserialize(jsonData);

			Console.WriteLine($"Converting to replay...");
			replay.Serialize(outputFilePath);

			Console.WriteLine($"Converted to replay: {outputFilePath}!");
		}
		catch (Exception e)
		{
			Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine($"Failed to serialize replay: {replayPath}: {e.Message}");
			return;
		}
	}
	else
	{
		Console.WriteLine("Invalid mode!");
	}
}