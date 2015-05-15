using System.Web.Mvc;
using System.Web.Routing;

namespace TubeToTune.App_Start
{
	public class RouteConfig
	{
		public static void RegisterRoutes(RouteCollection routes)
		{
			routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

			routes.MapRoute(
				name: "Default",
				url: "{controller}/{action}/{id}",
				defaults: new { controller = "Navigation", action = "Index", id = UrlParameter.Optional }
			);
		}
	}
}