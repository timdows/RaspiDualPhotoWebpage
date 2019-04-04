using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

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

		public IActionResult GetDisplayImageDetails()
		{
			var displayImages = Helpers.GetDisplayImages(_appSettings);

			foreach (var displayImage in displayImages)
			{
				displayImage.IsResized = System.IO.File.Exists(displayImage.ResizedFilePath);
			}

			return Ok(displayImages);
		}
	}
}