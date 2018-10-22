namespace HolmesMVC.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Web.Mvc;
    using HolmesMVC.Enums;
    using HolmesMVC.Models;
    using HolmesMVC.Models.ViewModels;

    [OutputCache(Duration = 2628000, VaryByCustom = "LastDbUpdate")]
    public class AppearanceController : HolmesDbController
    {
        [AllowAnonymous]
        public ActionResult PerEpisode(int epId = -1)
        {
            var relevantApps = (from a in Db.Appearances
                                where a.EpisodeID == epId
                                && (a.ActorID != 0
                                || a.CharacterID != 0)
                                select a).ToList();
            var lockApp = from a in Db.Appearances
                          where a.EpisodeID == epId
                          && a.ActorID == 0
                          && a.CharacterID == 0
                          select a;
            ViewBag.Locked = lockApp.Any();
            if (!relevantApps.Any())
            {
                ViewBag.EpId = epId;
                return PartialView(new List<Appearance>());
            }

            var adaptation = relevantApps.First().Episode.Season.AdaptationID;
            ViewBag.EpId = epId;

            // sort characters

            var hApps =
                (from a in relevantApps where a.CharacterID == (int)CanonCharacter.Holmes select a)
                    .ToList();
            var wApps =
                (from a in relevantApps where a.CharacterID == (int)CanonCharacter.Watson select a)
                    .ToList();
            var canonApps = (from a in relevantApps
                         where
                             a.CharacterID != (int)CanonCharacter.Holmes
                             && a.CharacterID != (int)CanonCharacter.Watson
                             && a.Character.IsCanon
                         select a).ToList();
            var uncanonApps = (from a in relevantApps
                           where !a.Character.IsCanon
                           select a).ToList();

            canonApps = canonApps.OrderBy(a => a.SortSurname()).ThenBy(a => a.SortForename()).ToList();
            uncanonApps = uncanonApps.OrderBy(a => a.SortSurname()).ThenBy(a => a.SortForename()).ToList();

            relevantApps.Clear();
            relevantApps.AddRange(hApps);
            relevantApps.AddRange(wApps);
            relevantApps.AddRange(canonApps);
            relevantApps.AddRange(uncanonApps);

            return PartialView(relevantApps);
        }

        [HttpGet]
        public ActionResult MassAdd(int id, int actorid = -1, int characterid = -1)
        {
            var adapt = Db.Adaptations.Find(id);

            if (null == adapt)
            {
                return HttpNotFound();
            }

            var eps = adapt.Seasons.SelectMany(s => s.Episodes).OrderBy(e => e.Airdate).ToList();

            var massAdd = new MassAdd
                              {
                                  AdaptationID = adapt.ID,
                                  AdaptationUrlName = adapt.UrlName,
                                  Adaptation = adapt
                              };

            ViewBag.Eps = eps;

            if (actorid > 0)
            {
                massAdd.ActorID = actorid;
                massAdd.Actor = Db.Actors.Find(actorid);
            }
            else
            {
                ViewBag.Actor = ActorList();
            }

            if (characterid > 0)
            {
                massAdd.CharacterID = characterid;
                massAdd.Character = Db.Characters.Find(characterid);
            }
            else
            {
                ViewBag.Character = CharacterList();
            }

            return View(massAdd);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MassAdd(MassAdd model)
        {
            if (model.ActorID < 1 || model.CharacterID < 1 || model.Episodes.Count() < 1)
            {
                return HttpNotFound();
            }

            foreach (var epId in model.Episodes)
            {
                var existApp = (from a in Db.Appearances
                                where a.EpisodeID == epId
                                && a.ActorID == model.ActorID
                                && a.CharacterID == model.CharacterID
                                select a).Any();

                // Silently fail if trying to add duplicate appearance
                if (existApp)
                {
                    continue;
                }

                var appearance = new Appearance
                                         {
                                             EpisodeID = epId,
                                             ActorID = model.ActorID,
                                             CharacterID = model.CharacterID
                };
                Db.Appearances.Add(appearance);
            }

            Db.SaveChanges();
            
            return RedirectToRoute("Details", new { controller = "Adaptation", id = model.AdaptationID });
        }

        [OutputCache(NoStore = true, Duration = 0)]
        public ActionResult Create(int id = -1, int actorid = -1, int characterid = -1)
        {
            if (id == -1)
            {
                return HttpNotFound();
            }

            var episode = Db.Episodes.Find(id);
            if (episode == null)
            {
                throw new NullReferenceException("Episode " + id + " not found!");
            }

            var appearance = new Appearance
            {
                Episode = episode,
                EpisodeID = episode.ID
            };

            var adapt = episode.Season.Adaptation;
            if (adapt.IsCanon)
            {
                appearance.ActorID = 0;
            }
            else if (actorid > 0)
            {
                appearance.ActorID = actorid;
                appearance.Actor = Db.Actors.Find(actorid);
            }
            else
            {
                ViewBag.Actor = ActorList();
            }

            if (characterid > 0)
            {
                appearance.CharacterID = characterid;
                appearance.Character = Db.Characters.Find(characterid);
            }
            else
            {
                ViewBag.Character = CharacterList();
            }

            return View(appearance);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Appearance appearance)
        {
            if (!ModelState.IsValid)
            {
                return View(appearance);
            }
            
            Db.Appearances.Add(appearance);
            Db.SaveChanges();

            return RedirectToRoute("Details", new { controller = "Episode", id = appearance.EpisodeID });
        }

        public List<SelectListItem> ActorList()
        {
            var actors = (from a in Db.Actors select a).ToList();
            var actorlist = (from actor in actors
                             where actor.Surname != "Nobody"
                             select
                                 new SelectListItem
                                 {
                                     Value = actor.ID.ToString(CultureInfo.InvariantCulture),
                                     Text = actor.DisplayName
                                 }).OrderBy(a => a.Text).ToList();
            return actorlist;
        }

        public List<SelectListItem> CharacterList()
        {
            var characters = (from c in Db.Characters select c).ToList();
            var characterlist = (from character in characters
                                 select
                                     new SelectListItem
                                     {
                                         Value =
                                             character.ID.ToString(CultureInfo.InvariantCulture),
                                         Text =
                                             character.DisplayName + (!string.IsNullOrEmpty(character.StoryID) ? " (" + character.StoryID + ")" : "")
                                     }).OrderBy(a => a.Text).ToList();
            return characterlist;
        }

        [OutputCache(NoStore = true, Duration = 0)]
        public ActionResult Delete(int id = 0)
        {
            var appearance = Db.Appearances.Find(id);
            if (appearance == null)
            {
                return HttpNotFound();
            }

            return View(appearance);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var appearance = Db.Appearances.Find(id);
            Db.Appearances.Remove(appearance);
            Db.SaveChanges();
            
            return RedirectToRoute("Details", new { controller = "Episode", id = appearance.EpisodeID });
        }
    }
}