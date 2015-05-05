using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

			// Enhance the exception handling and make it more robust following the best practices
			try
			{
				IEnumerable<VideoInfo> videoInfos = DownloadUrlResolver.GetDownloadUrls(youTubeVideoLink.link);

				VideoInfo video = videoInfos
					.Where(info => info.CanExtractAudio)
					.OrderByDescending(info => info.AudioBitrate)
					.First();


				if (video.RequiresDecryption)
				{
					DownloadUrlResolver.DecryptDownloadUrl(video);
				}

				// TODO: Obviously this is a placeholder but will be making a dialog box so that the user can choose his/her prefered directory before downloading + the name of the video
				var audioDownloader = new AudioDownloader(video, "DownloadedVideo" + video.AudioExtension);

				audioDownloader.Execute();

			}
			catch (Exception e)
			{				
				throw new AudioExtractionException(e.Message);
			}

			return "Conversion has been completed: " + youTubeVideoLink.link;
		}
	}
}
