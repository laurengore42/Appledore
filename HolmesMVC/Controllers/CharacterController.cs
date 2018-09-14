namespace HolmesMVC.Controllers
{
    using System;
    using System.Data;
    using System.Linq;
    using System.Web.Mvc;
    using HolmesMVC.Models;
    using HolmesMVC.Models.ViewModels;

    public class CharacterController : HolmesDbController
    {
        //
        // GET: /character/5

        [AllowAnonymous]
        public ActionResult Details(int id = 0)
        {
            if (id == 0)
            {
                return RedirectToAction("Index", "Home");
            }

            var character = Db.Characters.Find(id);
            if (character == null)
            {
                return HttpNotFound();
            }

            return View(new CharView(character));
        }

        //
        // GET: /character/sherlock_holmes

        [AllowAnonymous]
        public ActionResult NewDetails(string urlName = "")
        {
            if (urlName == "")
            {
                return RedirectToAction("Index", "Home");
            }

            var character = Db.Characters.Where(a => a.UrlName == urlName).FirstOrDefault();
            if (character == null)
            {
                return HttpNotFound();
            }

            return View("Details", new CharView(character));
        }

        //
        // POST: /character/create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public int CreateShort(string forename, string surname)
        {
            if (surname == null)
            {
                return -1;
            }
            var character = new Character
                                {
                                    Forename = forename,
                                    Surname = surname
                                };

            Db.Characters.Add(character);
            Db.SaveChanges(); Shared.SomethingChanged(HttpContext.Application);

            return character.ID;
        }

        [HttpGet]
        public ActionResult Combine(string charIdStr)
        {
            int[] charIds = charIdStr.Split(',').Select(c => Convert.ToInt32(c)).ToArray();
            Array.Sort(charIds);
            var oneTrueCharId = charIds[0];
            var oneTrueChar = Db.Characters.Find(oneTrueCharId);

            foreach (var character in charIds)
            {
                if (character == oneTrueCharId)
                {
                    continue;
                }

                var apps = Db.Characters.Find(character).Appearances;
                foreach (var app in apps)
                {
                    app.CharacterID = oneTrueCharId;
                    app.Character = oneTrueChar;
                }
            }

            Db.SaveChanges();

            foreach (var character in charIds)
            {
                if (character == oneTrueCharId)
                {
                    continue;
                }

                var charRecord = Db.Characters.Find(character);
                Db.Characters.Remove(charRecord);
            }

            Db.SaveChanges();

            Shared.SomethingChanged(HttpContext.Application);

            return RedirectToAction("Details", "Character", new { id = oneTrueCharId });
        }

        //
        // GET: /character/edit/5

        public ActionResult Edit(int id = 0)
        {
            Character character = Db.Characters.Find(id);
            if (character == null)
            {
                return HttpNotFound();
            }
            ViewBag.HonorificID = new SelectList(Db.Honorifics.OrderBy(h => h.Name), "ID", "Name", character.HonorificID);
            ViewBag.SpeciesID = new SelectList(Db.Species, "ID", "Name", character.SpeciesID);
            ViewBag.StoryID = new SelectList(Db.Stories, "ID", "Name", character.StoryID);
            return View(character);
        }

        //
        // POST: /character/edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Character character)
        {
            if (ModelState.IsValid)
            {
                Db.Entry(character).State = EntityState.Modified;
                Db.SaveChanges(); Shared.SomethingChanged(HttpContext.Application);

                return RedirectToRoute("Details", new { controller = "Character", id = character.ID });
            }
            ViewBag.Honorific = new SelectList(Db.Honorifics.OrderBy(h => h.Name), "ID", "Name", character.HonorificID);
            ViewBag.Species = new SelectList(Db.Species, "ID", "Name", character.SpeciesID);
            ViewBag.StoryID = new SelectList(Db.Stories, "ID", "Name", character.StoryID);
            return View(character);
        }

        //
        // GET: /character/delete/5

        public ActionResult Delete(int id = 0)
        {
            Character character = Db.Characters.Find(id);
            if (character == null)
            {
                return HttpNotFound();
            }
            if (character.Appearances.Any() || character.Renames.Any())
            {
                return View("CantDelete", character);
            }
            return View(character);
        }

        //
        // POST: /character/delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Character character = Db.Characters.Find(id);
            Db.Characters.Remove(character);
            Db.SaveChanges(); Shared.SomethingChanged(HttpContext.Application);

            return RedirectToAction("Index", "Home");
        }
    }
}