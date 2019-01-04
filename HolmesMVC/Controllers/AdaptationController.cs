namespace HolmesMVC.Controllers
{
    using System;
    using System.Data;
    using System.Data.Entity;
    using System.Linq;
    using System.Web.Mvc;
    using HolmesMVC.Enums;
    using HolmesMVC.Models;
    using HolmesMVC.Models.ViewModels;

    [OutputCache(Duration = 2628000, VaryByCustom = "LastDbUpdate")]
    public class AdaptationController : HolmesDbController
    {
        [AllowAnonymous]
        public ActionResult Index()
        {
            AdaptListView model = new AdaptListView(Db);

            return View(model);
        }

        //
        // GET: /radio/merrison -- multi
        // GET: /radio/hardwicke -- single

        [AllowAnonymous]
        public ActionResult RadioDetails(string urlName = "")
        {
            Adaptation adaptation = Db.Adaptations.Where(a => a.UrlName == urlName).FirstOrDefault();
            if (adaptation == null || adaptation.Medium != (int)Medium.Radio)
            {
                return RedirectToAction("Details", "Adaptation", new { urlName });
            }

            var viewmodel = new AdaptView(adaptation);

            if (viewmodel.SingleEpisode)
            {
                return View("SingleEpisodeDetails", viewmodel);
            }
            return View("Details", viewmodel);
        }

        //
        // GET: /film/early_rathbone -- multi
        // GET: /film/without_a_clue -- single

        [AllowAnonymous]
        public ActionResult FilmDetails(string urlName = "")
        {
            Adaptation adaptation = Db.Adaptations.Where(a => a.UrlName == urlName).FirstOrDefault();
            if (adaptation == null || adaptation.Medium != (int)Medium.Film)
            {
                return RedirectToAction("Details", "Adaptation", new { urlName });
            }

            var viewmodel = new AdaptView(adaptation);

            if (viewmodel.SingleEpisode)
            {
                return View("SingleEpisodeDetails", viewmodel);
            }
            return View("Details", viewmodel);
        }

        //
        // GET: /tv/granada -- multi
        // GET: /tv/higgins -- single

        [AllowAnonymous]
        public ActionResult TVDetails(string urlName = "")
        {
            Adaptation adaptation = Db.Adaptations.Where(a => a.UrlName == urlName).FirstOrDefault();
            if (adaptation == null || adaptation.Medium != (int)Medium.Television)
            {
                return RedirectToAction("Details", "Adaptation", new { urlName });
            }

            var viewmodel = new AdaptView(adaptation);

            if (viewmodel.SingleEpisode)
            {
                return View("SingleEpisodeDetails", viewmodel);
            }
            return View("Details", viewmodel);
        }


        //
        // GET: /adaptation/wontner

        [AllowAnonymous]
        public ActionResult Details(string urlName = "")
        {
            if (string.IsNullOrEmpty(urlName))
            {
                return HttpNotFound();
            }
            if (urlName == "canon")
            {
                return RedirectToActionPermanent("Index", "Canon");
            }

            int id = 0;
            if (int.TryParse(urlName, out id))
            {
                var adaptationById = Db.Adaptations.Find(id);
                if (adaptationById != null)
                {
                    return RedirectToActionPermanent("Details", "Adaptation", new { adaptationById.UrlName });
                }
            }

            Adaptation adaptation = Db.Adaptations.Where(a => a.UrlName == urlName).FirstOrDefault();
            if (adaptation == null)
            {
                return HttpNotFound();
            }

            var actionName = "Details";
            switch (adaptation.MediumUrlName)
            {
                case "radio":
                    actionName = "RadioDetails";
                    break;
                case "film":
                    actionName = "FilmDetails";
                    break;
                case "tv":
                    actionName = "TVDetails";
                    break;
            }
            if (actionName != "Details")
            {
                return RedirectToActionPermanent(actionName, "Adaptation", new { urlName });
            }
            
            var viewmodel = new AdaptView(adaptation);
            return View(viewmodel);
        }

        //
        // GET: /adaptation/create

        public ActionResult Create()
        {
            ViewBag.Medium = new SelectList(
                Enum.GetNames(typeof(Medium))
                    .Select(m => new
                    {
                        ID = (int)Enum.Parse(typeof(Medium), m),
                        Name = m
                    })
                , "ID", "Name", string.Empty);
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
                Db.SaveChanges();

                return RedirectToAction("Details", "Adaptation", new { adaptation.UrlName });
            }

            ViewBag.Medium = new SelectList(
                Enum.GetNames(typeof(Medium))
                    .Select(m => new
                    {
                        ID = (int)Enum.Parse(typeof(Medium), m),
                        Name = m
                    })
                , "ID", "Name", adaptation.Medium);
            return View(adaptation);
        }

        //
        // GET: /adaptation/edit/5
        [OutputCache(NoStore = true, Duration = 0)]
        public ActionResult Edit(int id = 0)
        {
            Adaptation adaptation = Db.Adaptations.Find(id);
            if (adaptation == null)
            {
                return HttpNotFound();
            }
            ViewBag.Medium = new SelectList(
                Enum.GetNames(typeof(Medium))
                    .Select(m => new
                    {
                        ID = (int)Enum.Parse(typeof(Medium), m),
                        Name = m
                    })
                , "ID", "Name", adaptation.Medium);
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
                Db.SaveChanges();

                return RedirectToAction("Details", new { adaptation.ID });
            }
            ViewBag.Medium = new SelectList(                
                Enum.GetNames(typeof(Medium))
                    .Select(m => new
                    {
                        ID = (int)Enum.Parse(typeof(Medium), m),
                        Name = m
                    })                
                , "ID", "Name", adaptation.Medium);
            return View(adaptation);
        }

        //
        // GET: /adaptation/delete/5
        [OutputCache(NoStore = true, Duration = 0)]
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
            Db.SaveChanges();

            return RedirectToAction("Index","Home");
        }
    }
}