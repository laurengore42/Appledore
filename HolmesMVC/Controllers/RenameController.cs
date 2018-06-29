namespace HolmesMVC.Controllers
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Web.Mvc;
    using HolmesMVC.Models;
    using HolmesMVC.Models.ViewModels;

    public class RenameController : HolmesDbController
    {
        //
        // GET: /Rename/Create

        public ActionResult Create(int id = -1)
        {
            if (id == -1)
            {
                var adapts = (from a in Db.Adaptations select a).ToList();
                var adaptsList = (from a in adapts
                                  select new SelectListItem
                                  {
                                      Value = a.ID.ToString(CultureInfo.InvariantCulture),
                                      Text = Shared.DisplayName(a)
                                  }).OrderBy(a => a.Text).ToList();
                ViewBag.Adaptation = adaptsList;

                var chars = (from c in Db.Characters select c).ToList();
                var charsList = (from c in chars
                                 where Shared.IsCanon(c)
                                 select new SelectListItem
                                 {
                                     Value = c.ID.ToString(CultureInfo.InvariantCulture),
                                     Text = Shared.DisplayName(c) + " [" + c.ID + "]"
                                 }).OrderBy(a => a.Text).ToList();
                ViewBag.Character = charsList;

                var actors = (from a in Db.Actors select a).ToList();
                var actorsList = (from a in actors
                                  select new SelectListItem
                                  {
                                      Value = a.ID.ToString(CultureInfo.InvariantCulture),
                                      Text = Shared.DisplayName(a) + " [" + a.ID + "]"
                                  }).OrderBy(a => a.Text).ToList();
                ViewBag.Actor = actorsList;

                ViewBag.Honorific = new SelectList(Db.Honorifics, "ID", "Name");
                return View();
            }


            var appearance = Db.Appearances.Find(id);

            if (appearance == null)
            {
                return Create();
            }

            var adapt =
                Db.Adaptations.Find(appearance.Episode1.Season1.Adaptation);

            var actor = Db.Actors.Find(appearance.Actor);

            var character = Db.Characters.Find(appearance.Character);

            if (adapt == null || actor == null || character == null)
            {
                return Create();
            }

            var rename = new Rename
                             {
                                 Adaptation = adapt.ID,
                                 Adaptation1 = adapt,
                                 Actor = actor.ID,
                                 Actor1 = actor,
                                 Character = character.ID,
                                 Character1 = character
                             };
            ViewBag.Honorific = new SelectList(Db.Honorifics, "ID", "Name");

            return View("CreateFull", rename);
        }

        //
        // POST: /Rename/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Rename rename)
        {
            if (ModelState.IsValid)
            {
                var adapt = rename.Adaptation;
                Db.Renames.Add(rename);
                Db.SaveChanges(); Shared.SomethingChanged(HttpContext.Application);
                return RedirectToAction(
                    "Details",
                    "Adaptation",
                    new { id = adapt });
            }

            return View(rename);
        }

        //
        // GET: /Rename/Delete/5

        public ActionResult Delete(int id = 0)
        {
            var appearance = Db.Appearances.Find(id);

            if (appearance == null)
            {
                return HttpNotFound();
            }

            var adapt =
                Db.Adaptations.Find(appearance.Episode1.Season1.Adaptation);

            var actor = Db.Actors.Find(appearance.Actor);

            var character = Db.Characters.Find(appearance.Character);

            var rename = (from r in Db.Renames where r.Adaptation == adapt.ID && r.Actor == actor.ID && r.Character == character.ID select r).FirstOrDefault();
            if (rename == null)
            {
                return HttpNotFound();
            }

            return View(new RenameView(rename));
        }

        //
        // POST: /Rename/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var appearance = Db.Appearances.Find(id);

            if (appearance == null)
            {
                throw new ArgumentException("Failure while trying to delete rename for appearance " + id + "!");
            }

            var adapt =
                Db.Adaptations.Find(appearance.Episode1.Season1.Adaptation);

            var actor = Db.Actors.Find(appearance.Actor);

            var character = Db.Characters.Find(appearance.Character);

            if (adapt == null || actor == null || character == null)
            {
                throw new ArgumentException("Failure while trying to delete rename for appearance " + id + "!");
            }

            var rename = (from r in Db.Renames where r.Adaptation == adapt.ID && r.Actor == actor.ID && r.Character == character.ID select r).FirstOrDefault();
            if (rename == null)
            {
                throw new ArgumentException("Failure while trying to delete rename for appearance " + id + "!");
            }

            var adaptid = adapt.ID;
            Db.Renames.Remove(rename);
            Db.SaveChanges(); Shared.SomethingChanged(HttpContext.Application);
            return RedirectToAction(
                "Details",
                "Adaptation",
                new { id = adaptid });
        }
    }
}