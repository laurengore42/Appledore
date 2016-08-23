namespace HolmesMVC.Controllers
{
    using System.Linq;
    using System.Web.Mvc;

    using HolmesMVC.Models;
    using HolmesMVC.Models.ViewModels;

    public class CanonController : HolmesDbController
    {
        //
        // GET: /Canon/

        [Stopwatch]
        [AllowAnonymous]
        public ActionResult Index()
        {
            var adapt =
                (from a in Db.Adaptations where a.Name == "Canon" select a)
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

        [Stopwatch]
        [AllowAnonymous]
        public ActionResult Search()
        {
            return View();
        }
    }
}
