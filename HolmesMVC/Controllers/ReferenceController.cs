namespace HolmesMVC.Controllers
{
    using System.Data;
    using System.Data.Entity;
    using System.Linq;
    using System.Web.Mvc;
    using HolmesMVC.Models;

    public class ReferenceController : HolmesDbController
    {
        //
        // GET: /Reference/

        public ActionResult Index()
        {
            var references = Db.References.Include(r => r.Episode).Include(r => r.Story);
            return View(references.ToList());
        }

        //
        // GET: /Reference/Details/5

        public ActionResult Details(int id = 0)
        {
            Reference reference = Db.References.Find(id);
            if (reference == null)
            {
                return HttpNotFound();
            }
            return View(reference);
        }

        //
        // GET: /Reference/Create

        public ActionResult Create()
        {
            ViewBag.Episode = new SelectList(Db.Episodes, "ID", "Story");
            ViewBag.Story = new SelectList(Db.Stories, "ID", "Name");
            return View();
        }

        //
        // POST: /Reference/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Reference reference)
        {
            if (ModelState.IsValid)
            {
                Db.References.Add(reference);
                Db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Episode = new SelectList(Db.Episodes, "ID", "Story", reference.EpisodeID);
            ViewBag.Story = new SelectList(Db.Stories, "ID", "Name", reference.StoryID);
            return View(reference);
        }

        //
        // GET: /Reference/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Reference reference = Db.References.Find(id);
            if (reference == null)
            {
                return HttpNotFound();
            }
            ViewBag.Episode = new SelectList(Db.Episodes, "ID", "Story", reference.EpisodeID);
            ViewBag.Story = new SelectList(Db.Stories, "ID", "Name", reference.StoryID);
            return View(reference);
        }

        //
        // POST: /Reference/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Reference reference)
        {
            if (ModelState.IsValid)
            {
                Db.Entry(reference).State = EntityState.Modified;
                Db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Episode = new SelectList(Db.Episodes, "ID", "Story", reference.EpisodeID);
            ViewBag.Story = new SelectList(Db.Stories, "ID", "Name", reference.StoryID);
            return View(reference);
        }

        //
        // GET: /Reference/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Reference reference = Db.References.Find(id);
            if (reference == null)
            {
                return HttpNotFound();
            }
            return View(reference);
        }

        //
        // POST: /Reference/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Reference reference = Db.References.Find(id);
            Db.References.Remove(reference);
            Db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}