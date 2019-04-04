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

			var originalPath = Path.Combine(_appSettings.ImageLocationPath, file);
			if (!System.IO.File.Exists(originalPath))
			{
				return BadRequest("Image does not exist");
			}

			//var path = ScaleImage(originalPath, _appSettings.ResizedImagesPath, _appSettings.MaxImageSize);
			var path = ResizeImage(originalPath, _appSettings.ResizedImagesPath, _appSettings.MaxImageSize);

			return PhysicalFile(path, "image/jpeg");
		}
		
		private static string ScaleImage(string imagePath, string saveDirectory, int maxSize)
		{
			var fileName = Path.GetFileName(imagePath);
			var imageDirectoryName = Path.GetFileName(Path.GetDirectoryName(imagePath));
			var saveDirectoryWithSubdir = Path.Combine(saveDirectory, imageDirectoryName);
			Directory.CreateDirectory(saveDirectoryWithSubdir);

			var savePath = Path.Combine(saveDirectoryWithSubdir, fileName);

			using (Image<Rgba32> image = Image.Load(imagePath))
			{
				var ratioX = (double)maxSize / image.Width;
				var ratioY = (double)maxSize / image.Height;
				var ratio = Math.Min(ratioX, ratioY);

				var newWidth = (int)(image.Width * ratio);
				var newHeight = (int)(image.Height * ratio);

				image.Mutate(x => x
					.Resize(newWidth, newHeight)
					.AutoOrient());

				using (var outputStream = new FileStream(savePath, FileMode.CreateNew))
				{
					image.SaveAsJpeg(outputStream);
				}
			}

			return savePath;
		}

		private static string ResizeImage(string imagePath, string saveDirectory, int maxSize)
		{
			var savePath = Helpers.GetSaveResizePath(imagePath, saveDirectory);

			var command = $"convert '{imagePath}' -resize {maxSize} '{savePath}'";
			var output = command.Bash();

			return savePath;
		}

	}
}