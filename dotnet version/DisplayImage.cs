using System;

namespace RaspiDualPhotoWebpage
{
	public class DisplayImage
	{
		public string FilePath { get; set; }
		public string Url => $"api/images/getfile?file={Uri.EscapeDataString(DirectoryName)}/{Uri.EscapeDataString(FileName)}";
		public string ThumbnailUrl => $"api/images/getthumbnail?file={Uri.EscapeDataString(DirectoryName)}/{Uri.EscapeDataString(FileName)}";
		public string DirectoryName { get; set; }
		public string FileName { get; set; }

		public bool IsResized { get; set; }
		public string ResizedFilePath { get; set; }
		public string ThumbnailImagePath { get; set; }

		public bool? BackgroundCover { get; set; }

		// Used in front end
		public bool IsResizing { get; set; }
	}
}
