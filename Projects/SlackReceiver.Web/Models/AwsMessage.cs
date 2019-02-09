using System;
using System.Collections.Generic;

namespace SlackReceiver.Web.Models
{
	public class AwsMessage
	{
		public string messageVersion { get; set; }
		public string invocationSource { get; set; }
		public string userId { get; set; }
		public string sessionAttributes { get; set; }
		public Dictionary<string, string> requestAttributes { get; set; }
		public SlackBot bot { get; set; }
		public string outputDialogMode { get; set; }
		public AwsIntent currentIntent { get; set; }
		public string inputTranscript { get; set; }

		public BotContext CreateContext(string botType)
		{
			var token = string.Empty;
			var team = string.Empty;
			var user = string.Empty;

			switch (botType.ToLower())
			{
				case "slack":
					token = requestAttributes["x-amz-lex:slack-bot-token"];
					team = requestAttributes["x-amz-lex:slack-team-id"];
					user = userId.Split(':', StringSplitOptions.None)[2];
					break;
			}

			return new BotContext
			{
				BotName = bot.name,
				BotToken = token,
				Intent = currentIntent.name,
				CallingTeam = team,
				CallingUser = user
			};
		}
	}
}