using Microsoft.AspNetCore.Http;

namespace SlackReceiver.Web.Extensions
{
	public static class HttpRequestExtensions
	{
		public static string GetBody(this HttpRequest request)
		{
			var msg = new byte[request.Body.Length];
			request.Body.Position = 0;
			request.Body.Read(msg, 0, (int)request.Body.Length);
			return System.Text.Encoding.UTF8.GetString(msg);
		}
	}
}