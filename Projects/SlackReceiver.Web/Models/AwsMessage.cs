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
	}
}
