namespace HolmesMVC.Controllers
{
    using System.IO;
    using System.Linq;
    using System.Web.Mvc;
    using System.Xml.Serialization;

    using HolmesMVC.Models.ViewModels;

    [OutputCache(Duration = 1, VaryByCustom = "LastDbUpdate")]
    public class SearchController : HolmesDbController
    {
        [AllowAnonymous]
        public ActionResult Index(string query)
        {
            // rare case where the application gets reset during a query
            if (HttpContext.Application["SearchDataFull"] == null)
            {
                HttpContext.Application["SearchDataFull"] = Shared.GetSearchDataFull(Db);
            }

            var results = (SearchData)new XmlSerializer(typeof(SearchData)).Deserialize(new StringReader(HttpContext.Application["SearchDataFull"].ToString()));

            // search the data and get the results
            results.DoSearch(query);
            
            // the single-result redirect
            if (results.Stories.Count + results.Actors.Count + results.Characters.Count + results.Episodes.Count + results.Adaptations.Count == 1)
            {
                if (results.Stories.Any())
                {
                    return RedirectToAction("Details", "Story", new { id = results.Stories.First().ID });
                }

                if (results.Actors.Any())
                {
                    return RedirectToAction("Details", "Actor", new { id = results.Actors.First().ID });
                }

                if (results.Characters.Any())
                {
                    return RedirectToAction("Details", "Character", new { id = results.Characters.First().ID });
                }

                if (results.Episodes.Any())
                {
                    return RedirectToAction("Details", "Episode", new { id = results.Episodes.First().ID });
                }

                if (results.Adaptations.Any())
                {
                    return RedirectToAction("Details", "Adaptation", new { id = results.Adaptations.First().ID });
                }
            }
            
            // if result count is anything other than 1
            // or there are renames
            return View(results);
        }
    }
}
