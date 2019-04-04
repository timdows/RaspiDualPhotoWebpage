using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.IO;

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

			//var originalPath = Path.Combine(_appSettings.ImageLocationPath, file);
			//if (!System.IO.File.Exists(originalPath))
			//{
				
			//}

			////var path = ScaleImage(originalPath, _appSettings.ResizedImagesPath, _appSettings.MaxImageSize);
			//var path = ResizeImage(originalPath, _appSettings.ResizedImagesPath, _appSettings.MaxImageSize);

			//return PhysicalFile(path, "image/jpeg");
		}
	}
}