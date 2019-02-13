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
		private readonly Logger _logger;

		public PowerShellRunner()
		{
			_logger = LogManager.GetCurrentClassLogger();
		}

		public string RunCommand(BotContext context)
		{
			_logger.Info($"Executing bot {context.BotName}, intent {context.Intent} for user {context.CallingUser}");

			using (var ps = PowerShell.Create())
			{
				var path = $"{GlobalAppSettings.BaseBotDirectory}\\{context.BotName}\\";
				var randomFile = $"{path}{Guid.NewGuid()}.txt";

				_logger.Info($"Temp file for output is {randomFile}");

				var scriptFile = $"{path}{context.Intent}.ps1";

				if (!File.Exists(scriptFile))
				{
					_logger.Error($"No script file exists for intent {context.Intent}");
					return "I understood what you wanted to do - unfortunately this integration is not yet ready on WinAutomation.";
				}

				var script = File.ReadAllText($"{path}{context.Intent}.ps1");

				var results = 
					ps
					.AddScript(script).AddParameters(new Dictionary<string, string> {{"user", context.CallingUser}, {"token", context.BotToken}, {"outfile", randomFile}})
					.Invoke();

				_logger.Info($"Will wait on file {randomFile}");

				do
				{
					Thread.Sleep(50);
				} while (!File.Exists(randomFile));

				// Wait a bit in case WA still has the lock.
				while (true)
				{
					Thread.Sleep(100);
					try
					{
						using (new FileStream(randomFile, FileMode.Open, FileAccess.ReadWrite, FileShare.None))
						{
						}

						break;
					}
					catch (Exception)
					{
						_logger.Info($"File {randomFile} still locked, waiting more");
					}
				}

				_logger.Info($"Wait complete on {randomFile}");

				var returned = File.ReadAllText(randomFile, Encoding.UTF8);

				_logger.Info($"Deleting file {randomFile}");

				File.Delete(randomFile);

				var sb = new StringBuilder();
				foreach (var result in results)
				{
					sb.AppendLine(result.ToString());
				}

				_logger.Info($"PS results: {sb}");

				return returned;
			}
		}
	}
}
