using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Skidor4
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            //routes.RouteExistingFiles = true;
            routes.MapRoute(
             "Default",
             "{controller}/{action}/{id}",
             new { area = "Test", controller = "Home", action = "Index", id = UrlParameter.Optional },
             new[] { "Skidor4.Controllers" }
             ).DataTokens.Add("area", "Test");

            //    routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //        routes.MapRoute(
            //        name: "Default",
            //        url: "{controller}/{action}/{id}",
            //        defaults: new { controller = "Shared", action = "_Layout", id = UrlParameter.Optional }
            //    );
        }
    }
}
