﻿namespace HolmesMVC.Controllers
{
    using System.Data;
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
        // GET: /Adaptation/Details/5

        [AllowAnonymous]
        public ActionResult Details(int id = 0)
        {
            Adaptation adaptation = Db.Adaptations.Find(id);
            if (adaptation == null)
            {
                return HttpNotFound();
            }
            if (adaptation.Name == "Canon")
            {
                return RedirectToAction("Index", "Canon");
            }
            var viewmodel = new AdaptView(adaptation);

            return viewmodel.SingleFilm ? View("SingleFilmDetails", viewmodel) : View(viewmodel);
        }

        //
        // GET: /Adaptation/Create

        public ActionResult Create()
        {
            ViewBag.MediumID = new SelectList(Db.Media, "ID", "Name");
            return View();
        }

        //
        // POST: /Adaptation/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Adaptation adaptation)
        {
            if (ModelState.IsValid)
            {
                Db.Adaptations.Add(adaptation);
                Db.SaveChanges(); Shared.SomethingChanged(HttpContext.Application);

                return RedirectToAction("Details","Adaptation", new {id = adaptation.ID});
            }

            ViewBag.Medium = new SelectList(Db.Media, "ID", "Name", adaptation.MediumID);
            return View(adaptation);
        }

        //
        // GET: /Adaptation/Edit/5

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
        // POST: /Adaptation/Edit/5

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
        // GET: /Adaptation/Delete/5

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
        // POST: /Adaptation/Delete/5

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