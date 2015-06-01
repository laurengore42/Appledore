namespace HolmesMVC.Controllers
{
    using System.Data;
    using System.Linq;
    using System.Web.Mvc;
    using HolmesMVC.Models;

    public class MediumController : HolmesDbController
    {
        //
        // GET: /Medium/

        public ActionResult Index()
        {
            return View(Db.Media.ToList());
        }

        //
        // GET: /Medium/Details/5

        public ActionResult Details(int id = 0)
        {
            Medium medium = Db.Media.Find(id);
            if (medium == null)
            {
                return HttpNotFound();
            }
            return View(medium);
        }

        //
        // GET: /Medium/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Medium/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Medium medium)
        {
            if (ModelState.IsValid)
            {
                Db.Media.Add(medium);
                Db.SaveChanges(); Shared.SomethingChanged(HttpContext.Application);
                return RedirectToAction("Index");
            }

            return View(medium);
        }

        //
        // GET: /Medium/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Medium medium = Db.Media.Find(id);
            if (medium == null)
            {
                return HttpNotFound();
            }
            return View(medium);
        }

        //
        // POST: /Medium/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Medium medium)
        {
            if (ModelState.IsValid)
            {
                Db.Entry(medium).State = EntityState.Modified;
                Db.SaveChanges(); Shared.SomethingChanged(HttpContext.Application);
                return RedirectToAction("Index");
            }
            return View(medium);
        }

        //
        // GET: /Medium/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Medium medium = Db.Media.Find(id);
            if (medium == null)
            {
                return HttpNotFound();
            }
            return View(medium);
        }

        //
        // POST: /Medium/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Medium medium = Db.Media.Find(id);
            Db.Media.Remove(medium);
            Db.SaveChanges(); Shared.SomethingChanged(HttpContext.Application);
            return RedirectToAction("Index");
        }
    }
}