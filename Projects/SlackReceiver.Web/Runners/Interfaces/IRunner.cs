using SlackReceiver.Web.Models;

namespace SlackReceiver.Web.Runners.Interfaces
{
	public interface IRunner
	{
		string RunCommand(BotContext context);
	}
}