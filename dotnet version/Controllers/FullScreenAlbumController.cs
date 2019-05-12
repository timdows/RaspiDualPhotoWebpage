using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace RaspiDualPhotoWebpage.Controllers
{
	[Route("api/[controller]/[action]")]
	[ApiController]
	public class FullScreenAlbumController : ControllerBase
	{
		private readonly AppSettings _appSettings;
		private readonly DisplayImagesService _displayImagesService;

		public FullScreenAlbumController(IOptions<AppSettings> appSettings, DisplayImagesService displayImagesService)
		{
			_appSettings = appSettings.Value;
			_displayImagesService = displayImagesService;
		}

		[HttpGet]
		[ProducesResponseType(typeof(List<AlbumInfo>), 200)]
		[ProducesResponseType(500)]
		public IActionResult GetAlbums()
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

			var grouped = displayImages.GroupBy(item => item.DirectoryName);
			var albumsInfo = new List<AlbumInfo>();

			foreach (var group in grouped)
			{
				var albumInfo = new AlbumInfo
				{
					Name = group.Key,
					ImageCount = group.Count(),
					ThumbnailUrl = group.FirstOrDefault(item => item.IsResized)?.ThumbnailUrl
				};

				albumsInfo.Add(albumInfo);
			}

			return Ok(albumsInfo);
		}
	}

	public class AlbumInfo
	{
		public string Name { get; set; }
		public int ImageCount { get; set; }
		public string ThumbnailUrl { get; set; }
	}
}
