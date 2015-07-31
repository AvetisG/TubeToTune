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

					string convertedAudioFileName = FileNameGenerationHelper.RemoveIllegalPathCharacters(video.Title) + video.AudioExtension;
					convertedAudioFilenames.Add(convertedAudioFileName);

					if (video.RequiresDecryption) { DownloadUrlResolver.DecryptDownloadUrl(video); }

					new AudioDownloader(video, Path.Combine(HttpContext.Current.Server.MapPath("~/App_Data"), convertedAudioFileName)).Execute();
				}
			}
			catch (Exception e)
			{
				throw new AudioExtractionException(e.Message);
			}

			return convertedAudioFilenames.Count() == 1 ? 
				convertedAudioFilenames.FirstOrDefault() : 
				FileZippingHelper.ZipConvertedAudioFiles(convertedAudioFilenames);
		}
	}
}
