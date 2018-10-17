namespace HolmesMVC.Controllers
{
    using System.Linq;
    using System.Web.Mvc;
    using HolmesMVC.Models;
    using HolmesMVC.Models.ViewModels;

    [OutputCache(Duration = 86400, VaryByCustom = "LastDbUpdate")]
    public class CanonController : HolmesDbController
    {
        //
        // GET: /Canon/

        [AllowAnonymous]
        public ActionResult Index()
        {
            var adapt =
                (from a in Db.Adaptations where a.IsCanon select a)
                    .FirstOrDefault();
            var profile =
                (from p in Db.UserProfiles
                 where p.UserName == User.Identity.Name
                 select p).FirstOrDefault();

            if (null == profile)
            {
                profile = new UserProfile();
            }

            return View("Index", new CanonView(adapt, profile));
        }

        [AllowAnonymous]
        public ActionResult Search(string query)
        {
            CanonSearchView model = new CanonSearchView(Db, query);

            return View(model);
        }
    }
}
