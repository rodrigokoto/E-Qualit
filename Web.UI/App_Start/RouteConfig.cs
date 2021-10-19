﻿using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

namespace Web.UI
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("{*x}", new { x = @".*\.asmx(/.*)?" });

            routes.MapRoute(
               name: "Isotec",
               url: "{route}",
               defaults: new { controller = "Isotec", action = "Index", route = "" }
           );

            routes.MapRoute(
                   name: "Default",
                   url: "{controller}/{action}/{id}",
                   defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
               );

            routes.MapRoute(
              name: "Login",
              url: "{rotaDoCliente}",
              defaults: new { controller = "Login", action = "Index", rotaDoCliente = "" });

            //routes.MapRoute(
            //       name: "processo",
            //       url: "processo/{processo}/{modulo}/{controller}/{action}/{id}",
            //       defaults: new { action = "Index", id = UrlParameter.Optional, modulo = 0, processo = 0 }
            //   );

              //routes.MapRoute(
              //       name: "modulo",
              //       url: "modulo/{processo}/{controller}/{action}/{id}",
              //       defaults: new { action = "Index", id = UrlParameter.Optional }
              //   );
        }
    }
}
