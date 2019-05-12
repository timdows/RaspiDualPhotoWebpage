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

		public static List<DisplayImage> GetDisplayImages(AppSettings appSettings, bool checkBackgroundCover)
		{
			var displayImages = new List<DisplayImage>();

			var filePaths = Directory.GetFiles(appSettings.ImageLocationPath, "*.*", SearchOption.AllDirectories);
			foreach (var filePath in filePaths)
			{
				filePath.Replace(appSettings.ImageLocationPath, string.Empty);
				var displayImage = new DisplayImage
				{
					FilePath = filePath,
					DirectoryName = Path.GetFileName(Path.GetDirectoryName(filePath)),
					FileName = Path.GetFileName(filePath),
					IsResized = File.Exists(GetSaveResizePath(filePath, appSettings.ResizedImagesPath)),
					ResizedFilePath = GetSaveResizePath(filePath, appSettings.ResizedImagesPath)
				};

				if (displayImage.IsResized && checkBackgroundCover)
				{
					displayImage.BackgroundCover = ShouldImageCoverBackground(appSettings, displayImage.ResizedFilePath);
				}

				displayImages.Add(displayImage);
			}

			return displayImages;
		}

		public static bool? ShouldImageCoverBackground(AppSettings appSettings, string filePath)
		{

			// Linux
			if (appSettings.ImageScaler == 2)
			{
				try
				{
					var command = $"identify -format '%w:%h' '{filePath}'";
					var output = command.Bash();
					Console.WriteLine($"{command} {output}");
					var splittedOutput = output.Split(":");
					var width = int.Parse(splittedOutput[0]);
					var height = int.Parse(splittedOutput[1]);

					return width > height;
				}
				catch (Exception excep)
				{
					Console.Error.WriteLine(excep.Message);
				}
			}
			else
			{
				using (Image<Rgba32> image = Image.Load(filePath))
				{
					return image.Width > image.Height;
				}
			}

			return null;
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

		public static string ScaleImage(int imageScaler, string saveDirectory, string imagePath, int maxSize)
		{
			switch (imageScaler)
			{
				case 1:
					return DoScaleImage(imagePath, saveDirectory, maxSize);
				case 2:
					return ResizeImage(imagePath, saveDirectory, maxSize);
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
