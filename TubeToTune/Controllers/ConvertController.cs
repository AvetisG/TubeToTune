using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Http;
using YoutubeExtractor;

namespace TubeToTune.Controllers
{
	public class ConvertController : ApiController
	{
		[HttpPost]
		public string ConvertTubeToTune([FromBody] string youTubeVideoLink)
		{
			if (string.IsNullOrEmpty(youTubeVideoLink)) throw new AudioExtractionException("Please enter a YouTube link.");

			var convertedAudioFilename = String.Empty;

			try
			{
				IEnumerable<VideoInfo> videoInfos = DownloadUrlResolver.GetDownloadUrls(youTubeVideoLink, false);

				VideoInfo video = videoInfos
					.Where(info => info.CanExtractAudio)
					.OrderByDescending(info => info.AudioBitrate)
					.First();

				if (video.RequiresDecryption) { DownloadUrlResolver.DecryptDownloadUrl(video); }

				convertedAudioFilename = RemoveIllegalPathCharacters(video.Title) + video.AudioExtension;

				var temporaryPath = Path.Combine(HttpContext.Current.Server.MapPath("~/App_Data"), convertedAudioFilename);

				var audioDownloader = new AudioDownloader(video, temporaryPath);

				audioDownloader.Execute();
			}
			catch (Exception e)
			{
				throw new AudioExtractionException(e.Message);
			}

			return convertedAudioFilename;
		}

		private static string RemoveIllegalPathCharacters(string path)
		{
			string regexSearch = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());
			var r = new Regex(string.Format("[{0}]", Regex.Escape(regexSearch)));
			return r.Replace(path, "").Replace("&", "and");
		}

	}
}
