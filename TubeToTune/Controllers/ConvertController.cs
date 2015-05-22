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

					var temporaryPath = Path.Combine(
						HttpContext.Current.Server.MapPath("~/App_Data"), 
						FileNameGenerationHelper.RemoveIllegalPathCharacters(video.Title) + video.AudioExtension);

					convertedAudioFilenames.Add(temporaryPath);

					new AudioDownloader(video, temporaryPath).Execute();
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
