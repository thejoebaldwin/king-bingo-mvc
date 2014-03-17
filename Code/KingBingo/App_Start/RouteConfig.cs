using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace KingBingo
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Contact",
                url: "contact/{action}",
                defaults: new { controller = "Contact", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
               name: "Service",
               url: "service/{action}",
               defaults: new { controller = "Service", action = "Index", id = UrlParameter.Optional }
           );

            routes.MapRoute(
              name: "Terms",
              url: "terms",
              defaults: new { controller = "Home", action = "terms", id = UrlParameter.Optional }
            );
        
            routes.MapRoute(
              name: "Privacy",
              url: "privacy",
              defaults: new { controller = "Home", action = "privacy", id = UrlParameter.Optional }
            );


            routes.MapRoute(
             name: "Profile",
             url: "{username}/{action}",
             defaults: new { controller = "Profile", action = "Index"}
         );

           /*
            routes.MapRoute(
               name: "Home",
               url: "",
               defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
           );
            */
            
            
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
            
        }
    }
}