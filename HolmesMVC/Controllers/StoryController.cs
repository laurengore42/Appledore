namespace HolmesMVC.Controllers
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Web.Mvc;
    using System.Xml;

    using HolmesMVC.Models.ViewModels;

    public class StoryController : HolmesDbController
    {
        [AllowAnonymous]
        [HttpGet]
        public string StoryXml(string storyCode)
        {
            var uri = new Uri("http://appledore.azurewebsites.net/services/storyxmlretriever.svc/retrieve?storyCode=" + storyCode);

            var xr = new XmlDocument();
            xr.Load(WebRequest.Create(uri).GetResponse().GetResponseStream());

            return xr.DocumentElement.OuterXml;
        }

        [AllowAnonymous]
        public ActionResult Index()
        {
            return RedirectToAction("Index", "Canon");
        }
        
        [Stopwatch]
        [AllowAnonymous]
        public ActionResult Details(string id = null, int start = -1, int length = -1)
        {
            var story = Db.Stories.Find(id);
            if (story == null)
            {
                return HttpNotFound();
            }

            var model = new StoryView(story);

            if (start >= 0 && length >= 0)
            {
                model.ChunkStart = start;
                model.ChunkLength = length;

                return View("Chunk", model);
            }

            ViewBag.HolmesId = Shared.GetHolmes(Db.Characters.ToList());
            return View("Details", model);
        }
    }
}