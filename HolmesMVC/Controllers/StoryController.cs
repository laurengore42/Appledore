namespace HolmesMVC.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Net;
    using System.Web.Mvc;
    using System.Web.Routing;
    using System.Xml;
    using HolmesMVC.Enums;
    using HolmesMVC.Models;
    using HolmesMVC.Models.ViewModels;

    [OutputCache(Duration = 2628000, VaryByCustom = "LastDbUpdate")]
    public class StoryController : HolmesDbController
    {
        [AllowAnonymous]
        [HttpGet]
        public string StoryXml(string storyCode)
        {
            var uri = new Uri("https://appledore.azurewebsites.net/services/storyservice/storyxmlretriever.svc/retrieve?storyCode=" + storyCode);

            var xr = new XmlDocument();
            xr.Load(WebRequest.Create(uri).GetResponse().GetResponseStream());

            return xr.DocumentElement.OuterXml;
        }

        [AllowAnonymous]
        [HttpGet]
        public string StoryAdapteds(string storyCode)
        {
            var ID = storyCode.ToUpper();
            var story = Db.Stories.Find(ID);

            // these episodes are all the adaptations of that story
            var adapteds = (from e in story.Episodes
                            where
                                (null == e.Season.Adaptation.Name
                                    || e.Season.Adaptation.Name != "Canon")
                                && e.Season.Adaptation.Medium != (int)Medium.Stage // to_do_theatre
                            select e).OrderBy(e => e.Airdate).ToList();

            var returnString = "";

            returnString += "<ul>";
            foreach (Episode e in adapteds)
            {
                var actorHolmes = e.Season.Adaptation.PlayedBy(CanonCharacter.Holmes).First();
                returnString += "<li>";
                returnString += e.Airdate.ToString("yyyy");
                returnString += ": ";
                returnString += HtmlHelper.GenerateRouteLink(HttpContext.Request.RequestContext, RouteTable.Routes, "'" + e.DisplayName + "'", "EpDetails", new RouteValueDictionary(new { adaptWord = e.Season.Adaptation.MediumUrlName, adaptName = e.Season.Adaptation.UrlName, seasonNumber = e.Season.AirOrder, episodeNumber = e.AirOrder }), null);
                returnString += ",";
                if (!string.IsNullOrEmpty(e.Translation))
                {
                    returnString += "<span style=\"font-weight: normal; font-size: small;\"><i>";
                    returnString += e.Translation;
                    returnString += "</i></span>";
                }
                returnString += " starring ";
                returnString += HtmlHelper.GenerateLink(HttpContext.Request.RequestContext, RouteTable.Routes, actorHolmes.ShortName, "ActorDetails", "Details", "Actor", new RouteValueDictionary(new { actorHolmes.UrlName }), null);
                returnString += "</li>";
            }
            returnString += "</ul>";

            return returnString;
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
        [OutputCache(NoStore = true, Duration = 0)]
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
                Db.SaveChanges();

                return RedirectToRoute("Stories", new { controller = "Story", id = story.ID });
            }
            return View(story);
        }
    }
}