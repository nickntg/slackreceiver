namespace SlackReceiver.Web.Models
{
	public class BotContext
	{
		public string BotName { get; set; }
		public string BotToken { get; set; }
		public string Intent { get; set; }
		public string CallingUser { get; set; }
		public string CallingTeam { get; set; }
	}
}