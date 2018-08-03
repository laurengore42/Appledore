namespace HolmesMVC.Controllers
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Web.Mvc;
    using System.Xml.Serialization;
    using HolmesMVC.Models;
    using HolmesMVC.Models.ViewModels;

    [SessionState(System.Web.SessionState.SessionStateBehavior.Disabled)]
    [OutputCache(Duration = 1, VaryByCustom = "LastDbUpdate")]
    public class SearchController : HolmesDbController
    {
        [AllowAnonymous]
        public static string GetSearchDataFull(HolmesDBEntities Db)
        {
            var sr = new StringWriter();
            var sd = new SearchData();
            sd.Populate(Db);
            (new XmlSerializer(typeof(SearchData))).Serialize(sr, sd);

            return sr.ToString();
        }

        [AllowAnonymous]
        public ContentResult DeserialiseSearchStringForPartial()
        {
            if (HttpContext.Application["SearchDataShort"] != null)
            {
                return Content(HttpContext.Application["SearchDataShort"].ToString());
            }

            string searchDataFull = (HttpContext.Application["SearchDataFull"] == null)
                ? string.Empty
                : HttpContext.Application["SearchDataFull"].ToString();

            while (searchDataFull.Length < 1)
            {
                HttpContext.Application["SearchDataFull"] = GetSearchDataFull(Db);
                searchDataFull = HttpContext.Application["SearchDataFull"].ToString();
            }

            var sd = (SearchData)new XmlSerializer(typeof(SearchData)).Deserialize(new StringReader(searchDataFull));

            var siList = new List<string>();

            siList.AddRange(from s in sd.Stories select s.Name);
            siList.AddRange(from a in sd.Actors select (string.IsNullOrWhiteSpace(a.Forename) ? string.Empty : a.Forename + " ") + a.Surname);
            siList.AddRange(from c in sd.Characters select (string.IsNullOrWhiteSpace(c.Honorific) ? string.Empty : c.Honorific + " ") + (string.IsNullOrWhiteSpace(c.Forename) ? string.Empty : c.Forename + " ") + c.Surname);
            siList.AddRange(from r in sd.Renames select (string.IsNullOrWhiteSpace(r.Forename) ? string.Empty : r.Forename + " ") + r.Surname);
            siList.AddRange(from e in sd.Episodes select e.Name);
            siList.AddRange(from e in sd.Episodes select e.Translation);
            siList.AddRange(from a in sd.Adaptations select a.Name);
            siList.AddRange(from a in sd.Adaptations select a.Translation);

            siList = siList.Distinct().ToList();
            siList.Sort();

            siList = (from s in siList select s.Replace("\"", "\\" + "\"")).ToList();
            HttpContext.Application["SearchDataShort"] = string.Join("¦", siList);

            return Content(HttpContext.Application["SearchDataShort"].ToString());
        }

        [AllowAnonymous]
        public ActionResult Index(string query)
        {
            // rare case where the application gets reset during a query
            if (HttpContext.Application["SearchDataFull"] == null)
            {
                HttpContext.Application["SearchDataFull"] = GetSearchDataFull(Db);
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
