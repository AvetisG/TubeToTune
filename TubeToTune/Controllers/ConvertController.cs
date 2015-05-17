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

			var convertedAudioFilenames = new List<string>();

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

				var zip = new ZipFile(Path.Combine(HttpContext.Current.Server.MapPath("~/App_Data"), "ZipedFile.zip"));
				zip.AddFiles(convertedAudioFilenames);
				zip.Save(); 
				zip.Dispose();
			}
			catch (Exception e)
			{
				throw new AudioExtractionException(e.Message);
			}

			return "ZipedFile.zip";
		}

		private static string RemoveIllegalPathCharacters(string path)
		{
			string regexSearch = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());
			var r = new Regex(string.Format("[{0}]", Regex.Escape(regexSearch)));
			return r.Replace(path, "").Replace("&", "and");
		}

	}
}
