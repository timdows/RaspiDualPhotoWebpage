using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;

namespace RaspiDualPhotoWebpage.Controllers
{
	[Route("api/[controller]/[action]")]
	[ApiController]
	public class ControlController : ControllerBase
	{
		private readonly AppSettings _appSettings;
		private readonly CountdownTimer _countdownTimer;

		public ControlController(IOptions<AppSettings> appSettings, CountdownTimer countdownTimer)
		{
			_appSettings = appSettings.Value;
			_countdownTimer = countdownTimer;
		}

		[HttpGet]
		public IActionResult GetCountdown()
		{
			var displayOnTimer = _appSettings.DisplayOnTimer;
			var secondsRunning = Convert.ToInt32((DateTime.Now - _countdownTimer.DateTimeStarted).TotalSeconds);

			var secondsLeft = displayOnTimer - secondsRunning;

			if (secondsLeft <= 0)
			{
				var output = "vcgencmd display_power 0".Bash();
			}

			return Ok(secondsLeft);
		}

		[HttpGet]
		public IActionResult DisplayOnTimer()
		{
			var output = "vcgencmd display_power 1".Bash();

			_countdownTimer.DateTimeStarted = DateTime.Now;

			return Ok("Executed");
		}
	}
}