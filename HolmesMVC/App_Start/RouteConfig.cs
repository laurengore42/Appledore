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

            // CRUD links
            routes.MapRoute(
                "Create",
                "{controller}/{action}",
                new {},
                new { action = @"create" }
            );
            routes.MapRoute(
                "CreateShort",
                "{controller}/{action}",
                new { },
                new { action = @"createshort" }
            );
            routes.MapRoute(
                "Edit",
                "{controller}/{action}",
                new { },
                new { action = @"edit" }
            );
            routes.MapRoute(
                "Delete",
                "{controller}/{action}",
                new { },
                new { action = @"delete" }
            );
            routes.MapRoute(
                "DeleteConfirmed",
                "{controller}/{action}",
                new { },
                new { action = @"deleteconfirmed" }
            );
            routes.MapRoute(
                "Combine",
                "{controller}/{action}",
                new { },
                new { action = @"combine" }
            );

            // actor human-readable URLs
            routes.MapRoute(
                "ActorNewDetails",
                "actor/{urlName}",
                new { action = "NewDetails" }
            );

            // character human-readable URLs
            routes.MapRoute(
                "CharNewDetails",
                "character/{urlName}",
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