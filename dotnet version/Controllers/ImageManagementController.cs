using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Collections.Generic;

namespace RaspiDualPhotoWebpage.Controllers
{
	[Route("api/[controller]/[action]")]
	[ApiController]
	public class ImageManagementController : ControllerBase
	{
		private readonly AppSettings _appSettings;

		public ImageManagementController(IOptions<AppSettings> appSettings)
		{
			_appSettings = appSettings.Value;
		}

		[HttpGet]
		[ProducesResponseType(typeof(List<DisplayImage>), 200)]
		[ProducesResponseType(500)]
		public IActionResult GetDisplayImageDetails()
		{
			var displayImages = Helpers.GetDisplayImages(_appSettings, false);
			return Ok(displayImages);
		}

		[HttpPost]
		[ProducesResponseType(typeof(DisplayImage), 200)]
		[ProducesResponseType(500)]
		public IActionResult ResizeImage([FromBody] DisplayImage displayImage)
		{
			displayImage.ResizedFilePath = Helpers.ScaleImage(_appSettings, displayImage.FilePath, 800);
			displayImage.IsResized = true;

			return Ok(displayImage);
		}
	}
}