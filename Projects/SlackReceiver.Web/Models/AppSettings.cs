namespace SlackReceiver.Web.Models
{
	public class AppSettings
	{
		public int RunTimeoutInSeconds { get; set; }
		public string BaseBotDirectory { get; set; }
	}

	public static class GlobalAppSettings
	{
		public static int RunTimeoutInSeconds { get; set; }
		public static string BaseBotDirectory { get; set; }
	}
}