namespace HolmesMVC.Controllers
{
    using System.Linq;
    using System.Web.Mvc;

    using HolmesMVC.Models;
    using HolmesMVC.Models.ViewModels;
    using System;
    using System.Xml;
    using System.Web.Hosting;
    using System.Collections.Generic;
    using System.Xml.Linq;

    public class CanonController : HolmesDbController
    {
        //
        // GET: /Canon/

        [AllowAnonymous]
        public ActionResult Index()
        {
            var adapt =
                (from a in Db.Adaptations where a.Name == "Canon" select a)
                    .FirstOrDefault();
            var profile =
                (from p in Db.UserProfiles
                 where p.UserName == User.Identity.Name
                 select p).FirstOrDefault();

            if (null == profile)
            {
                profile = new UserProfile();
            }

            return View("Index", new CanonView(adapt, profile));
        }

        [AllowAnonymous]
        public ActionResult Search(string query)
        {
            CanonSearchView model = new CanonSearchView();

            if (query != null)
            {
                model.Query = query.ToLower();
                
                var xmlDoc = new XDocument();

                var storyUrl = HostingEnvironment.MapPath("~/Services/Stories/ALL.xml");

                if (storyUrl != null)
                {
                    xmlDoc = XDocument.Load(storyUrl);

                    model.Nodes = (from item in xmlDoc.Descendants("p")
                                   where item.Value.ToLower().Contains(model.Query)
                                   select new CanonSearchNode
                                   {
                                       Story = "STUD",
                                       Snippet = item.Value
                                   }
                                ).ToList();
                }
            }

            return View(model);
        }
    }
}
