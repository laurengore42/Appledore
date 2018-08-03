namespace HolmesMVC
{
    using System.Web.Mvc;

    public static class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new AuthorizeAttribute());
            filters.Add(new ClacksOverheadHeaderFilter());
        }
    }

    public sealed class ClacksOverheadHeaderFilter : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            filterContext.HttpContext.Response.Headers.Add("X-Clacks-Overhead", "GNU Terry Pratchett");
        }
    }
}