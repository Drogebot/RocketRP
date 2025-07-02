using CommandLine;
using RocketRP;
using RocketRP.TrainingCLI;
using RocketRP.Serializers;
using System.Text;
using RocketRP.Actors.TAGame;

if (args.Length <= 0)
{
	args = ["--help"];
}

Parser.Default.ParseArguments<Options>(args)
.WithParsed<Options>(opts =>
{
	var trainingFileInfo = new FileInfo(opts.TrainingPath);
	opts.TrainingPath = trainingFileInfo.FullName;
	opts.OutputPath ??= trainingFileInfo?.Directory?.FullName;
	if (opts.OutputPath == null)
	{
		Console.WriteLine("Output path could not be determined!");
		return;
	}

	if (!opts.DirectoryMode)
	{
		ParseTraining(opts.TrainingPath, opts.OutputPath, opts.EnforceCRC, opts.PrettyPrint, opts.Mode);
	}
	else
	{
		if (trainingFileInfo?.Directory == null)
		{
			Console.WriteLine("Invalid training path!");
			return;
		}

		var trainingFiles = opts.Mode == SerializationMode.Deserialize ? trainingFileInfo.Directory.GetFiles("*.tem", SearchOption.TopDirectoryOnly) : trainingFileInfo.Directory.GetFiles("*.json", SearchOption.TopDirectoryOnly);
		trainingFiles = trainingFiles.OrderByDescending(f => f.LastWriteTime).ToArray();

		var threads = new List<Thread>();
		foreach (var trainingFile in trainingFiles)
		{
			var thread = new Thread(() => ParseTraining(trainingFile.FullName, opts.OutputPath, opts.EnforceCRC, opts.PrettyPrint, opts.Mode));
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

static void ParseTraining(string trainingPath, string outputPath, bool enforceCRC, bool prettyPrint, SerializationMode mode)
{
	var serializer = new SaveDataJsonSerializer();

	if (mode == SerializationMode.Deserialize)
	{
		var outputFilePath = Path.Combine(outputPath, Path.GetFileNameWithoutExtension(trainingPath) + ".json");
		try
		{
			//if (File.Exists(outputFilePath)) return;
			Console.WriteLine($"Parsing training: {trainingPath}...");
			var training = SaveData<SaveData_GameEditor_Training_TA>.Deserialize(trainingPath, true, enforceCRC);
			var trainingData = training.Properties.TrainingData?.GetObject(training.Objects);
			if(trainingData is not null)
			{
				Console.WriteLine($"Training Name: {trainingData.TM_Name}");
			}

			Console.WriteLine($"Converting to JSON...");
			var jsonData = serializer.Serialize(training, prettyPrint);
			var dirName = Path.GetDirectoryName(outputFilePath);
			if (!string.IsNullOrEmpty(dirName)) Directory.CreateDirectory(dirName);
			File.WriteAllText(outputFilePath, jsonData);

			Console.WriteLine($"Parsed training: {trainingPath}: {outputFilePath}!");
		}
		catch (Exception e)
		{
			Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine($"Failed to parse training: {e.Message}");
			//var trainingOutputPath = Path.Combine(outputPath + "\\failedTrainings", Path.GetFileName(trainingPath));
			//Directory.CreateDirectory(Path.GetDirectoryName(trainingOutputPath));
			//if (File.Exists(trainingOutputPath)) return;
			//File.Copy(trainingPath, trainingOutputPath, false);
			Console.ForegroundColor = ConsoleColor.Gray;
			return;
		}
	}
	else if (mode == SerializationMode.Serialize)
	{
		var outputFilePath = Path.Combine(outputPath, Path.GetFileNameWithoutExtension(trainingPath) + ".tem");
		try
		{
			//if (File.Exists(outputFilePath)) return;
			Console.WriteLine($"Parsing JSON: {trainingPath}...");
			var jsonData = File.ReadAllText(trainingPath);
			var training = serializer.Deserialize<SaveData_GameEditor_Training_TA>(jsonData);

			Console.WriteLine($"Converting to training...");
			training.Serialize(outputFilePath);

			Console.WriteLine($"Converted to training: {outputFilePath}!");
		}
		catch (Exception e)
		{
			Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine($"Failed to serialize training: {trainingPath}: {e.Message}");
			Console.ForegroundColor = ConsoleColor.Gray;
			return;
		}
	}
	else
	{
		Console.WriteLine("Invalid mode!");
	}
}