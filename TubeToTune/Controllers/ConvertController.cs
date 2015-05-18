using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Http;
using Ionic.Zip;
using TubeToTune.Models;
using YoutubeExtractor;

namespace TubeToTune.Controllers
{
	public class ConvertController : ApiController
	{
		[HttpPost]
		public string ConvertTubeToTune([FromBody] List<YouTubeVideoLink> youtubeVideoLinks)
		{
			if (!youtubeVideoLinks.Any()) throw new AudioExtractionException("Please enter a YouTube link.");

			var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
			var random = new Random();
			var result = new string(
				Enumerable.Repeat(chars, 8)
						  .Select(s => s[random.Next(s.Length)])
						  .ToArray());

			var convertedAudioFilenames = new List<string>();
			var zippedFileName = result + ".zip";

			try
			{
				foreach (YouTubeVideoLink youtubeVideoLink in youtubeVideoLinks)
				{
					IEnumerable<VideoInfo> videoInfos = DownloadUrlResolver.GetDownloadUrls(youtubeVideoLink.link, false);

					VideoInfo video = videoInfos
						.Where(info => info.CanExtractAudio)
						.OrderByDescending(info => info.AudioBitrate)
						.First();

					if (video.RequiresDecryption) { DownloadUrlResolver.DecryptDownloadUrl(video); }

					var convertedAudioFilename = RemoveIllegalPathCharacters(video.Title) + video.AudioExtension;
					var temporaryPath = Path.Combine(HttpContext.Current.Server.MapPath("~/App_Data"), convertedAudioFilename);

					convertedAudioFilenames.Add(temporaryPath);

					var audioDownloader = new AudioDownloader(video, temporaryPath);

					audioDownloader.Execute();
				}
				var zip = new ZipFile(Path.Combine(HttpContext.Current.Server.MapPath("~/App_Data"), zippedFileName));
				zip.AddFiles(convertedAudioFilenames, false, "");
				zip.Save(); 
				zip.Dispose();
			}
			catch (Exception e)
			{
				throw new AudioExtractionException(e.Message);
			}

			return zippedFileName;
		}

		private static string RemoveIllegalPathCharacters(string path)
		{
			string regexSearch = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());
			var r = new Regex(string.Format("[{0}]", Regex.Escape(regexSearch)));
			return r.Replace(path, "").Replace("&", "and");
		}

	}
}
