namespace HolmesMVC.Controllers
{
    using System.Linq;
    using System.Web.Mvc;
    using HolmesMVC.Models;

    public class HonorificController : HolmesDbController
    {
        //
        // GET: /Honorific/

        public ActionResult Index()
        {
            return View(Db.Honorifics.ToList().OrderBy(h => h.Name));
        }

        //
        // GET: /Honorific/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Honorific/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Honorific honorific)
        {
            if (ModelState.IsValid)
            {
                Db.Honorifics.Add(honorific);
                Db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(honorific);
        }

        //
        // GET: /Honorific/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Honorific honorific = Db.Honorifics.Find(id);
            if (honorific == null)
            {
                return HttpNotFound();
            }
            return View(honorific);
        }

        //
        // POST: /Honorific/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Honorific honorific = Db.Honorifics.Find(id);
            Db.Honorifics.Remove(honorific);
            Db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}