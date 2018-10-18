using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using HolmesMVC.Models;

namespace HolmesMVC.Controllers
{
    public class HolmesLinkAppearanceController : HolmesDbController
    {

        //
        // GET: /HolmesLinkAppearance/

        public ActionResult Index()
        {
            return View(Db.HolmesLinkAppearances.ToList());
        }

        //
        // GET: /HolmesLinkAppearance/Details/5

        public ActionResult Details(int id = 0)
        {
            HolmesLinkAppearance holmeslinkappearance = Db.HolmesLinkAppearances.Find(id);
            if (holmeslinkappearance == null)
            {
                return HttpNotFound();
            }
            return View(holmeslinkappearance);
        }

        //
        // GET: /HolmesLinkAppearance/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /HolmesLinkAppearance/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(HolmesLinkAppearance holmeslinkappearance)
        {
            if (ModelState.IsValid)
            {
                Db.HolmesLinkAppearances.Add(holmeslinkappearance);
                Db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(holmeslinkappearance);
        }

        //
        // GET: /HolmesLinkAppearance/Edit/5

        public ActionResult Edit(int id = 0)
        {
            HolmesLinkAppearance holmeslinkappearance = Db.HolmesLinkAppearances.Find(id);
            if (holmeslinkappearance == null)
            {
                return HttpNotFound();
            }
            return View(holmeslinkappearance);
        }

        //
        // POST: /HolmesLinkAppearance/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(HolmesLinkAppearance holmeslinkappearance)
        {
            if (ModelState.IsValid)
            {
                Db.Entry(holmeslinkappearance).State = EntityState.Modified;
                Db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(holmeslinkappearance);
        }

        //
        // GET: /HolmesLinkAppearance/Delete/5

        public ActionResult Delete(int id = 0)
        {
            HolmesLinkAppearance holmeslinkappearance = Db.HolmesLinkAppearances.Find(id);
            if (holmeslinkappearance == null)
            {
                return HttpNotFound();
            }
            return View(holmeslinkappearance);
        }

        //
        // POST: /HolmesLinkAppearance/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            HolmesLinkAppearance holmeslinkappearance = Db.HolmesLinkAppearances.Find(id);
            Db.HolmesLinkAppearances.Remove(holmeslinkappearance);
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