namespace HolmesMVC.Controllers
{
    using System;
    using System.Data;
    using System.Linq;
    using System.Web.Mvc;

    using HolmesMVC.Models;
    using HolmesMVC.Models.ViewModels;

    public class ActorController : HolmesDbController
    {
        //
        // GET: /actor/jeremy_brett

        [AllowAnonymous]
        public ActionResult Details(string urlName = "")
        {
            if (string.IsNullOrEmpty(urlName))
            {
                return RedirectToAction("Index", "Home");
            }

            var actor = Db.Actors.Where(a => a.UrlName == urlName).FirstOrDefault();
            if (actor == null)
            {
                return HttpNotFound();
            }

            var actorView = new ActorView(actor);

            return View("Details", actorView);
        }

        //
        // POST: /actor/create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public int CreateShort(string forename, string surname)
        {
            if (surname == null)
            {
                return -1;
            }
            var urlName = surname.Replace(" ", "_").Replace("'", "_");
            if (!string.IsNullOrEmpty(forename))
            {
                urlName = forename.Replace(" ", "_").Replace("'", "_") + "_" + urlName;
            }
            var actor = new Actor
                            {
                                Forename = forename,
                                Surname = surname,
                                UrlName = urlName.ToLower()
            };

            Db.Actors.Add(actor);
            Db.SaveChanges(); Shared.SomethingChanged(HttpContext.Application);

            return actor.ID;
        }

        [HttpGet]
        public ActionResult Combine(string actorIdStr)
        {
            int[] actorIds = actorIdStr.Split(',').Select(a => Convert.ToInt32(a)).ToArray();
            Array.Sort(actorIds);
            var oneTrueActorId = actorIds[0];
            var oneTrueActor = Db.Actors.Find(oneTrueActorId);

            foreach (var actor in actorIds)
            {
                if (actor == oneTrueActorId)
                {
                    continue;
                }

                var apps = Db.Actors.Find(actor).Appearances;
                foreach (var app in apps)
                {
                    app.ActorID = oneTrueActorId;
                    app.Actor = oneTrueActor;
                }
            }

            Db.SaveChanges();

            foreach (var actor in actorIds)
            {
                if (actor == oneTrueActorId)
                {
                    continue;
                }

                var actorRecord = Db.Actors.Find(actor);
                Db.Actors.Remove(actorRecord);
            }

            Db.SaveChanges();

            Shared.SomethingChanged(HttpContext.Application);

            return RedirectToAction("Details", "Actor", new { oneTrueActor.UrlName });
        }

        // Actor history instances

        [AllowAnonymous]
        public PartialViewResult History()
        {
            return PartialView();
        }

        // and summary instances

        [AllowAnonymous]
        public PartialViewResult HistorySummary()
        {
            return PartialView();
        }

        //
        // GET: /actor/edit/1

        public ActionResult Edit(int id = 0)
        {
            Actor actor = Db.Actors.Find(id);
            if (actor == null)
            {
                return HttpNotFound();
            }
            
            ViewBag.Species = new SelectList(Db.Species, "ID", "Name", actor.SpeciesID);
            return View(actor);
        }

        //
        // POST: /actor/edit/1

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Actor actor)
        {
            if (ModelState.IsValid)
            {
                Db.Entry(actor).State = EntityState.Modified;
                Db.SaveChanges(); Shared.SomethingChanged(HttpContext.Application);

                return RedirectToAction("Details", "Actor", new { actor.UrlName });
            }
            ViewBag.Species = new SelectList(Db.Species, "ID", "Name", actor.SpeciesID);
            return View(actor);
        }

        //
        // GET: /actor/delete/1

        public ActionResult Delete(int id = 0)
        {
            Actor actor = Db.Actors.Find(id);
            if (actor == null)
            {
                return HttpNotFound();
            }
            if (actor.Appearances.Any())
            {
                return View("CantDelete", actor);
            }
            return View(actor);
        }

        //
        // POST: /actor/delete/1

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Actor actor = Db.Actors.Find(id);
            Db.Actors.Remove(actor);
            Db.SaveChanges(); Shared.SomethingChanged(HttpContext.Application);

            return RedirectToAction("Index", "Home");
        }
    }
}