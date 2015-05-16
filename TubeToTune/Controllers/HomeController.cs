using System.Web.Mvc;

namespace TubeToTune.Controllers
{
    public class HomeController : Controller
    {
	    public ActionResult Index()
	    {
		    return View();
	    }

		public ActionResult ErrorPage()
		{
			return View();
		}

		public PartialViewResult Conversion()
		{
			return PartialView();
		}
    }
}
