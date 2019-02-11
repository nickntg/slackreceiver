using System;
using System.Collections.Generic;
using System.IO;
using System.Management.Automation;
using System.Text;
using System.Threading;
using NLog;
using SlackReceiver.Web.Models;
using SlackReceiver.Web.Runners.Interfaces;

namespace SlackReceiver.Web.Runners
{
	public class PowerShellRunner : IRunner
	{
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

		public string RunCommand(BotContext context)
		{
			Logger.Info($"Executing bot {context.BotName}, intent {context.Intent} for user {context.CallingUser}");

			using (var ps = PowerShell.Create())
			{
				var path = $"{GlobalAppSettings.BaseBotDirectory}\\{context.BotName}\\";
				var randomFile = $"{path}{Guid.NewGuid()}.txt";

				Logger.Info($"Temp file for output is {randomFile}");

				var script = File.ReadAllText($"{path}{context.Intent}.ps1");

				var results = 
					ps
					.AddScript(script).AddParameters(new Dictionary<string, string> {{"user", context.CallingUser}, {"token", context.BotToken}, {"outfile", randomFile}})
					.Invoke();

				Logger.Info($"Will wait on file {randomFile}");

				do
				{
					Thread.Sleep(50);
				} while (!File.Exists(randomFile));

				// Wait a bit in case WA still has the lock.
				Thread.Sleep(50);

				Logger.Info($"Wait complete on {randomFile}");

				var returned = File.ReadAllText(randomFile, Encoding.UTF8);

				Logger.Info($"Deleting file {randomFile}");

				File.Delete(randomFile);

				var sb = new StringBuilder();
				foreach (var result in results)
				{
					sb.AppendLine(result.ToString());
				}

				Logger.Info($"PS results: {sb}");

				return returned;
			}
		}
	}
}
