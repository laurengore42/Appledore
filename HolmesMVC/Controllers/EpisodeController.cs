namespace HolmesMVC.Controllers
{
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Entity;
    using System.Globalization;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using HolmesMVC.Models;
    using HolmesMVC.Models.ViewModels;

    [OutputCache(Duration = 86400, VaryByCustom = "LastDbUpdate")]
    public class EpisodeController : HolmesDbController
    {
        //
        // GET: /{adaptWord}/{adaptName}/season/{seasonNumber}/episode/{episodeNumber}

        [AllowAnonymous]
        public ActionResult LongformDetails(string adaptName = "", int seasonNumber = 0, int episodeNumber = 0)
        {
            Adaptation adaptation = Db.Adaptations.Where(a => a.UrlName == adaptName).FirstOrDefault();
            if (adaptation == null)
            {
                return RedirectToAction("Details", "Adaptation", new { adaptName });
            }

            Season season = adaptation.Seasons.Where(s => s.AirOrder == seasonNumber).FirstOrDefault();
            if (season == null)
            {
                return RedirectToAction("Details", "Adaptation", new { adaptName });
            }

            Episode episode = season.Episodes.Where(e => e.AirOrder == episodeNumber).FirstOrDefault();
            if (episode == null)
            {
                return RedirectToAction("Details", "Adaptation", new { adaptName });
            }

            var canon = Shared.DisplayName(episode.Season.Adaptation) == "Canon";

            // the Canon case
            if (canon)
            {
                return RedirectToAction("Details", "Story", new { id = episode.StoryID });
            }

            // the single-film case
            if (episode.Season.Adaptation.Seasons.SelectMany(s => s.Episodes).Count() == 1)
            {
                return RedirectToAction("Details", "Adaptation", new { episode.Season.Adaptation.UrlName });
            }

            return View("Details", new EpisodeView(episode));
        }

        //
        // GET: /episode/details/5

        [AllowAnonymous]
        public ActionResult Details(int id = 0)
        {
            Episode episode = Db.Episodes.Find(id);
            if (episode == null)
            {
                return HttpNotFound();
            }

            return RedirectToActionPermanent("LongformDetails", new { adaptWord = "adaptation", adaptName = episode.Season.Adaptation.UrlName, seasonNumber = episode.Season.AirOrder, episodeNumber = episode.AirOrder });
        }

        //
        // GET: /episode/create

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
            ViewBag.adaptUrlName = chosenAdapt.UrlName;

            ViewBag.Story = new SelectList(Db.Stories, "ID", "Name");
            ViewBag.Season = new SelectList(Db.Seasons, "ID", "Name");

            return View(new EpisodeView());
        }

        //
        // POST: /episode/create

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

            ViewBag.Season = new SelectList(Db.Seasons, "ID", "Name", episode.SeasonID);
            ViewBag.Story = new SelectList(Db.Stories, "ID", "Name", episode.StoryID);
            return View(new EpisodeView(episode));
        }

        //
        // GET: /episode/edit/5

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

            ViewBag.Adaptation = new SelectList(listAdapts, "Value", "Text", episode.Season.AdaptationID);
            
            ViewBag.Story = new SelectList(Db.Stories, "ID", "Name", episode.StoryID);
            return View(new EpisodeView(episode));
        }

        //
        // POST: /episode/edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Episode episode)
        {
            if (ModelState.IsValid)
            {
                var id = episode.ID;
                Db.Entry(episode).State = EntityState.Modified;
                Db.SaveChanges(); Shared.SomethingChanged(HttpContext.Application);
                return RedirectToRoute("EpDetails", new { adaptWord = episode.Season.Adaptation.MediumUrlName, adaptName = episode.Season.Adaptation.UrlName, seasonNumber = episode.Season.AirOrder, episodeNumber = episode.AirOrder });
            }
            ViewBag.Season = new SelectList(Db.Seasons, "ID", "Name", episode.SeasonID);
            ViewBag.Story = new SelectList(Db.Stories, "ID", "Name", episode.StoryID);
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
        // GET: /episode/delete/5

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
        // POST: /episode/delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var episode = Db.Episodes.Find(id);
            var urlName = episode.Season.Adaptation.UrlName;
            Db.Episodes.Remove(episode);
            Db.SaveChanges(); Shared.SomethingChanged(HttpContext.Application);
            return RedirectToAction("Details", "Adaptation", new { urlName });
        }

        public ActionResult Lock(int id)
        {
            var episode = Db.Episodes.Find(id);
            var lockApp = new Appearance
            {
                Actor = Db.Actors.Find(0),
                Character = Db.Characters.Find(0),
                Episode = episode
            };
            Db.Appearances.Add(lockApp);
            Db.SaveChanges(); Shared.SomethingChanged(HttpContext.Application);

            return RedirectToRoute("EpDetails", new { adaptWord = episode.Season.Adaptation.MediumUrlName, adaptName = episode.Season.Adaptation.UrlName, seasonNumber = episode.Season.AirOrder, episodeNumber = episode.AirOrder });
        }

        public ActionResult Unlock(int id)
        {
            var episode = Db.Episodes.Find(id);
            var lockApp = (from a in episode.Appearances
                          where a.ActorID == 0
                          && a.CharacterID == 0
                          select a).First();
            Db.Appearances.Remove(lockApp);
            Db.SaveChanges(); Shared.SomethingChanged(HttpContext.Application);

            return RedirectToRoute("EpDetails", new { adaptWord = episode.Season.Adaptation.MediumUrlName, adaptName = episode.Season.Adaptation.UrlName, seasonNumber = episode.Season.AirOrder, episodeNumber = episode.AirOrder });
        }
    }
}