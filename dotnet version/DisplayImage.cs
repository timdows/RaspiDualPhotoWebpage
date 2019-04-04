using System;

namespace RaspiDualPhotoWebpage
{
	public class DisplayImage
	{
		public string FilePath { get; set; }
		public string Url => $"api/images/getfile?file={Uri.EscapeDataString(DirectoryName)}/{Uri.EscapeDataString(FileName)}";
		public string DirectoryName { get; set; }
		public string FileName { get; set; }

		public bool IsResized { get; set; }
		public string ResizedFilePath { get; set; }
		//public string ThumbnailFilePath { get; set; }
	}
}
