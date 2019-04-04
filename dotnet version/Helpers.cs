using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace RaspiDualPhotoWebpage
{
	public static class Helpers
	{
		public static string Bash(this string cmd)
		{
			var escapedArgs = cmd.Replace("\"", "\\\"");

			var process = new Process()
			{
				StartInfo = new ProcessStartInfo
				{
					FileName = "/bin/bash",
					Arguments = $"-c \"{escapedArgs}\"",
					RedirectStandardOutput = true,
					UseShellExecute = false,
					CreateNoWindow = true,
				}
			};

			process.Start();
			string result = process.StandardOutput.ReadToEnd();
			process.WaitForExit();

			return result;
		}

		private static Random rng = new Random();

		public static void Shuffle<T>(this IList<T> list)
		{
			int n = list.Count;
			while (n > 1)
			{
				n--;
				int k = rng.Next(n + 1);
				T value = list[k];
				list[k] = list[n];
				list[n] = value;
			}
		}

		public static List<DisplayImage> GetDisplayImages(AppSettings appSettings)
		{
			var displayImages = new List<DisplayImage>();

			var filePaths = Directory.GetFiles(appSettings.ImageLocationPath, "*.*", SearchOption.AllDirectories);
			foreach (var filePath in filePaths)
			{
				filePath.Replace(appSettings.ImageLocationPath, string.Empty);
				displayImages.Add(new DisplayImage
				{
					FilePath = filePath,
					DirectoryName = Path.GetFileName(Path.GetDirectoryName(filePath)),
					FileName = Path.GetFileName(filePath),
					ResizedFilePath = GetSaveResizePath(filePath, appSettings.ResizedImagesPath)
				});
			}

			return displayImages;
		}

		public static string GetSaveResizePath(string imagePath, string saveDirectory)
		{
			var fileName = Path.GetFileName(imagePath);
			var imageDirectoryName = Path.GetFileName(Path.GetDirectoryName(imagePath));
			var saveDirectoryWithSubdir = Path.Combine(saveDirectory, imageDirectoryName);
			Directory.CreateDirectory(saveDirectoryWithSubdir);

			var savePath = Path.Combine(saveDirectoryWithSubdir, fileName);

			return savePath;
		}

		public static string ScaleImage(AppSettings appSettings, string imagePath, int maxSize)
		{
			switch (appSettings.ImageScaler)
			{
				case 1:
					return DoScaleImage(imagePath, appSettings.ResizedImagesPath, maxSize);
				case 2:
					return ResizeImage(imagePath, appSettings.ResizedImagesPath, maxSize);
				default:
					throw new Exception("Image scaler not found");
			}
		}

		private static string DoScaleImage(string imagePath, string saveDirectory, int maxSize)
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
			var savePath = GetSaveResizePath(imagePath, saveDirectory);

			var command = $"convert '{imagePath}' -resize {maxSize} '{savePath}'";
			var output = command.Bash();

			return savePath;
		}
	}
}
