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
				var script = File.ReadAllText($"{GlobalAppSettings.BaseBotDirectory}\\{context.BotName}\\{context.Intent}");

				ps.AddScript(script).AddParameter("user", context.CallingUser);
				ps.AddScript(script).AddParameter("token", context.BotToken);

				var results = ps.Invoke();

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
