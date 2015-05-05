using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Http;
using TubeToTune.Models;
using YoutubeExtractor;

namespace TubeToTune.Controllers
{
	public class TubeToTuneController : ApiController
	{
		[HttpPost]
		public string ConvertTubeToTune([FromBody] YouTubeLink youTubeVideoLink)
		{
			if (youTubeVideoLink.link == null) return "Please enter a YouTube link.";

			var temporaryPath = String.Empty;

			try
			{
				IEnumerable<VideoInfo> videoInfos = DownloadUrlResolver.GetDownloadUrls(youTubeVideoLink.link, false);

				VideoInfo video = videoInfos
					.Where(info => info.CanExtractAudio)
					.OrderByDescending(info => info.AudioBitrate)
					.First();

				if (video.RequiresDecryption) { DownloadUrlResolver.DecryptDownloadUrl(video); }

				temporaryPath = Path.Combine(Path.GetTempPath(),
					RemoveIllegalPathCharacters(video.Title) + video.AudioExtension);

				var audioDownloader = new AudioDownloader(video, temporaryPath);

				audioDownloader.Execute();
			}
			catch (Exception e)
			{
				throw new AudioExtractionException(e.Message);
			}

			return temporaryPath;
		}

		private static string RemoveIllegalPathCharacters(string path)
		{
			string regexSearch = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());
			var r = new Regex(string.Format("[{0}]", Regex.Escape(regexSearch)));
			return r.Replace(path, "");
		}

	}
}
