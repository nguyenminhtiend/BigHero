using System.Web.Mvc;
using System.Web.Routing;

namespace HvN.BigHero.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{tableName}/{rowId}",
                defaults: new { controller = "Table", action = "Create", tableName = UrlParameter.Optional, rowId = UrlParameter.Optional }
            );
        }
    }
}
