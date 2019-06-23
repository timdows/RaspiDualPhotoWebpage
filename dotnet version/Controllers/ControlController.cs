using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Runtime.InteropServices;

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
		public IActionResult GetControlDetails()
		{
			var secondsLeft = GetSecondsLeft();
			var secondsLeftForDisplay = TimeSpan.FromSeconds(secondsLeft).ToString("mm\\:ss");
			var displayFullScreen = false;

			return Ok(new
			{
				secondsLeft,
				secondsLeftForDisplay,
				displayFullScreen
			});
		}

		private int GetSecondsLeft()
		{
			var displayOnTimer = _appSettings.DisplayOnTimer;
			var secondsRunning = Convert.ToInt32((DateTime.Now - _countdownTimer.DateTimeStarted).TotalSeconds);

			var secondsLeft = displayOnTimer - secondsRunning;

			var isLinux = RuntimeInformation.IsOSPlatform(OSPlatform.Linux);
			if (secondsLeft <= 0 && isLinux)
			{
				var output = "vcgencmd display_power 0".Bash();
			}

			return secondsLeft;
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