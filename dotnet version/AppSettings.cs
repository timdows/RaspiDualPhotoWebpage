namespace RaspiDualPhotoWebpage
{
	public class AppSettings
	{
		public string ImageLocationPath { get; set; }
		public string ResizedImagesPath { get; set; }
		public string ThumbnailImagesPath { get; set; }
		public int MaxImageSize { get; set; }

		public int DisplayOnTimer { get; set; }

		public int ImageScaler { get; set; }

		public string MagicMirrorUrl { get; set; }
	}
}
