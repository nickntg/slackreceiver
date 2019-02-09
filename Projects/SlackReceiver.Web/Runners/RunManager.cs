using System;
using System.Threading.Tasks;
using NLog;
using SlackReceiver.Web.Models;
using SlackReceiver.Web.Runners.Interfaces;

namespace SlackReceiver.Web.Runners
{
	public class RunManager
	{
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

		public string RunWithTimeout(IRunner runner, BotContext context, TimeSpan timeout)
		{
			try
			{
				var result = string.Empty;

				var t = new Task(() => { result = runner.RunCommand(context); });
				t.Start();
				t.Wait(timeout);

				if (t.Status == TaskStatus.RanToCompletion)
				{
					return result;
				}

				if (t.Status == TaskStatus.WaitingToRun || t.Status == TaskStatus.Running)
				{
					return "Unfortunately your request is taking longer than expected to fulfill. Please try to submit it again.";
				}

				return "Unfortunately I don't have the faintest clue what happened with your request. Please contact your administrator about this.";
			}
			catch (AggregateException e)
			{
				Logger.Error(e.InnerExceptions[0], $"Task failed, bot {context.BotName}, intent {context.Intent}");
				return "Unfortunately an internal error stopped me from fulfilling your request. Please contact your administrator about this.";
			}
		}

		public static RunManager Create()
		{
			return new RunManager();
		}
	}
}