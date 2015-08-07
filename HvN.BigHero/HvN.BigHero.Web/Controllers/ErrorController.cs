using System.Web.Mvc;

namespace HvN.BigHero.Web.Controllers
{
    public class ErrorController : Controller
    {
        public ActionResult NotFound()
        {
            return View();
        }
	}
}