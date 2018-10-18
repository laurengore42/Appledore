namespace HolmesMVC
{
    using System;
    using System.Globalization;
    using System.Web;
    using System.Web.Http;
    using System.Web.Mvc;
    using System.Web.Optimization;
    using System.Web.Routing;

    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AuthConfig.RegisterAuth();

            Application["LastDbUpdate"] = DateTime.UtcNow.ToString(CultureInfo.InvariantCulture);
        }

        public override string GetVaryByCustomString(HttpContext context, string custom)
        {
            switch (custom)
            {
                case "LastDbUpdate":
                    // Every call to Db.SaveChanges() is followed by an update of this value
                    return string.Concat(User.Identity.Name ?? string.Empty, Application["LastDbUpdate"] ?? string.Empty);
            }

            return base.GetVaryByCustomString(context, custom);
        }
    }
}