namespace HolmesMVC
{
    using System.Web.Mvc;
    using System.Web.Routing;

    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

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
                "Story/StoryXml",
                new { controller = "Story", action = "StoryXml" }
            );

            // stories, and chunks
            routes.MapRoute(
                "Stories",
                "Story/{id}",
                new { controller = "Story", action = "Details" },
                new { id = @"\w+" }
            );

            // backwards compatibility, for old chunk links
            routes.MapRoute(
                "Chunks",
                "Chunk/{id}",
                new { controller = "Story", action = "Details" },
                new { id = @"\w+" }
            );

            // Search route - everything but Search/OuterSearch
            routes.MapRoute(
                "Search",
                "Search/{id}",
                new { controller = "Search", action = "Index" },
                new { id = "(?!OuterSearch|DeserialiseSearchStringForPartial).*" }
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