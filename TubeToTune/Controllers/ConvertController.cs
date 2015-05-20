using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Http;
using TubeToTune.Helpers;
using YoutubeExtractor;

namespace TubeToTune.Controllers
{
	public class ConvertController : ApiController
	{
		//TODO: This all needs t be cleaned out because right now it is looking messy
		[HttpPost]
		public string ConvertTubeToTune([FromBody] List<string> youtubeVideoLinks)
		{
			if (!youtubeVideoLinks.Any()) throw new AudioExtractionException("Please enter a YouTube link.");

			List<string> convertedAudioFilenames = new List<string>();

			try
			{
				foreach (string youtubeVideoLink in youtubeVideoLinks)
				{
					IEnumerable<VideoInfo> videoInfos = DownloadUrlResolver.GetDownloadUrls(youtubeVideoLink, false);

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
