using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;

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
               name: "Search",
               url: "search",
               defaults: new { controller = "Home", action = "search", id = UrlParameter.Optional }
           );


            routes.MapRoute(
               name: "Service",
               url: "service/{action}/{operation}",
               defaults: new { controller = "Service", action = "Index", operation = string.Empty }
           );

            routes.MapRoute(
               name: "ServiceHelp",
               url: "service/{action}",
               defaults: new { controller = "Service", action = "Index" }
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
            name: "Account",
            url: "account/{action}",
            defaults: new { controller = "Account", action = "Index" }
            );


            routes.MapRoute(
            name: "SecureGameDetail",
            url: "secure/games/{id}",
            defaults: new { controller = "Secure", action = "GameDetail" }
            );


            
            routes.MapRoute(
            name: "Secure",
            url: "secure/{action}/{status}",
            defaults: new { controller = "Secure", action = "Index", status = string.Empty }
            );


            routes.MapRoute(
            name: "Profile",
            url: "{username}/{action}",
            defaults: new { controller = "Profile", action = "Index" }
            );


            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

        }
    }
}