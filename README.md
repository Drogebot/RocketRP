[![NuGet version (RocketRP)](https://img.shields.io/nuget/v/RocketRP.svg?style=flat-square)](https://www.nuget.org/packages/RocketRP/)
[![Donate](https://img.shields.io/badge/Dontate-Paypal-002f85)](https://www.paypal.com/paypalme/Drogings)
# [RocketRP](https://github.com/Drogebot/RocketRP)
A Rocket League Replay parser written in C#. Replay files are converted to an object which can be exported as JSON.

RocketRP can also convert the object or generated JSON file back into a replay file. This functionality hasn't been extensively tested and might not always work.

Supports all Rocket League versions up to at least [v2.60](https://www.rocketleague.com/en/news/rocket-league-patch-notes-v2-60) (2025-10-17). Newer versions will likely also work because the replay format doesn't change often.
RocketRP successfully parses all my 900+ replays dating back to early 2016. If you find a replay that fails to get parsed, please create an issue so I can look into fixing it.

RocketRP can also be used to convert custom training files to and from objects and JSON.

I like working on this project regardless, but if you would like to support me with this project: [![Donate](https://img.shields.io/badge/Dontate-Paypal-002f85)](https://www.paypal.com/paypalme/Drogings)

## Install
You can download [the latest release](https://github.com/Drogebot/RocketRP/releases/latest) or build from source.

If you want to include this in your project, [a NuGet package](https://www.nuget.org/packages/RocketRP) is also available.

## Usage
### Replays
```
USAGE:
 RocketRP.CLI [FLAGS]

FLAGS:
 -r, --replay       Required. Path to the Replay File/Directory
 -o, --output       (Default: ReplayPath) Path to the output Directory
 -d, --directory    (Default: false) Process entire Directory
 -t, --threads      (Default: 10) Number of Threads to run in Directory Mode
 -f, --fast         (Default: false) Skips the Netstream
 -c, --enforce-crc  (Default: false) Fail if CRCs don't match the data
 -p, --pretty       (Default: false) Output JSON pretty-printed
 -m, --mode         (Default: Deserialize) Deserialize to JSON or Serialize from JSON
 --help             Display this help screen.
 --version          Display version information.

EXAMPLES:
 RocketRP.CLI -r example.replay -p              Convert Replay to pretty-printed JSON
 RocketRP.CLI -r "path\to\replay\files" -d -c   Convert all Replays in a directory if they pass the CRC check
 RocketRP.CLI -r example.json -m Serialize      Convert a JSON file back into a Replay
 RocketRP.CLI -r example.replay -f              Convert only the metadata of a Replay to JSON
```
### Trainings
```
USAGE:
 RocketRP.TrainingCLI [FLAGS]

FLAGS:
 -f, --training     Required. Path to the Training File/Directory
 -o, --output       (Default: TrainingPath) Path to the output Directory
 -d, --directory    (Default: false) Process entire Directory
 -t, --threads      (Default: 10) Number of Threads to run in Directory Mode
 -c, --enforce-crc  (Default: false) Fail if CRCs don't match the data
 -p, --pretty       (Default: false) Output JSON pretty-printed
 -m, --mode         (Default: Deserialize) Deserialize to JSON or Serialize from JSON
 --help             Display this help screen.
 --version          Display version information.

EXAMPLES:
 RocketRP.TrainingCLI -f example.tem -p                   Convert Training to pretty-printed JSON
 RocketRP.TrainingCLI -f "path\to\training\files" -d -c   Convert all Trainings in a directory if they pass the CRC check
 RocketRP.TrainingCLI -f example.json -m Serialize        Convert a JSON file back into a Training
```
