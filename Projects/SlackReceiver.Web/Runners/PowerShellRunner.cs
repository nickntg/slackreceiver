using System;
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
			System.Threading.Thread.Sleep(100);
			throw new NotImplementedException();
		}
	}
}
