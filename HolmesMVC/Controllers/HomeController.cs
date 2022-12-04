﻿namespace HolmesMVC.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Globalization;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Web.Configuration;
    using System.Web.Mvc;
    using System.Xml.Linq;
    using HolmesMVC.ActionResults;
    using HolmesMVC.Enums;
    using HolmesMVC.Extensions;
    using HolmesMVC.Models;
    using HolmesMVC.Models.ViewModels;

    [OutputCache(Duration = 3600, VaryByCustom = "LastDbUpdate")]
    public class HomeController : HolmesDbController
    {
        [AllowAnonymous]
        public ActionResult Credits()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult SitemapXml()
        {
            ViewBag.Actors = Db.Actors;
            ViewBag.Adaptations = Db.Adaptations;
            ViewBag.Characters = Db.Characters;

            XNamespace ns = @"http://www.sitemaps.org/schemas/sitemap/0.9";
            XDocument xmlDoc = new XDocument(
                new XDeclaration("1.0", "utf-8", "true")
            );
            var root = new XElement(ns + "urlset");
            root.Add(
                new XElement(ns + "url",
                new XElement(ns + "loc", new XText(Url.Action("Index", "Home", null, "https")))
                ),
                new XElement(ns + "url",
                new XElement(ns + "loc", new XText(Url.Action("Search", "Canon", null, "https")))
                ),
                new XElement(ns + "url",
                new XElement(ns + "loc", new XText(Url.Action("Index", "Adaptation", null, "https")))
                ),
                new XElement(ns + "url",
                new XElement(ns + "loc", new XText(Url.Action("HolmesNumToy", "Toys", null, "https")))
                ),
                new XElement(ns + "url",
                new XElement(ns + "loc", new XText(Url.Action("PhotoCollageToy", "Toys", null, "https")))
                ),
                new XElement(ns + "url",
                new XElement(ns + "loc", new XText(Url.Action("MapPinToy", "Toys", null, "https")))
                ),
                new XElement(ns + "url",
                new XElement(ns + "loc", new XText(Url.Action("MultiTimelineToy", "Toys", null, "https")))
                ),
                new XElement(ns + "url",
                new XElement(ns + "loc", new XText(Url.Action("Credits", "Home", null, "https")))
                )
            );
            foreach(var a in Db.Actors.Where(a => a.ID > 0))
            {
                root.Add(
                    new XElement(ns + "url",
                    new XElement(ns + "loc", new XText(Url.Action("Details", "Actor", new { a.UrlName }, "https")))
                    )
                );
            }
            foreach (var c in Db.Characters.Where(c => c.ID > 0))
            {
                root.Add(
                    new XElement(ns + "url",
                    new XElement(ns + "loc", new XText(Url.Action("Details", "Character", new { c.UrlName }, "https")))
                    )
                );
            }
            foreach (var a in Db.Adaptations)
            {
                if (a.UrlName != "canon")
                {
                    var actionName = "Details";
                    switch (a.MediumUrlName)
                    {
                        case "radio":
                            actionName = "RadioDetails";
                            break;
                        case "film":
                            actionName = "FilmDetails";
                            break;
                        case "tv":
                            actionName = "TVDetails";
                            break;
                    }
                    root.Add(
                        new XElement(ns + "url",
                        new XElement(ns + "loc", new XText(Url.Action(actionName, "Adaptation", new { a.UrlName }, "https")))
                        )
                    );
                    if (a.Seasons.SelectMany(s => s.Episodes).Count() > 1)
                    {
                        foreach (var s in a.Seasons)
                        {
                            foreach (var e in s.Episodes)
                            {
                                root.Add(
                                    new XElement(ns + "url",
                                    new XElement(ns + "loc", new XText(Url.RouteUrl("EpDetails", new { adaptWord = e.Season.Adaptation.MediumUrlName, adaptName = e.Season.Adaptation.UrlName, seasonNumber = e.Season.AirOrder, episodeNumber = e.AirOrder }, "https")))
                                    )
                                );
                            }
                        }
                    }
                }
                else
                {
                    root.Add(
                        new XElement(ns + "url",
                        new XElement(ns + "loc", new XText(Url.Action("Index", "Canon", null, "https")))
                        )
                    );
                    foreach (var s in a.Seasons)
                    {
                        foreach (var e in s.Episodes)
                        {
                            root.Add(
                                new XElement(ns + "url",
                                new XElement(ns + "loc", new XText(Url.Action("Details", "Story", new { id = e.StoryID }, "https")))
                                )
                            );
                        }
                    }
                }
            }
            xmlDoc.Add(root);

            return new XmlActionResult(xmlDoc);
        }

        [AllowAnonymous]
        public ActionResult Sitemap()
        {
            ViewBag.Actors = Db.Actors;
            ViewBag.Adaptations = Db.Adaptations;
            ViewBag.Characters = Db.Characters;
            return View();
        }

        public ActionResult Analytics()
        {
            ViewBag.actCount = Db.Actors.Count();
            ViewBag.holCount =
                Db.Appearances.Where(a => a.CharacterID == (int)CanonCharacter.Holmes)
                    .GroupBy(a => a.ActorID)
                    .Count();

            ViewBag.fadaCount =
                Db.Episodes.Count(e => e.Season.Adaptation.Medium == (int)Medium.Film);
            ViewBag.tadaCount =
                Db.Adaptations.Count(a => a.Medium == (int)Medium.Television);
            ViewBag.radaCount =
                Db.Adaptations.Count(a => a.Medium == (int)Medium.Radio);

            ViewBag.chaCount = Db.Characters.Count();
            ViewBag.canCount = Db.Stories.Count();

            ViewBag.canChaCount = (from a in Db.Appearances
                                   where a.Episode.Season.Adaptation == Db.Adaptations.Canon()
                                   select a.CharacterID).Distinct().Count();
            
            return View();
        }

        [HttpPost]
        public ActionResult DataEntry(int epId, string imdbInput)
        {
            // kill IMDb cruft
            imdbInput = imdbInput.Replace(" (credit only)", "");
            imdbInput = imdbInput.Replace(" (uncredited)", "");
            imdbInput = imdbInput.Replace(" (voice)", "");
            imdbInput = imdbInput.Replace(" (archive footage)", "");
            imdbInput = imdbInput.Replace("Rest of cast listed alphabetically:", "");
            Regex asPattern = new Regex(@" \(as .*\)");
            imdbInput = asPattern.Replace(imdbInput, "");
            Regex numberedPattern = new Regex(@" #\d+");
            imdbInput = numberedPattern.Replace(imdbInput, "");

            // split into lines
            var imdbSplit = imdbInput.Split(new[] { "\r\n" }, StringSplitOptions.None);

            // recombine into actor-char pairs
            var imdbPairs = new List<string>();
            for (int i = 0; i < imdbSplit.Count(); i++)
            {
                var imdbSubSplit = imdbSplit[i].Split(new[] { "\t" }, StringSplitOptions.None);
                var imdbPair = imdbSubSplit[0];
                if (imdbSubSplit.Length >= 4)
                {
                    imdbPair += "¦" + imdbSubSplit[3];
                }
                if (!string.IsNullOrWhiteSpace(imdbPair)) {
                    imdbPairs.Add(imdbPair);
                }
            }

            // we are now left with e.g. 'Ben Syder¦Sherlock Holmes'
            // statistically characters and actors are usually New,
            // so create them
            // we already have a mechanism for fixing dupes here
            for (int i = 0; i < imdbPairs.Count; i++)
            {
                var pair = imdbPairs[i];

                var actor = pair.Split('¦')[0];
                var character = pair.Split('¦')[1];

                List<string> actorNames = actor.Split(' ').ToList();
                var actorSurname = actorNames.Last();
                actorNames.RemoveAt(actorNames.Count() - 1);
                var actorForename = string.Join(" ", actorNames);
                var actorId = ActorCreateShort(actorForename, actorSurname, i);

                List<string> charNames = character.Split(' ').ToList();
                var charSurname = charNames.Last();
                charNames.RemoveAt(charNames.Count() - 1);
                var charForename = string.Join(" ", charNames);
                var charId = CharCreateShort(charForename, charSurname, i);

                // Right, we now have the actor and character IDs.
                // Create an appearance!
                Db.Appearances.Add(new Appearance
                {
                    CharacterID = charId,
                    Character = Db.Characters.Find(charId),
                    ActorID = actorId,
                    Actor = Db.Actors.Find(actorId),
                    EpisodeID = epId,
                    Episode = Db.Episodes.Find(epId)
                });
                Db.SaveChanges();
            }

            var episode = Db.Episodes.Find(epId);
            return RedirectToRoute("EpDetails", new { adaptWord = episode.Season.Adaptation.MediumUrlName, adaptName = episode.Season.Adaptation.UrlName, seasonNumber = episode.Season.AirOrder, episodeNumber = episode.AirOrder });
        }

        [HttpGet]
        public ViewResult DataEntry(int? epId)
        {
            var dupeCharNames = (from c in Db.Characters
                                 where c.ID > 0 && c.StoryID == null
                                 orderby c.ID
                                 group c by (c.HonorificID == null ? string.Empty : (c.Honorific.Name + " ")) + (c.Forename == null || c.Forename.Length < 1 ? string.Empty : c.Forename + " ") + c.Surname
                                     into grp
                                     where grp.Count() > 1
                                     select grp).ToList();
            if (dupeCharNames.Any())
            {
                ViewBag.dupeCharNames = dupeCharNames;
            }

            var titleForenames = (from c in Db.Characters
                                  join h in Db.Honorifics on 1 equals 1
                                  where c.ID > 0 && c.HonorificID == null && !string.IsNullOrEmpty(c.Forename) && c.Forename.StartsWith(h.Name + " ")
                                  orderby c.ID
                                  select c)
                                  .Union
                                 (from c in Db.Characters
                                  join h in Db.Honorifics on 1 equals 1
                                  where c.ID > 0 && c.HonorificID == null && !string.IsNullOrEmpty(c.Forename) && c.Forename == h.Name
                                  orderby c.ID
                                  select c)
                                  .ToList();
            if (titleForenames.Any())
            {
                ViewBag.titleForenames = titleForenames;
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
                if (!string.IsNullOrEmpty(mainActorIMDb)) {
                    var valid = false;
                    foreach (var possible in dupe)
                    {
                        if (possible.ID != mainActor.ID && (string.IsNullOrEmpty(possible.IMDb) || possible.IMDb == mainActorIMDb))
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

            if (epId != null)
            {
                ViewBag.epId = epId;
            }

            return View();
        }

        [HttpGet]
        public ViewResult Unlocked()
        {
            var unlockedCanonActors = (from a in Db.Actors
                                       where a.ID > 0
                                       && a.IMDb == null
                                       && a.Surname != "extra"
                                       && (from ap in a.Appearances
                                           where
                                               (from ap2 in ap.Character.Appearances
                                                    where ap2.Episode.Season.Adaptation.Name == "Canon"
                                                    select ap2.ID).Any()
                                               && !ap.Actor.Surname.Contains("the dog")
                                               && (ap.Episode.Season.Adaptation.Medium == (int)Medium.Television
                                                   || ap.Episode.Season.Adaptation.Medium == (int)Medium.Film)
                                           select ap).Any()
                                       select a).ToList();

            var rand = (new Random()).Next(unlockedCanonActors.Count() - 1);
            ViewBag.unlockedActor = unlockedCanonActors[rand];
            ViewBag.unlockedActorCount = unlockedCanonActors.Count();
            
            var unlockedHolmeses = (from a in unlockedCanonActors
                                    where (from ap in a.Appearances
                                           where
                                               ap.CharacterID == (int)CanonCharacter.Holmes
                                               && ap.Episode.Season.Adaptation
                                                      .Medium != (int)Medium.Stage // to_do_theatre
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
                                 e.Season.Adaptation.Medium != (int)Medium.Stage // to_do_theatre
                                 && e.Season.Adaptation.Seasons.SelectMany(s => s.Episodes).Count() == 1
                                 && !(from a in e.Appearances
                                      where a.ActorID == 0 && a.CharacterID == 0
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
                var unlockedNonRadioEpisodes = (from e in Db.Episodes
                                       where
                                           e.Season.Adaptation.Medium != (int)Medium.Stage // to_do_theatre
                                           && e.Season.Adaptation.Medium != (int)Medium.Radio
                                           && !(from a in e.Appearances
                                                where a.ActorID == 0 && a.CharacterID == 0
                                                select a.ID).Any()
                                       select e).ToList();

                if (unlockedNonRadioEpisodes.Any())
                {
                    rand = (new Random()).Next(unlockedNonRadioEpisodes.Count() - 1);
                    ViewBag.unlockedEpisode = unlockedNonRadioEpisodes[rand];
                    ViewBag.unlockedEpisodeCount = unlockedNonRadioEpisodes.Count();
                }
                else
                {
                    var unlockedRadioEpisodes = (from e in Db.Episodes
                                                    where
                                                        e.Season.Adaptation.Medium == (int)Medium.Radio
                                                        && !(from a in e.Appearances
                                                             where a.ActorID == 0 && a.CharacterID == 0
                                                             select a.ID).Any()
                                                    select e).ToList();

                    rand = (new Random()).Next(unlockedRadioEpisodes.Count() - 1);
                    ViewBag.unlockedEpisode = unlockedRadioEpisodes[rand];
                    ViewBag.unlockedEpisodeCount = unlockedRadioEpisodes.Count();
                }
            }

            return View();
        }

        [AllowAnonymous]
        public ViewResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        public PartialViewResult RandomFact()
        {
            return PartialView(new RandomView(Db));
        }

        [AllowAnonymous]
        public ActionResult DatabaseLinks(string table, string id)
        {
            ViewBag.Table = table;
            ViewBag.ID = id;

            if (ViewBag.Table == "Episode")
            {
                int epId = Int16.Parse(ViewBag.ID);
                ViewBag.adaptId = (from e in Db.Episodes
                                   where e.ID == epId
                                   select e.Season.AdaptationID).FirstOrDefault();
            }

            return PartialView();
        }

        [AllowAnonymous]
        public PartialViewResult Menu()
        {
            // the menu at the top of the main layout

            // get the tables for the Data links
            var holmesConn = WebConfigurationManager.ConnectionStrings["HolmesDB"].ConnectionString;
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
                               "seasons",
                               "species",
                               "stories",

                               // the dates used for canon stories don't have a page
                               "date",

                               // this leaves:
                               // Honorific, Medium, Rename, Reference, HolmesLink, HolmesLinkAppearance

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

        private static string AppendDate(string urlname, int i) =>
             string.Concat(urlname, "_", DateTime.Now.ToString("o", CultureInfo.InvariantCulture), "_", i);

        private int ActorCreateShort(string forename, string surname, int i)
        {
            var actor = new Actor
            {
                Forename = forename,
                Surname = surname,
                UrlName = Shared.BuildUrlName(forename, surname)
            };

            if (Db.Actors.Where(c => c.UrlName == actor.UrlName).Any())
            {
                actor.UrlName = AppendDate(actor.UrlName, i);
            }

            Db.Actors.Add(actor);
            Db.SaveChanges();

            return actor.ID;
        }

        private int CharCreateShort(string forename, string surname, int i)
        {
            var character = new Character
            {
                Forename = forename,
                Surname = surname,
                UrlName = Shared.BuildUrlName(forename, surname)
            };

            if (Db.Characters.Where(c => c.UrlName == character.UrlName).Any())
            {
                character.UrlName = AppendDate(character.UrlName, i);
            }

            Db.Characters.Add(character);
            Db.SaveChanges();

            return character.ID;
        }
    }
}
