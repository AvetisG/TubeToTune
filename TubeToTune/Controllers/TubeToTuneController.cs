using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Http;
using YoutubeExtractor;

namespace TubeToTune.Controllers
{
	public class TubeToTuneController : ApiController
	{
		[HttpPost]
		public string ConvertTubeToTune([FromBody] YouTubeLink youTubeVideoLink)
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
			var audioDownloader = new AudioDownloader(video, Path.Combine("C:/Users/ghukasyana/Documents/Downloads", "DownloadedVideo" + video.AudioExtension));

			audioDownloader.DownloadProgressChanged += (sender, args) => Console.WriteLine(args.ProgressPercentage*0.85);
			audioDownloader.AudioExtractionProgressChanged +=
				(sender, args) => Console.WriteLine(85 + args.ProgressPercentage*0.15);

			audioDownloader.Execute();

			return "Successfully downloaded the video " + youTubeVideoLink.link;
		}

		public class YouTubeLink
		{
			public string link { get; set; }
		}
	}
}
