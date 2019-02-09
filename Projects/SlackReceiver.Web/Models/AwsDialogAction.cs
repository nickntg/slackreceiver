namespace SlackReceiver.Web.Models
{
	public class AwsDialogAction
	{
		public string type { get; set; }
		public string fulfillmentState { get; set; }
		public AwsBotMessage message { get; set; }
	}

	public class AwsBotMessage
	{
		public string contentType { get; set; }
		public string content { get; set; }
	}

	public class DialogAction
	{
		public AwsDialogAction dialogAction { get; set; }

		public static DialogAction Create(string type, string fulfillmentState, string message)
		{
			return new DialogAction
			{
				dialogAction = new AwsDialogAction
				{
					fulfillmentState = fulfillmentState, type = type,
					message = new AwsBotMessage {contentType = "PlainText", content = message}
				}
			};
		}
	}
}
