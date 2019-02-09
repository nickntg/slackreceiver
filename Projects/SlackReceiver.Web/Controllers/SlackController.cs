using System;
using Microsoft.AspNetCore.Mvc;
using NLog;
using SlackReceiver.Web.Extensions;
using SlackReceiver.Web.Models;
using SlackReceiver.Web.Runners;
using SlackReceiver.Web.Runners.Interfaces;

namespace SlackReceiver.Web.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class SlackController : ControllerBase
	{
		private readonly Logger _logger;
		private readonly IRunner _runner;

		public SlackController(IRunner runner)
		{
			_logger = LogManager.GetCurrentClassLogger();
			_runner = runner;
		}

		[HttpPost]
		public IActionResult OnPost([FromBody] AwsMessage message)
		{
			_logger.Info(Request.GetBody());

			var context = message.CreateContext("slack");

			var result = RunManager.Create().RunWithTimeout(_runner, context, TimeSpan.FromSeconds(GlobalAppSettings.RunTimeoutInSeconds));

			return Ok(DialogAction.Create("Close", "Fulfilled", result));
		}
	}
}