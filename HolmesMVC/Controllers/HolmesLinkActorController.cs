using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using HolmesMVC.Models;

namespace HolmesMVC.Controllers
{
    public class HolmesLinkActorController : HolmesDbController
    {

        //
        // GET: /HolmesLinkActor/

        public ActionResult Index()
        {
            return View(Db.HolmesLinkActors.ToList());
        }

        //
        // GET: /HolmesLinkActor/Details/5

        public ActionResult Details(int id = 0)
        {
            HolmesLinkActor holmeslinkactor = Db.HolmesLinkActors.Find(id);
            if (holmeslinkactor == null)
            {
                return HttpNotFound();
            }
            return View(holmeslinkactor);
        }

        //
        // GET: /HolmesLinkActor/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /HolmesLinkActor/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(HolmesLinkActor holmeslinkactor)
        {
            if (ModelState.IsValid)
            {
                Db.HolmesLinkActors.Add(holmeslinkactor);
                Db.SaveChanges();
                Shared.SomethingChanged(HttpContext.Application);
                return RedirectToAction("Index");
            }

            return View(holmeslinkactor);
        }

        //
        // GET: /HolmesLinkActor/Edit/5

        public ActionResult Edit(int id = 0)
        {
            HolmesLinkActor holmeslinkactor = Db.HolmesLinkActors.Find(id);
            if (holmeslinkactor == null)
            {
                return HttpNotFound();
            }
            return View(holmeslinkactor);
        }

        //
        // POST: /HolmesLinkActor/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(HolmesLinkActor holmeslinkactor)
        {
            if (ModelState.IsValid)
            {
                Db.Entry(holmeslinkactor).State = EntityState.Modified;
                Db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(holmeslinkactor);
        }

        //
        // GET: /HolmesLinkActor/Delete/5

        public ActionResult Delete(int id = 0)
        {
            HolmesLinkActor holmeslinkactor = Db.HolmesLinkActors.Find(id);
            if (holmeslinkactor == null)
            {
                return HttpNotFound();
            }
            return View(holmeslinkactor);
        }

        //
        // POST: /HolmesLinkActor/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            HolmesLinkActor holmeslinkactor = Db.HolmesLinkActors.Find(id);
            Db.HolmesLinkActors.Remove(holmeslinkactor);
            Db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            Db.Dispose();
            base.Dispose(disposing);
        }
    }
}