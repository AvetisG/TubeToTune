using System.Web.Mvc;

namespace TubeToTune.Controllers
{
    public class NavigationController : Controller
    {
		public ActionResult Index()
		{
			return View();
		}

		public ActionResult ErrorPage()
		{
			return View();
		}

		public PartialViewResult SingleTune()
	    {
		    return	PartialView();
	    }

		public PartialViewResult MultipleTunes()
		{
			return PartialView();
		}
    }
}
