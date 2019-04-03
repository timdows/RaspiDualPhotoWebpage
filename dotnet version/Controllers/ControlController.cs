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

		public IActionResult GetCountdown()
		{
			var displayOnTimer = _appSettings.DisplayOnTimer;
			var secondsRunning = Convert.ToInt32((DateTime.Now - _countdownTimer.DateTimeStarted).TotalSeconds);

			var secondsLeft = displayOnTimer - secondsRunning;

			return Ok(secondsLeft);
		}
		
		public IActionResult DisplayOnTimer()
		{
			_countdownTimer.DateTimeStarted = DateTime.Now;

			return Ok("Executed");
		}
	}
}