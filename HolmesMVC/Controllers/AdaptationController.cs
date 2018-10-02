namespace HolmesMVC.Controllers
{
    using System.Data;
    using System.Data.Entity;
    using System.Linq;
    using System.Web.Mvc;

    using HolmesMVC.Models;
    using HolmesMVC.Models.ViewModels;

    public class AdaptationController : HolmesDbController
    {
        [AllowAnonymous]
        public ActionResult Index()
        {
            AdaptListView model = new AdaptListView(Db);

            return View(model);
        }

        //
        // GET: /film/without_a_clue

        [AllowAnonymous]
        public ActionResult SingleFilmDetails(string urlName = "")
        {
            Adaptation adaptation = Db.Adaptations.Where(a => a.UrlName == urlName).FirstOrDefault();
            if (adaptation == null)
            {
                return RedirectToAction("Details", "Adaptation", new { urlName });
            }

            var viewmodel = new AdaptView(adaptation);

            if (!viewmodel.SingleFilm)
            {
                return RedirectToAction("Details", "Adaptation", new { viewmodel.UrlName });
            }
            return View(viewmodel);
        }

        //
        // GET: /tv/granada

        [AllowAnonymous]
        public ActionResult TVDetails(string urlName = "")
        {
            Adaptation adaptation = Db.Adaptations.Where(a => a.UrlName == urlName).FirstOrDefault();
            if (adaptation == null)
            {
                return RedirectToAction("Details", "Adaptation", new { urlName });
            }

            var viewmodel = new AdaptView(adaptation);

            if (viewmodel.MediumName != "Television")
            {
                return RedirectToAction("Details", "Adaptation", new { viewmodel.UrlName });
            }
            return View("Details", viewmodel);
        }

        //
        // GET: /adaptation/house_md

        [AllowAnonymous]
        public ActionResult Details(string urlName = "")
        {
            if (string.IsNullOrEmpty(urlName))
            {
                return HttpNotFound();
            }
            if (urlName == "canon")
            {
                return RedirectToAction("Index", "Canon");
            }

            int id = 0;
            if (int.TryParse(urlName, out id))
            {
                var adaptationById = Db.Adaptations.Find(id);
                if (adaptationById != null)
                {
                    return RedirectToAction("Details", "Adaptation", new { adaptationById.UrlName });
                }
            }

            Adaptation adaptation = Db.Adaptations.Where(a => a.UrlName == urlName).FirstOrDefault();
            if (adaptation == null)
            {
                return HttpNotFound();
            }

            var viewmodel = new AdaptView(adaptation);            
            if (viewmodel.SingleFilm)
            {
                return RedirectToAction("SingleFilmDetails", "Adaptation", new { viewmodel.UrlName });
            }
            return View(viewmodel);
        }

        //
        // GET: /adaptation/create

        public ActionResult Create()
        {
            ViewBag.MediumID = new SelectList(Db.Media, "ID", "Name");
            return View();
        }

        //
        // POST: /adaptation/create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Adaptation adaptation)
        {
            if (ModelState.IsValid)
            {
                Db.Adaptations.Add(adaptation);
                Db.SaveChanges(); Shared.SomethingChanged(HttpContext.Application);

                return RedirectToAction("Details", "Adaptation", new { adaptation.UrlName });
            }

            ViewBag.Medium = new SelectList(Db.Media, "ID", "Name", adaptation.MediumID);
            return View(adaptation);
        }

        //
        // GET: /adaptation/edit/5

        public ActionResult Edit(int id = 0)
        {
            Adaptation adaptation = Db.Adaptations.Find(id);
            if (adaptation == null)
            {
                return HttpNotFound();
            }
            ViewBag.MediumID = new SelectList(Db.Media, "ID", "Name", adaptation.MediumID);
            return View(adaptation);
        }

        //
        // POST: /adaptation/edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Adaptation adaptation)
        {
            if (ModelState.IsValid)
            {
                Db.Entry(adaptation).State = EntityState.Modified;
                Db.SaveChanges(); Shared.SomethingChanged(HttpContext.Application);

                return RedirectToAction("Details", new { adaptation.ID });
            }
            ViewBag.MediumID = new SelectList(Db.Media, "ID", "Name", adaptation.MediumID);
            return View(adaptation);
        }

        //
        // GET: /adaptation/delete/5

        public ActionResult Delete(int id = 0)
        {
            Adaptation adaptation = Db.Adaptations.Find(id);
            if (adaptation == null)
            {
                return HttpNotFound();
            }
            if (adaptation.Seasons.Any())
            {
                return View("CantDelete", adaptation);
            }
            return View(adaptation);
        }

        //
        // POST: /adaptation/delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Adaptation adaptation = Db.Adaptations.Find(id);
            Db.Adaptations.Remove(adaptation);
            Db.SaveChanges(); Shared.SomethingChanged(HttpContext.Application);

            return RedirectToAction("Index","Home");
        }
    }
}