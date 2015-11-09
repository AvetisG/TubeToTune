using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Http;
using TubeToTune.Dtos;
using TubeToTune.Helpers;
using YoutubeExtractor;

namespace TubeToTune.Controllers
{
	public class ConvertController : ApiController
	{
		[HttpPost]
		public ConversionResponse ConvertTubeToTune(ConversionRequest ConversionRequest)
        {
            if (ConversionRequest == null) throw new AudioExtractionException("Please enter a YouTube link.");

            IList<VideoConversionErrorDetailsDto> conversionErrors = new List<VideoConversionErrorDetailsDto>();
            IList<string> convertedVideoLinks = new List<string>();

            foreach (VideoConversionDetailsDto videoConversionDetail in ConversionRequest.VideoConversionDetails)
            {
                try
                {
                    IEnumerable<VideoInfo> videoInfos = DownloadUrlResolver.GetDownloadUrls(videoConversionDetail.VideoLink, false);

                    VideoInfo video = videoInfos
                        .Where(info => info.CanExtractAudio)
                        .OrderByDescending(info => info.AudioBitrate)
                        .First();

                    string convertedAudioFileName = FileNameGenerationHelper.RemoveIllegalPathCharacters(video.Title) + video.AudioExtension;
                    convertedVideoLinks.Add(convertedAudioFileName);

                    if (video.RequiresDecryption) { DownloadUrlResolver.DecryptDownloadUrl(video); }

                    new AudioDownloader(video, Path.Combine(HttpContext.Current.Server.MapPath("~/App_Data"), convertedAudioFileName)).Execute();
                }
                catch (Exception e) { conversionErrors.Add(BuildVideoConversionError(e.Message, videoConversionDetail.VideoId)); }
            }

            return BuildConversionResponse(convertedVideoLinks, conversionErrors, ConversionRequest.VideoConversionDetails.Count());
        }

        private VideoConversionErrorDetailsDto BuildVideoConversionError(string message, int videoId)
        {
            VideoConversionErrorDetailsDto VideoConversionErrorDetailsDto = new VideoConversionErrorDetailsDto();
            VideoConversionErrorDetailsDto.VideoConversionError = message;
            VideoConversionErrorDetailsDto.VideoId = videoId;
            return VideoConversionErrorDetailsDto;
        }

        private static ConversionResponse BuildConversionResponse(
            IEnumerable<string> convertedAudioFilenames, IEnumerable<VideoConversionErrorDetailsDto> conversionErrors, int totalVideosToBeConverted)
        {
            ConversionResponse ConversionResponse = new ConversionResponse(totalVideosToBeConverted);
            ConversionResponse.ConvertedAudioFilename = convertedAudioFilenames.Count() == 1 ?
                convertedAudioFilenames.FirstOrDefault() :
                FileZippingHelper.ZipConvertedAudioFiles(convertedAudioFilenames);
            ConversionResponse.ErrorMessages.AddRange(conversionErrors);

            return ConversionResponse;
        }
    }
}
