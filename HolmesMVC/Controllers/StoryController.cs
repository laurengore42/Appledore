namespace HolmesMVC.Controllers
{
    using System;
    using System.Data.Entity;
    using System.Net;
    using System.Web.Mvc;
    using System.Xml;
    using HolmesMVC.Enums;
    using HolmesMVC.Models;
    using HolmesMVC.Models.ViewModels;

    [OutputCache(Duration = 1, VaryByCustom = "LastDbUpdate")]
    public class StoryController : HolmesDbController
    {
        [AllowAnonymous]
        [HttpGet]
#pragma warning disable CA1822 // Mark members as static
        public string StoryXml(string storyCode)
#pragma warning restore CA1822 // Mark members as static
        {
            var uri = new Uri("https://appledore.azurewebsites.net/services/storyxmlretriever.svc/retrieve?storyCode=" + storyCode);

            var xr = new XmlDocument();
            xr.Load(WebRequest.Create(uri).GetResponse().GetResponseStream());

            return xr.DocumentElement.OuterXml;
        }

        [AllowAnonymous]
        public ActionResult Index()
        {
            return RedirectToActionPermanent("Index", "Canon");
        }
        
        [AllowAnonymous]
        public ActionResult Details(string id = null, int start = -1, int length = -1)
        {
            var story = Db.Stories.Find(id);
            if (story == null)
            {
                return HttpNotFound();
            }

            var model = new StoryView(story, start, length);

            if (model.ChunkXml != null)
            {
                return View("Chunk", model);
            }
            
            return View("Details", model);
        }


        //
        // GET: /Story/Edit/stud

        public ActionResult Edit(string id = "")
        {
            var story = Db.Stories.Find(id);
            if (story == null)
            {
                return HttpNotFound();
            }

            return View(story);
        }

        //
        // POST: /Story/Edit/stud

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Story story)
        {
            if (ModelState.IsValid)
            {
                Db.Entry(story).State = EntityState.Modified;
                Db.SaveChanges(); Shared.SomethingChanged(HttpContext.Application);

                return RedirectToRoute("Stories", new { controller = "Story", id = story.ID });
            }
            return View(story);
        }
    }
}