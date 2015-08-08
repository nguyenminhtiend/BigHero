using System.Web.Mvc;
using HvN.BigHero.DAL.MyException;

namespace HvN.BigHero.Web.Controllers
{
    public class BaseController : Controller
    {
        protected override void OnException(ExceptionContext filterContext)
        {
            filterContext.ExceptionHandled = true;
            if (filterContext.Exception is NotFoundException)
            {
                filterContext.Result = new RedirectResult("~/Error/NotFound");
            }
            //else if (filterContext.Exception is InternalServerException)
            //{
            //    filterContext.Result = new RedirectResult("~/Error/ServerError");
            //}
            //else
            //{
            //    filterContext.Result = new RedirectResult("~/Error/ServerError");
            //}
        }
    }
}