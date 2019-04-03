using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Drawing.Imaging;

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
			var displayImages = new List<DisplayImage>();

			var filePaths = Directory.GetFiles(_appSettings.ImageLocationPath, "*.*", SearchOption.AllDirectories);
			foreach (var filePath in filePaths)
			{
				filePath.Replace(_appSettings.ImageLocationPath, string.Empty);
				displayImages.Add(new DisplayImage
				{
					DirectoryName = Path.GetFileName(Path.GetDirectoryName(filePath)),
					FileName = Path.GetFileName(filePath)
				});
			}

			displayImages.Shuffle();

			return Ok(displayImages);
		}

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
		
		public static string ScaleImage(string imagePath, string saveDirectory, int maxSize)
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

		public static string ResizeImage(string imagePath, string saveDirectory, int maxSize)
		{
			var fileName = Path.GetFileName(imagePath);
			var imageDirectoryName = Path.GetFileName(Path.GetDirectoryName(imagePath));
			var saveDirectoryWithSubdir = Path.Combine(saveDirectory, imageDirectoryName);
			Directory.CreateDirectory(saveDirectoryWithSubdir);

			var savePath = Path.Combine(saveDirectoryWithSubdir, fileName);

			var command = $"convert '{imagePath}' -resize {maxSize} '{savePath}'";
			var output = command.Bash();

			return savePath;
		}

	}

	public class DisplayImage
	{
		public string Url => $"api/images/getfile?file={Uri.EscapeDataString(DirectoryName)}/{Uri.EscapeDataString(FileName)}";
		public string DirectoryName { get; set; }
		public string FileName { get; set; }
	}
}