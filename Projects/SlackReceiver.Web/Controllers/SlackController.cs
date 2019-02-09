using Microsoft.AspNetCore.Mvc;
using SlackReceiver.Web.Models;

namespace SlackReceiver.Web.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class SlackController : ControllerBase
	{
		[HttpPost]
		public IActionResult OnPost([FromBody] AwsMessage message)
		{
			return Ok(DialogAction.Create("Close", "Fulfilled", "Got you!"));
		}
	}
}