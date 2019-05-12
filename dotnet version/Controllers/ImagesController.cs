using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace RaspiDualPhotoWebpage.Controllers
{
	[Route("api/[controller]/[action]")]
	[ApiController]
	public class ImagesController : ControllerBase
	{
		private readonly AppSettings _appSettings;
		private readonly DisplayImagesService _displayImagesService;

		public ImagesController(IOptions<AppSettings> appSettings, DisplayImagesService displayImagesService)
		{
			_appSettings = appSettings.Value;
			_displayImagesService = displayImagesService;
		}

		[HttpGet]
		public IActionResult GetImages()
		{
			List<DisplayImage> displayImages = null;
			if (_displayImagesService.DisplayImages != null)
			{
				displayImages = _displayImagesService.DisplayImages;
			}
			else
			{
				displayImages = Helpers.GetDisplayImages(_appSettings, false);
				_displayImagesService.DisplayImages = displayImages;
			}

			displayImages = displayImages.Where(item => item.IsResized).ToList();
			displayImages.Shuffle();

			return Ok(displayImages);
		}

		[HttpGet]
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

		[HttpGet]
		[ResponseCache(NoStore = true, Duration = 0)]
		public IActionResult GetThumbnail([FromQuery]string file)
		{
			var resizedPath = Path.Combine(_appSettings.ThumbnailImagesPath, file);
			if (System.IO.File.Exists(resizedPath))
			{
				return PhysicalFile(resizedPath, "image/jpeg");
			}

			return BadRequest("Resized image does not exist");
		}
	}
}