using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;

namespace TubeToTune.Controllers
{
    public class DownloadController : ApiController
    {
		[HttpGet]
		public HttpResponseMessage GetConvertedFile(string convertedAudioFilename)
		{
			if (convertedAudioFilename == string.Empty) 
				return new HttpResponseMessage(HttpStatusCode.BadRequest);

			string path = HttpContext.Current.Request.MapPath(Path.Combine(HttpContext.Current.Request.ApplicationPath, "App_Data/" + convertedAudioFilename));
			var result = new HttpResponseMessage(HttpStatusCode.OK);
			var stream = new FileStream(path, FileMode.Open);
			result.Content = new StreamContent(stream);
			result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
			{
				FileName = convertedAudioFilename
			};

			result.Content.Headers.ContentType = new MediaTypeHeaderValue(MimeMapping.GetMimeMapping(path));
			return result;
		}
    }
}
