namespace HolmesMVC.Controllers
{
    using System.Collections.Generic;
    using System.Data;
    using System.Globalization;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;

    using HolmesMVC.Models;
    using HolmesMVC.Models.ViewModels;

    public class EpisodeController : HolmesDbController
    {
        //
        // GET: /Episode/Details/5

        [Stopwatch]
        [AllowAnonymous]
        public ActionResult Details(int id = 0)
        {
            Episode episode = Db.Episodes.Find(id);
            if (episode == null)
            {
                return HttpNotFound();
            }

            var canon = Shared.DisplayName(episode.Season1.Adaptation1) == "Canon";

            // the Canon case
            if (canon)
            {
                return RedirectToAction("Details","Story", new {id = episode.Story});
            }

            // the single-film case
            if (episode.Season1.Adaptation1.Seasons.SelectMany(s => s.Episodes).Count() == 1)
            {
                return RedirectToAction("Details","Adaptation",new {ID = episode.Season1.Adaptation});
            }

            return View(new EpisodeView(episode));
        }

        //
        // GET: /Episode/Create

        public ActionResult Create(int id = -1)
        {
            if (id == -1)
            {
                return HttpNotFound();
            }

            var chosenAdapt = Db.Adaptations.Find(id);

            if (chosenAdapt == null)
            {
                return HttpNotFound();
            }

            ViewBag.chosenAdapt = chosenAdapt;
            ViewBag.adaptId = chosenAdapt.ID;

            ViewBag.Story = new SelectList(Db.Stories, "ID", "Name");
            ViewBag.Season = new SelectList(Db.Seasons, "ID", "Name");

            return View(new EpisodeView());
        }

        //
        // POST: /Episode/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Episode episode)
        {
            if (ModelState.IsValid)
            {
                Db.Episodes.Add(episode);
                Db.SaveChanges(); Shared.SomethingChanged(HttpContext.Application);
                return RedirectToRoute("Details", new { controller = "Episode", id = episode.ID });
            }

            ViewBag.Season = new SelectList(Db.Seasons, "ID", "Name", episode.Season);
            ViewBag.Story = new SelectList(Db.Stories, "ID", "Name", episode.Story);
            return View(new EpisodeView(episode));
        }

        //
        // GET: /Episode/Edit/5

        public ActionResult Edit(int id = 0)
        {
            var episode = Db.Episodes.Find(id);
            if (episode == null)
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

            ViewBag.Adaptation = new SelectList(listAdapts, "Value", "Text", episode.Season1.Adaptation);
            
            ViewBag.Story = new SelectList(Db.Stories, "ID", "Name", episode.Story);
            return View(new EpisodeView(episode));
        }

        //
        // POST: /Episode/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Episode episode)
        {
            if (ModelState.IsValid)
            {
                var id = episode.ID;
                Db.Entry(episode).State = EntityState.Modified;
                Db.SaveChanges(); Shared.SomethingChanged(HttpContext.Application);
                return RedirectToAction("Details", "Episode", new { ID = id });
            }
            ViewBag.Season = new SelectList(Db.Seasons, "ID", "Name", episode.Season);
            ViewBag.Story = new SelectList(Db.Stories, "ID", "Name", episode.Story);
            return View( new EpisodeView(episode));
        }

        public PartialViewResult SeasonBox(int id, int currentSeason)
        {
            var relevantAdapt = Db.Adaptations.Find(id);
            if (relevantAdapt == null)
            {
                throw new HttpException(404, "Not found");
            }
            
            var listSeasons = new List<SelectListItem>();
            var relevantSeasons = relevantAdapt.Seasons;

            foreach (var season in relevantSeasons)
            {
                    var val = season.ID.ToString(CultureInfo.InvariantCulture);
                    var name = season.AirOrder.ToString(CultureInfo.InvariantCulture);
                    name += string.IsNullOrWhiteSpace(season.Name) ? string.Empty : " " + season.Name;
                    listSeasons.Add(new SelectListItem { Text = name, Value = val, Selected = season.ID == currentSeason });
            }

            if (currentSeason < 0)
            {
                currentSeason =
                    (from s in relevantSeasons
                     orderby s.AirOrder descending
                     select s.ID).FirstOrDefault();
            }

            var model = new EpisodeView();

            listSeasons = listSeasons.OrderBy(item => item.Text).ToList();
            ViewBag.SeasonList = new SelectList(listSeasons, "Value", "Text");
            model.Season = currentSeason;
            model.Adaptation = relevantAdapt.ID;
            ViewBag.adaptName = Shared.DisplayName(relevantAdapt);

            return PartialView(model);
        }

        //
        // GET: /Episode/Delete/5

        public ActionResult Delete(int id = 0)
        {
            var episode = Db.Episodes.Find(id);
            if (episode == null)
            {
                return HttpNotFound();
            }
            if (episode.Appearances.Any())
            {
                return View("CantDelete", episode);
            }
            return View(episode);
        }

        //
        // POST: /Episode/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var episode = Db.Episodes.Find(id);
            var adaptId = episode.Season1.Adaptation;
            Db.Episodes.Remove(episode);
            Db.SaveChanges(); Shared.SomethingChanged(HttpContext.Application);
            return RedirectToAction("Details","Adaptation", new {ID = adaptId});
        }

        public ActionResult Lock(int id)
        {
            var episode = Db.Episodes.Find(id);
            var lockApp = new Appearance();
            lockApp.Actor1 = Db.Actors.Find(0);
            lockApp.Character1 = Db.Characters.Find(0);
            lockApp.Episode1 = episode;
            Db.Appearances.Add(lockApp);
            Db.SaveChanges(); Shared.SomethingChanged(HttpContext.Application);

            return RedirectToAction("Details", "Episode", new { id });
        }

        public ActionResult Unlock(int id)
        {
            var episode = Db.Episodes.Find(id);
            var lockApp = (from a in episode.Appearances
                          where a.Actor == 0
                          && a.Character == 0
                          select a).First();
            Db.Appearances.Remove(lockApp);
            Db.SaveChanges(); Shared.SomethingChanged(HttpContext.Application);

            return RedirectToAction("Details", "Episode", new { id });
        }
    }
}