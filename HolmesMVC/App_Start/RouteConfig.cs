namespace HolmesMVC
{
    using System.Web.Mvc;
    using System.Web.Routing;

    public static class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.LowercaseUrls = true;
            routes.AppendTrailingSlash = false;

            // catchall for things like 'Actor/1'
            routes.MapRoute(
                "Details",
                "{controller}/{id}",
                new { action = "Details" },
                new { id = @"\d+" }
            );

            // new human-readable URLs
            routes.MapRoute(
                "NewDetails",
            // XML route
            routes.MapRoute(
                "StoryXml",
                "story/storyxml",
                new { controller = "Story", action = "StoryXml" }
            );

            // stories, and chunks
            routes.MapRoute(
                "Stories",
                "story/{id}",
                new { controller = "Story", action = "Details" },
                new { id = @"\w+" }
            );

            // backwards compatibility, for old chunk links
            routes.MapRoute(
                "Chunks",
                "chunk/{id}",
                new { controller = "Story", action = "Details" },
                new { id = @"\w+" }
            );

            // Account controller is exempt from human-readable
            routes.MapRoute(
                "Account",
                "account/{action}",
                new { controller = "Account" }
            );

            // And this one
            routes.MapRoute(
                "Home",
                "home/{action}",
                new { controller = "Home" }
            );

            // new human-readable URLs
            routes.MapRoute(
                "NewDetails",
                "{controller}/{urlName}",
                new { action = "NewDetails" }
            );

            //// Search route
            //routes.MapRoute(
            //    "Search",
            //    "Search/{id}",
            //    new { controller = "Search", action = "Index" },
            //    new { id = "(?!OuterSearch|DeserialiseSearchStringForPartial|GetAccentDictionaryAsJson).*" }
            //);

            // Google verification route
            routes.IgnoreRoute(
                "google0df7ec4a1dcd4db8.html"
            );

            // default route
            routes.MapRoute(
                "Default",
                "{controller}/{action}/{id}",
                new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}