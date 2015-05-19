using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Http;
using Ionic.Zip;
using TubeToTune.Helpers;
using TubeToTune.Models;
using YoutubeExtractor;

namespace TubeToTune.Controllers
{
	public class ConvertController : ApiController
	{
		//TODO: This all needs t be cleaned out because right now it is looking messy
		[HttpPost]
		public string ConvertTubeToTune([FromBody] List<YouTubeVideoLink> youtubeVideoLinks)
		{
			if (!youtubeVideoLinks.Any()) throw new AudioExtractionException("Please enter a YouTube link.");

			List<string> convertedAudioFilenames = new List<string>();

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

					var convertedAudioFilename = FileNameGenerationHelper.RemoveIllegalPathCharacters(video.Title) + video.AudioExtension;
					var temporaryPath = Path.Combine(HttpContext.Current.Server.MapPath("~/App_Data"), convertedAudioFilename);

					convertedAudioFilenames.Add(temporaryPath);

					var audioDownloader = new AudioDownloader(video, temporaryPath);

					audioDownloader.Execute();
				}
			}
			catch (Exception e)
			{
				throw new AudioExtractionException(e.Message);
			}

			return FileZippingHelper.ZipConvertedAudioFiles(convertedAudioFilenames);
		}
	}
}
