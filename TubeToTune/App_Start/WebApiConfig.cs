using System.Web.Http;

namespace TubeToTune.App_Start
{
	public static class WebApiConfig
	{
		public static void Register(HttpConfiguration config)
		{
			config.Routes.MapHttpRoute(
				name: "DefaultApi",
				routeTemplate: "api/{controller}/{convertedAudioFilename}",
				defaults: new { convertedAudioFilename = RouteParameter.Optional}
			);
		}
	}
}
