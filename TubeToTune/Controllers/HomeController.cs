using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using YoutubeExtractor;

namespace TubeToTune.Controllers
{
    public class HomeController : Controller
    {
	    public ActionResult Index()
	    {
			//IEnumerable<VideoInfo> videoInfos = DownloadUrlResolver.GetDownloadUrls("https://www.youtube.com/watch?v=9hEWf_YlE30");
	   
			//VideoInfo video = videoInfos
			//	.Where(info => info.CanExtractAudio)
			//	.OrderByDescending(info => info.AudioBitrate)
			//	.First();

				
			//if (video.RequiresDecryption)
			//{
			//	DownloadUrlResolver.DecryptDownloadUrl(video);
			//}

			//var audioDownloader = new AudioDownloader(video, Path.Combine("C:/", video.Title + video.AudioExtension));

			//audioDownloader.DownloadProgressChanged += (sender, args) => Console.WriteLine(args.ProgressPercentage * 0.85);
			//audioDownloader.AudioExtractionProgressChanged += (sender, args) => Console.WriteLine(85 + args.ProgressPercentage * 0.15);

			//audioDownloader.Execute();

		    return View();
	    }
    }
}
