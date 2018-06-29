namespace HolmesMVC.Controllers
{
    using System.Collections.Generic;
    using System.Data;
    using System.Globalization;
    using System.Linq;
    using System.Web.Mvc;
    using HolmesMVC.Models;

    public class SeasonController : HolmesDbController
    {
        //
        // GET: /Season/Details/5

        public ActionResult Details(int id = 0)
        {
            Season season = Db.Seasons.Find(id);
            if (season == null)
            {
                return HttpNotFound();
            }
            return View(season);
        }

        //
        // GET: /Season/Create

        public ActionResult Create(int id = -1)
        {
            if (id == -1)
            {
                return HttpNotFound();
            }

            var adapt = Db.Adaptations.Find(id);
            if (adapt == null)
            {
                return HttpNotFound();
            }

            var existingSeasons = from s in Db.Seasons where s.Adaptation == id select s.AirOrder;
            int newSeasonAirOrder = 1;
            if (existingSeasons.Any())
            {
                newSeasonAirOrder = existingSeasons.Max() + 1;
            }

            var model = new Season();
            model.Adaptation = id;
            model.AirOrder = newSeasonAirOrder;

            ViewBag.AdaptationName = Shared.DisplayName(adapt);

            return View(model);
        }

        //
        // POST: /Season/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Season season)
        {
            if (ModelState.IsValid)
            {
                var id = season.Adaptation;
                Db.Seasons.Add(season);
                Db.SaveChanges(); Shared.SomethingChanged(HttpContext.Application);
                return RedirectToAction("Create", "Episode", new { ID = id });
            }

            ViewBag.Adaptation = GetAdaptList();
            return View(season);
        }

        //
        // GET: /Season/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Season season = Db.Seasons.Find(id);
            if (season == null)
            {
                return HttpNotFound();
            }

            var adapts = (from a in Db.Adaptations select a).ToList();
            var listAdapts = (from a in adapts
                              select new SelectListItem
                              {
                                  Text = a.Name ?? Shared.DisplayName(a),
                                  Value = a.ID.ToString(
                                     CultureInfo.InvariantCulture
                                  )
                              }).ToList();

            ViewBag.Adaptation = new SelectList(listAdapts, "Value", "Text", season.Adaptation);

            return View(season);
        }

        //
        // POST: /Season/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Season season)
        {
            if (ModelState.IsValid)
            {
                Db.Entry(season).State = EntityState.Modified;
                Db.SaveChanges(); Shared.SomethingChanged(HttpContext.Application);
                return RedirectToAction("Details", "Adaptation", new { id = season.Adaptation} );
            }
            ViewBag.Adaptation = GetAdaptList();
            return View(season);
        }

        //
        // GET: /Season/Delete/5

        public ActionResult Delete(int id = 0)
        {
            var season = Db.Seasons.Find(id);
            if (season == null)
            {
                return HttpNotFound();
            }
            if (season.Episodes.Any())
            {
                return View("CantDelete", season);
            }
            return View(season);
        }

        //
        // POST: /Season/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Season season = Db.Seasons.Find(id);
            Db.Seasons.Remove(season);
            Db.SaveChanges(); Shared.SomethingChanged(HttpContext.Application);
            return RedirectToAction("Details", "Adaptation", new { id = season.Adaptation });
        }

        private List<SelectListItem> GetAdaptList()
        {
            var adapts = (from a in Db.Adaptations select a).ToList();
            var adaptsList = (from a in adapts
                              select new SelectListItem
                              {
                                  Value = a.ID.ToString(CultureInfo.InvariantCulture),
                                  Text = Shared.DisplayName(a)
                              }).ToList();
            return adaptsList;
        } 
    }
}