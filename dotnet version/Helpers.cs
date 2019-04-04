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
	}
}
