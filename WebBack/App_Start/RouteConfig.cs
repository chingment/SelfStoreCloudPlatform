using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace WebBack
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");


            routes.MapRoute(
         name: "App_Default",
         url: "App/{controller}/{action}/{id}",
         defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
         namespaces:new string[] { "WebBack.Controllers.App" }
      );

            routes.MapRoute(
                   name: "Biz_Default",
                   url: "Biz/{controller}/{action}/{id}",
                   defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                 namespaces: new string[] { "WebBack.Controllers.Biz" }
                   );


            //, namespaces: new string[] { "Lumos.WebSite.Controllers" }
            routes.MapRoute(
               name: "Sys_Default",
               url: "Sys/{controller}/{action}/{id}",
               defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                   namespaces: new string[] { "WebBack.Controllers.Sys" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );



        }
    }
}
