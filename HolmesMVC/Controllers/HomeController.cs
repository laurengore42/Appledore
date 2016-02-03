namespace HolmesMVC.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Web.Configuration;
    using System.Web.Mvc;
    using System.Xml.Serialization;

    using HolmesMVC.BaconXml;
    using HolmesMVC.Models;
    using HolmesMVC.Models.ViewModels;

    [OutputCache(Duration = 1, VaryByCustom = "LastDbUpdate")]
    public class HomeController : HolmesDbController
    {
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
                HttpContext.Application["SearchDataFull"] = Shared.GetSearchDataFull(Db);
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

        [Stopwatch]
        [AllowAnonymous]
        public ActionResult Credits()
        {
            return View();
        }

        [Stopwatch]
        [AllowAnonymous]
        public ActionResult Toys()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public string BrettNum(string targetImdbName)
        {
            // check spelling of target
            targetImdbName = targetImdbName.Trim();

            var oldName = targetImdbName;
            if (!targetImdbName.Contains("(") &&
                BaconXmlTools.IntegerHolmesNumber("Jeremy Brett (I)", targetImdbName) == -2)
            {
                targetImdbName += " (I)";
            }

            var errorCode = BaconXmlTools.IntegerHolmesNumber("Jeremy Brett (I)", targetImdbName);
            if (errorCode == -2)
            {
                return "Could not find anyone called '" + oldName + "' in IMDb. You may need to add (I) or (II) to the name. Try searching: <a href='http://www.imdb.com/find?s=nm&q=" + oldName + "'>click here</a>";
            }

            if (errorCode == -1)
            {
                return "Unknown error during spellcheck!";
            }

            if (errorCode == -42)
            {
                return "According to the Oracle of Bacon, this actor CANNOT be linked to Jeremy Brett!";
            }

            return BaconXmlTools.BrettNumber(targetImdbName);
        }

        [AllowAnonymous]
        [HttpPost]
        public string HolmesNum(string targetImdbName)
        {
            var holmesId = Shared.GetHolmes(Db.Characters.ToList());
            var watsonId = Shared.GetWatson(Db.Characters.ToList());

            var stringOut = string.Empty;

            try
            {
                // see if they're in the actual database!
                var searchName = targetImdbName.ToLower();
                if (searchName.IndexOf("(") > -1)
                {
                    searchName = searchName.Substring(0,searchName.IndexOf("(") - 1);
                }

                var surname = searchName.Split(' ').Last().Trim();
                var forename = searchName.Replace(" " + surname, string.Empty).Trim();

                var dbActor = (from a in Db.Actors
                              where a.Forename.ToLower() == forename
                              && a.Surname.ToLower() == surname
                              select a).FirstOrDefault();
                if (null != dbActor)
                {
                    var actorName = Shared.ShortName(dbActor);

                    // they're in Appledore!
                    // are they a Holmes or a Watson?
                    var isAHolmes = (from ap in dbActor.Appearances
                                     where ap.Character == holmesId
                                     select ap).Any();
                    var isAWatson = (from ap in dbActor.Appearances
                                     where ap.Character == watsonId
                                     select ap).Any();

                    var dbApp = dbActor.Appearances.OrderBy(a => a.Episode1.Airdate).First();
                    var dbAdapt = dbApp.Episode1.Season1.Adaptation1;
                    var dbRename = Shared.CheckRename(dbApp);
                    var charName = dbRename ?? Shared.LongName(dbApp.Character1);

                    if (isAHolmes)
                    {
                        dbApp = (from ap in dbActor.Appearances
                                    where ap.Character == holmesId
                                 select ap).First();
                        dbAdapt = dbApp.Episode1.Season1.Adaptation1;
                        dbRename = Shared.CheckRename(dbApp);
                        charName = dbRename ?? Shared.LongName(dbApp.Character1);

                        stringOut = "Appledore has a page on <a href='/Actor/" + dbActor.ID + "'>" + actorName + "</a>!<br><br>";
                        stringOut += actorName + " played " + charName + " in '" + Shared.DisplayName(dbAdapt) + "'.";
                        stringOut += "<br><br>" + actorName + "'s Holmes number is 0.";
                        return stringOut;
                    }

                    if (isAWatson)
                    {
                        dbApp = (from ap in dbActor.Appearances
                                 where ap.Character == watsonId
                                 select ap).First();
                        dbAdapt = dbApp.Episode1.Season1.Adaptation1;
                        dbRename = Shared.CheckRename(dbApp);
                        charName = dbRename ?? Shared.LongName(dbApp.Character1);
                        stringOut = "Appledore has a page on <a href='/Actor/" + dbActor.ID + "'>" + actorName + "</a>!<br><br>";
                        stringOut += actorName + " played " + charName + " in '" + Shared.DisplayName(dbAdapt) + "', which starred <a href='/Actor/" + new AdaptView(dbAdapt).HolmesActors.First().ID + "'>" + Shared.ShortName(new AdaptView(dbAdapt).HolmesActors.First()) + "</a> as Sherlock Holmes.";
                        stringOut += "<br><br>" + actorName + "'s Holmes number is 1.";
                        return stringOut;
                    }

                    // Or if they're just some randomer.
                    stringOut = "Appledore has a page on <a href='/Actor/" + dbActor.ID + "'>" + actorName + "</a>!<br><br>";
                    stringOut += actorName + " played " + charName + " in '" + Shared.DisplayName(dbAdapt) + "', which starred <a href='/Actor/" + new AdaptView(dbAdapt).HolmesActors.First().ID + "'>" + Shared.ShortName(new AdaptView(dbAdapt).HolmesActors.First()) + "</a> as Sherlock Holmes.";
                    stringOut += "<br><br>" + actorName + "'s Holmes number is 1.";
                    return stringOut;
                }

                // if they're not in the database, we'll need to hit the Oracle
                // check spelling of target
                targetImdbName = targetImdbName.Trim();

                var oldName = targetImdbName;
                if (!targetImdbName.Contains("(")
                    && BaconXmlTools.IntegerHolmesNumber(
                        "Jeremy Brett (I)",
                        targetImdbName) == -2)
                {
                    targetImdbName += " (I)";
                }

                var errorCode = BaconXmlTools.IntegerHolmesNumber(
                    "Jeremy Brett (I)",
                    targetImdbName);
                if (errorCode == -2)
                {
                    return "Could not find anyone called '" + oldName
                           + "' in IMDb. You may need to add (I) or (II) to the name. Try searching: <a href='http://www.imdb.com/find?s=nm&q="
                           + oldName + "'>click here</a>";
                }

                if (errorCode == -1)
                {
                    return "Unknown error during spellcheck!";
                }

                // get holmes list
                var holmeses = (from a in Db.Appearances
                                where
                                    a.Character == holmesId
                                    && a.Actor1.IMDbName != null
                                select new ToyHolmes { ID = a.Actor, Name = a.Actor1.IMDbName }).Distinct().ToList();

                var r = new Random();
                holmeses = holmeses.OrderBy(x => r.Next()).ToList();

                // spelling passed, calculate number
                var holmesNum = 1000000;
                var count = holmeses.Count();
                string holmesStr = string.Empty;

                for (int i = 0; i < count; i++)
                {
                    var holmesTesting = holmeses[i];

                    var thisHolmesNum = BaconXmlTools.IntegerHolmesNumber(
                        holmesTesting.Name,
                        targetImdbName);

                    if (thisHolmesNum == -2 && !holmesTesting.Name.Contains("("))
                    {
                        holmesTesting.Name += " (I)";
                        thisHolmesNum = BaconXmlTools.IntegerHolmesNumber(
                            holmesTesting.Name,
                            targetImdbName);
                    }

                    if (thisHolmesNum < 0)
                    {
                        // awards show kludge
                        if (thisHolmesNum == -10)
                        {
                            continue;
                        }

                        // genuinely unlinkable actors
                        if (thisHolmesNum == -42)
                        {
                            continue;
                        }

                        return stringOut
                            + "Got a " + thisHolmesNum
                               + " result when testing holmes " + i + ": "
                               + holmesTesting + ".";
                    }

                    if (thisHolmesNum < holmesNum)
                    {
                        holmesStr = BaconXmlTools.HolmesNumber(
                            holmesTesting.ID,
                            holmesTesting.Name,
                            targetImdbName);
                        holmesNum = thisHolmesNum;
                    }

                    if (holmesNum == 1)
                    {
                        break;
                    }
                }

                if (holmesStr == string.Empty)
                {
                    holmesStr = "I was unable to link this actor to ANY of the Sherlock Holmes actors known to Appledore. This is very rare - well done!";
                }

                return stringOut + holmesStr;
            }
            catch (Exception e)
            {
                if (e.InnerException == null)
                {
                    return "Error during calculation: " + e.Message
                           + e.StackTrace;
                }

                return "Error during calculation: " + e.Message + "¦¦¦" + e.InnerException.Message + e.StackTrace;
            }
        }

        [Stopwatch]
        public ActionResult Analytics()
        {
            var holmesId = Shared.GetHolmes(Db.Characters.ToList());

            ViewBag.actCount = Db.Actors.Count();
            ViewBag.holCount =
                Db.Appearances.Where(a => a.Character == holmesId)
                    .GroupBy(a => a.Actor)
                    .Count();

            ViewBag.fadaCount =
                Db.Episodes.Count(e => e.Season1.Adaptation1.Medium1.Name == "Film");
            ViewBag.tadaCount =
                Db.Adaptations.Count(a => a.Medium1.Name == "Television");
            ViewBag.radaCount =
                Db.Adaptations.Count(a => a.Medium1.Name == "Radio");

            ViewBag.chaCount = Db.Characters.Count();
            ViewBag.canCount = Db.Stories.Count();

            ViewBag.canChaCount = (from a in Db.Appearances
                                   where a.Episode1.Season1.Adaptation1.Name == "Canon"
                                   select a.Character).Distinct().Count();
            
            return View();
        }

        [HttpPost]
        public ActionResult DataEntry(int epId, string imdbInput)
        {
            // split into lines
            var imdbSplit = imdbInput.Split(new[] { "\r\n" }, StringSplitOptions.None);

            // recombine into actor-char pairs
            var imdbPairs = new List<string>();
            for (int i = 0; i < imdbSplit.Count(); i += 2)
            {
                imdbPairs.Add(imdbSplit[i] + imdbSplit[i + 1]);
            }

            // trim IMDb's whitespace and formatting
            var imdbTrimmedPairs = new List<string>();
            foreach (var pair in imdbPairs)
            {
                var encodedPair = WebUtility.UrlEncode(pair);
                encodedPair = encodedPair.Substring(encodedPair.IndexOf("%09"));
                encodedPair = encodedPair.Replace("%09", string.Empty);
                encodedPair = encodedPair.Replace("...", "¦");
                encodedPair = WebUtility.UrlDecode(encodedPair);
                imdbTrimmedPairs.Add(encodedPair);
            }

            // we are now left with e.g. 'Ben Syder¦Sherlock Holmes'
            // statistically characters and actors are usually New,
            // so create them
            // we already have a mechanism for fixing dupes here
            foreach (var pair in imdbTrimmedPairs)
            {
                var actor = pair.Split('¦')[0];
                var character = pair.Split('¦')[1];

                List<string> actorNames = actor.Split(' ').ToList();
                var actorSurname = actorNames.Last();
                actorNames.RemoveAt(actorNames.Count() - 1);
                var actorForename = string.Join(" ", actorNames);
                var actorId = ActorCreateShort(actorForename, actorSurname);

                List<string> charNames = character.Split(' ').ToList();
                var charSurname = charNames.Last();
                charNames.RemoveAt(charNames.Count() - 1);
                var charForename = string.Join(" ", charNames);
                var charId = CharCreateShort(charForename, charSurname);

                // Right, we now have the actor and character IDs.
                // Create an appearance!
                Db.Appearances.Add(new Appearance
                {
                    Character = charId,
                    Character1 = Db.Characters.Find(charId),
                    Actor = actorId,
                    Actor1 = Db.Actors.Find(actorId),
                    Episode = epId,
                    Episode1 = Db.Episodes.Find(epId)
                });
                Db.SaveChanges();
            }

            Shared.SomethingChanged(HttpContext.Application);

            return RedirectToAction("Details", "Episode", new { id = epId });
        }

        [Stopwatch]
        [HttpGet]
        public ViewResult DataEntry()
        {
            var dupeCharNames = (from c in Db.Characters
                                 where c.ID > 0
                                 orderby c.ID
                                 group c by (c.Honorific == null ? string.Empty : (c.Honorific1.Name + " ")) + (c.Forename == null || c.Forename.Length < 1 ? string.Empty : c.Forename + " ") + c.Surname
                                     into grp
                                     where grp.Count() > 1
                                     select grp).ToList();
            if (dupeCharNames.Any())
            {
                ViewBag.dupeCharNames = dupeCharNames;
            }


            var dupeActNames = (from a in Db.Actors
                                where a.ID > 0
                                orderby a.ID
                                // don't use middle names, IMDb auto input misses them out
                                group a by (a.Forename ?? string.Empty) + " " + a.Surname
                                    into grp
                                    where grp.Count() > 1
                                    select grp).ToList();
            // Need something which says
            // if they both have IMDb entries, and they're DIFFERENT,
            // they're not dupes
            // i.e. John Carr 1746979 and John Carr 6454142
            var dupeActNamesForEnumeration = new List<IGrouping<String, Actor>>();
            dupeActNamesForEnumeration.AddRange(dupeActNames);
            foreach (var dupe in dupeActNamesForEnumeration)
            {
                var mainActor = dupe.First();
                var mainActorIMDb = mainActor.IMDb;
                if (mainActorIMDb != "") {
                    var valid = false;
                    foreach (var possible in dupe)
                    {
                        if (possible.ID != mainActor.ID && (possible.IMDb == null || possible.IMDb == "" || possible.IMDb == mainActorIMDb))
                        {
                            valid = true;
                            break;
                        }
                    }
                    if (!valid)
                    {
                        dupeActNames.Remove(dupe);
                    }
                }

            }
            if (dupeActNames.Any())
            {
                ViewBag.dupeActNames = dupeActNames;
            }

            return View();
        }

        [Stopwatch]
        [HttpGet]
        public ViewResult Unlocked()
        {
            var unlockedCanonActors = (from a in Db.Actors
                                       where a.ID > 0
                                       && a.IMDb == null
                                       && a.Surname != "extra"
                                       && (from ap in a.Appearances
                                           where
                                               (from ap2 in ap.Character1.Appearances
                                                    where ap2.Episode1.Season1.Adaptation1.Name != null
                                                    && ap2.Episode1.Season1.Adaptation1.Name == "Canon"
                                                    select ap2.ID).Any()
                                               && !ap.Actor1.Surname.Contains("the dog")
                                               && (ap.Episode1.Season1.Adaptation1.Medium1.Name == "Television"
                                                   || ap.Episode1.Season1.Adaptation1.Medium1.Name == "Film")
                                           select ap).Any()
                                       select a).ToList();

            var rand = (new Random()).Next(unlockedCanonActors.Count() - 1);
            ViewBag.unlockedActor = unlockedCanonActors[rand];
            ViewBag.unlockedActorCount = unlockedCanonActors.Count();

            var holmesId = Shared.GetHolmes(Db.Characters.ToList());

            var unlockedHolmeses = (from a in unlockedCanonActors
                                    where (from ap in a.Appearances
                                           where
                                               ap.Character == holmesId
                                               && ap.Episode1.Season1.Adaptation1
                                                      .Medium1.Name != "Stage" // to_do_theatre
                                           select ap).Any()
                                    select a).ToList();

            if (unlockedHolmeses.Any())
            {
                rand = (new Random()).Next(unlockedHolmeses.Count() - 1);
                ViewBag.unlockedHolmes = unlockedHolmeses[rand];
                ViewBag.unlockedHolmesCount = unlockedHolmeses.Count();
            }

            var unlockedFilms = (from e in Db.Episodes
                                 where 
                                 e.Season1.Adaptation1.Medium1.Name != "Stage" // to_do_theatre
                                 && e.Season1.Adaptation1.Seasons.SelectMany(s => s.Episodes).Count() == 1
                                 && !(from a in e.Appearances
                                      where a.Actor == 0 && a.Character == 0
                                      select a.ID).Any()
                                 select e).ToList();
            if (unlockedFilms.Any())
            {
                rand = (new Random()).Next(unlockedFilms.Count() - 1);
                ViewBag.unlockedFilm = unlockedFilms[rand];
                ViewBag.unlockedFilmCount = unlockedFilms.Count();
            }
            else
            {
                var unlockedEpisodes = (from e in Db.Episodes
                                       where
                                           e.Season1.Adaptation1.Medium1.Name != "Stage" // to_do_theatre
                                           && !(from a in e.Appearances
                                                where a.Actor == 0 && a.Character == 0
                                                select a.ID).Any()
                                       select e).ToList();

                rand = (new Random()).Next(unlockedEpisodes.Count() - 1);
                ViewBag.unlockedEpisode = unlockedEpisodes[rand];
                ViewBag.unlockedEpisodeCount = unlockedEpisodes.Count();
            }

            return View();
        }

        [Stopwatch]
        [HttpGet]
        public ViewResult Scraps()
        {
            var holmesId = Shared.GetHolmes(Db.Characters.ToList());

            var fullHolmesList = (from app in Db.Appearances
                                  where app.Character == holmesId
                                  && app.Actor > 0
                                  && app.Actor1.Pic != null
                                  && app.Actor1.Pic != ""
                                  group app by app.Actor into grp
                                  orderby grp.FirstOrDefault().Episode1.Airdate
                                  select grp.FirstOrDefault().Actor1).ToList();

            return View(fullHolmesList);
        }

        [Stopwatch]
        [AllowAnonymous]
        public ViewResult Index()
        {
            ViewBag.lastdbupdate = HttpContext.Application["LastDbUpdate"];
            ViewBag.lastgotsearchdata = HttpContext.Application["LastGotSearchData"];
            return View();
        }

        [AllowAnonymous]
        public PartialViewResult RandomFact()
        {
            return PartialView(new RandomView(Db));
        }

        [AllowAnonymous]
        public ActionResult DatabaseLinks(string table, int id)
        {
            ViewBag.Table = table;
            ViewBag.ID = id;

            if (ViewBag.Table == "Episode")
            {
                int epId = ViewBag.ID;
                ViewBag.adaptId = (from e in Db.Episodes
                                   where e.ID == epId
                                   select e.Season1.Adaptation).FirstOrDefault();
            }

            return PartialView();
        }

        [AllowAnonymous]
        public PartialViewResult Menu()
        {
            // the menu at the top of the main layout

            // get the tables for the Data links
            var holmesConn = WebConfigurationManager.ConnectionStrings["HolmesDBEntities"].ConnectionString;
            var tables = new DataTable("Tables");
            using (var conn = new SqlConnection(holmesConn))
            {
                using (var comm = conn.CreateCommand())
                {
                    comm.CommandText = "select table_name as Name from INFORMATION_SCHEMA.Tables where TABLE_TYPE = 'BASE TABLE'";
                    conn.Open();
                    tables.Load(comm.ExecuteReader(CommandBehavior.CloseConnection));
                }
            }

            var pagenames = new List<string>();

            // manual exceptions from the Data links
            var nope = new List<string>
                           {
                               // this is for tables you aren't building an index page for
                               // because they're accessible in a better way
                               "actors",
                               "characters",
                               "appearances",
                               "adaptations",
                               "episodes",
                               "genders",
                               "renames",
                               "seasons",
                               "species",
                               "stories",

                               // the dates used for canon stories don't have a page
                               "date",

                               // this leaves:
                               // Honorific, Medium, Reference

                               // also don't link to the membership tables
                               // which shouldn't be in this database, but, Azure
                               "userprofile",
                               "webpages_membership",
                               "webpages_oauthmembership",
                               "webpages_roles",
                               "webpages_usersinroles",

                               // or the code-first migrations table
                               "__migrationhistory"
                           };

            foreach (DataRow row in tables.Rows)
            {
                var tablename = row[0].ToString().ToLower();
                if (nope.Contains(tablename.ToLower()))
                {
                    continue;
                }

                var addname = tablename.EndsWith("s") ? tablename.Substring(0, tablename.Length - 1) : tablename;

                switch (addname)
                {
                    case "media":
                        addname = "medium";
                        break;
                    case "specie":
                        addname = "species";
                        break;
                    case "storie":
                        addname = "story";
                        break;
                }

                var myTextInfo = new CultureInfo("en-GB").TextInfo;

                pagenames.Add(myTextInfo.ToTitleCase(addname));
            }

            pagenames.Sort();
            ViewBag.TablesForData = pagenames;

            return this.PartialView();
        }

        private int ActorCreateShort(string forename, string surname)
        {
            var actor = new Actor
            {
                Forename = forename,
                Surname = surname
            };

            Db.Actors.Add(actor);
            Db.SaveChanges();

            return actor.ID;
        }

        private int CharCreateShort(string forename, string surname)
        {
            var character = new Character
            {
                Forename = forename,
                Surname = surname
            };

            Db.Characters.Add(character);
            Db.SaveChanges();

            return character.ID;
        }
    }
}
