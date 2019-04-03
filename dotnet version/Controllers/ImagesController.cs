using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

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
					DirecotryName = Path.GetFileName(Path.GetDirectoryName(filePath)),
					FileName = Path.GetFileName(filePath)
				});
			}
			return Ok(displayImages);
		}

		public IActionResult GetFile([FromQuery]string file)
		{
			var path = Path.Combine(_appSettings.ImageLocationPath, file);
			return PhysicalFile(path, "image/jpeg");
		}
	}

	public class DisplayImage
	{
		public string Url => $"api/images/getfile?file={DirecotryName}/{FileName}";
		public string DirecotryName { get; set; }
		public string FileName { get; set; }
	}
}