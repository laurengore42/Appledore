﻿namespace HolmesMVC.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Web.Mvc;

    using HolmesMVC.Models;
    using HolmesMVC.Models.ViewModels;

    public class AppearanceController : HolmesDbController
    {
        [AllowAnonymous]
        public ActionResult PerEpisode(int epId = -1)
        {
            var relevantApps = (from a in Db.Appearances
                                where a.Episode == epId
                                && (a.Actor != 0
                                || a.Character != 0)
                                select a).ToList();
            var lockApp = from a in Db.Appearances
                          where a.Episode == epId
                          && a.Actor == 0
                          && a.Character == 0
                          select a;
            ViewBag.Locked = lockApp.Any();
            if (!relevantApps.Any())
            {
                ViewBag.EpId = epId;
                return PartialView(new List<Appearance>());
            }

            var adaptation = relevantApps.First().Episode1.Season1.Adaptation;
            ViewBag.EpId = epId;

            // sort characters
            var holmesId = Shared.GetHolmes(relevantApps.Select(a => a.Character1).ToList());
            var watsonId = Shared.GetWatson(relevantApps.Select(a => a.Character1).ToList());

            var hApps =
                (from a in relevantApps where a.Character == holmesId select a)
                    .ToList();
            var wApps =
                (from a in relevantApps where a.Character == watsonId select a)
                    .ToList();
            var canonApps = (from a in relevantApps
                         where
                             a.Character != holmesId && a.Character != watsonId
                             && Shared.IsCanon(a.Character1)
                         orderby a.Character1.Surname, a.Character1.Forename
                         select a).ToList();
            var uncanonApps = (from a in relevantApps
                           where !Shared.IsCanon(a.Character1)
                           orderby a.Character1.Surname, a.Character1.Forename
                           select a).ToList();

            relevantApps.Clear();
            relevantApps.AddRange(hApps);
            relevantApps.AddRange(wApps);
            relevantApps.AddRange(canonApps);
            relevantApps.AddRange(uncanonApps);

            return PartialView(relevantApps);
        }

        [HttpGet]
        public ActionResult MassAdd(int id, int actor = -1, int character = -1)
        {
            var adapt = Db.Adaptations.Find(id);

            if (null == adapt)
            {
                return HttpNotFound();
            }

            var eps = adapt.Seasons.SelectMany(s => s.Episodes).OrderBy(e => e.Airdate).ToList();

            var massAdd = new MassAdd
                              {
                                  Adaptation = adapt.ID,
                                  Adaptation1 = adapt
                              };

            ViewBag.Eps = eps;

            if (actor > 0)
            {
                massAdd.Actor = actor;
                massAdd.Actor1 = Db.Actors.Find(actor);
            }
            else
            {
                ViewBag.Actor = ActorList();
            }

            if (character > 0)
            {
                massAdd.Character = character;
                massAdd.Character1 = Db.Characters.Find(character);
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
            var returnTo = model.Adaptation;
            if (model.Actor < 1 || model.Character < 1 || model.Episodes.Count() < 1)
            {
                return HttpNotFound();
            }

            foreach (var epId in model.Episodes)
            {
                var existApp = (from a in Db.Appearances
                                where a.Episode == epId
                                && a.Actor == model.Actor
                                && a.Character == model.Character
                                select a).Any();

                // Silently fail if trying to add duplicate appearance
                if (existApp)
                {
                    continue;
                }

                var appearance = new Appearance
                                         {
                                             Episode = epId,
                                             Actor = model.Actor,
                                             Character = model.Character
                                         };
                Db.Appearances.Add(appearance);
            }

            Db.SaveChanges(); Shared.SomethingChanged(HttpContext.Application);

            return RedirectToAction("Details", "Adaptation", new { id = returnTo });
        }

        public ActionResult Create(int id = -1, int actor = -1, int character = -1)
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
                Episode1 = episode,
                Episode = episode.ID
            };

            var adapt = episode.Season1.Adaptation1;
            if (Shared.DisplayName(adapt) == "Canon")
            {
                appearance.Actor = 0;
            }
            else if (actor > 0)
            {
                appearance.Actor = actor;
                appearance.Actor1 = Db.Actors.Find(actor);
            }
            else
            {
                ViewBag.Actor = ActorList();
            }

            if (character > 0)
            {
                appearance.Character = character;
                appearance.Character1 = Db.Characters.Find(character);
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

            var id = appearance.Episode;
            Db.Appearances.Add(appearance);
            Db.SaveChanges(); Shared.SomethingChanged(HttpContext.Application);

            return RedirectToAction("Details", "Episode", new { ID = id });
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
                                     Text = Shared.DisplayName(actor)
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
                                             Shared.DisplayName(character)
                                     }).OrderBy(a => a.Text).ToList();
            return characterlist;
        }

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
            var epId = appearance.Episode;
            Db.Appearances.Remove(appearance);
            Db.SaveChanges(); Shared.SomethingChanged(HttpContext.Application);

            return RedirectToAction("Details", "Episode", new { ID = epId });
        }
    }
}