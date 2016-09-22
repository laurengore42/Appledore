using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HolmesMVC.Models;

namespace HolmesMVC.Controllers
{
    public class HolmesLinkController : HolmesDbController
    {

        //
        // GET: /HolmesLink/

        public ActionResult Index()
        {
            return View(Db.HolmesLinks.ToList());
        }

        //
        // GET: /HolmesLink/Details/5

        public ActionResult Details(int id = 0)
        {
            HolmesLink holmeslink = Db.HolmesLinks.Find(id);
            if (holmeslink == null)
            {
                return HttpNotFound();
            }
            return View(holmeslink);
        }

        //
        // GET: /HolmesLink/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /HolmesLink/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(HolmesLink holmeslink)
        {
            if (ModelState.IsValid)
            {
                Db.HolmesLinks.Add(holmeslink);
                Db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(holmeslink);
        }

        //
        // GET: /HolmesLink/Edit/5

        public ActionResult Edit(int id = 0)
        {
            HolmesLink holmeslink = Db.HolmesLinks.Find(id);
            if (holmeslink == null)
            {
                return HttpNotFound();
            }
            return View(holmeslink);
        }

        //
        // POST: /HolmesLink/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(HolmesLink holmeslink)
        {
            if (ModelState.IsValid)
            {
                Db.Entry(holmeslink).State = EntityState.Modified;
                Db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(holmeslink);
        }

        //
        // GET: /HolmesLink/Delete/5

        public ActionResult Delete(int id = 0)
        {
            HolmesLink holmeslink = Db.HolmesLinks.Find(id);
            if (holmeslink == null)
            {
                return HttpNotFound();
            }
            return View(holmeslink);
        }

        //
        // POST: /HolmesLink/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            HolmesLink holmeslink = Db.HolmesLinks.Find(id);
            Db.HolmesLinks.Remove(holmeslink);
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