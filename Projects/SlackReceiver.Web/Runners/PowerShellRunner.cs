using System.Collections.Generic;
using System.IO;
using System.Management.Automation;
using System.Text;
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
				var script = File.ReadAllText($"{GlobalAppSettings.BaseBotDirectory}\\{context.BotName}\\{context.Intent}.ps1");

				var results = ps
					.AddScript(script).AddParameters(
						new Dictionary<string, string> {{"user", context.CallingUser}, {"token", context.BotToken}})
					.Invoke();

				var sb = new StringBuilder();
				foreach (var result in results)
				{
					sb.AppendLine(result.ToString());
				}

				return sb.ToString();
			}
		}
	}
}
