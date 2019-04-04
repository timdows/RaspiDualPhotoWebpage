using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.IO;
using System.Linq;

namespace RaspiDualPhotoWebpage.Controllers
{
	[Route("api/[controller]/[action]")]
	[ApiController]
	public class ImagesController : ControllerBase
	{
		private readonly AppSettings _appSettings;

		public ImagesController(IOptions<AppSettings> appSettings)
		{
			_appSettings = appSettings.Value;
		}

		public IActionResult GetImages()
		{
			var displayImages = Helpers.GetDisplayImages(_appSettings);
			displayImages = displayImages.Where(item => item.IsResized).ToList();
			displayImages.Shuffle();

			return Ok(displayImages);
		}

		[ResponseCache(NoStore = true, Duration = 0)]
		public IActionResult GetFile([FromQuery]string file)
		{
			var resizedPath = Path.Combine(_appSettings.ResizedImagesPath, file);
			if (System.IO.File.Exists(resizedPath))
			{
				return PhysicalFile(resizedPath, "image/jpeg");
			}

			return BadRequest("Resized image does not exist");
		}
	}
}