namespace HolmesMVC
{
    using System.Web.Mvc;
    using HolmesMVC.Models;

    [OutputCache(Duration = 86400, VaryByCustom = "LastDbUpdate")]
    public class HolmesDbController : Controller
    {
        protected readonly HolmesDBEntities Db = new HolmesDBEntities();
        
        protected override void Dispose(bool disposing)
        {
            Db.Dispose();
            base.Dispose(disposing);
        }
    }
}