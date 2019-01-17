namespace HolmesMVC.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Web.Mvc;
    using HolmesMVC.Services.BaconNumber;
    using HolmesMVC.Enums;
    using HolmesMVC.Models;
    using HolmesMVC.Models.ViewModels;

    [OutputCache(Duration = 2628000, VaryByCustom = "LastDbUpdate")]
    public class ToysController : HolmesDbController
    {
        [AllowAnonymous]
        [HttpPost]
#pragma warning disable CA1822 // Mark members as static
        public string BrettNum(string targetImdbName)
#pragma warning restore CA1822 // Mark members as static
        {
            // check spelling of target
            targetImdbName = targetImdbName.Trim();

            var spellcheckBN = new BaconNumber(1, Shared.JeremyBrettImdb(), targetImdbName);
            if (spellcheckBN.Number == -2)
            {
                return "Could not find anyone called '" + targetImdbName + "' in IMDb. You may need to add (I) or (II) to the name. Try searching: <a href='https://www.imdb.com/find?s=nm&q=" + targetImdbName + "'>click here</a>";
            }

            if (spellcheckBN.Number == -1)
            {
                return "Unknown error during spellcheck!";
            }

            if (spellcheckBN.Number == -42)
            {
                return "According to the Oracle of Bacon, this actor CANNOT be linked to Jeremy Brett!";
            }

            return BaconNumber.BrettNumber(targetImdbName);
        }

        [AllowAnonymous]
        [HttpPost]
        public string HolmesNum(string targetImdbName)
        {
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
                    var actorName = dbActor.ShortName;

                    // they're in Appledore!
                    // are they a Holmes or a Watson?
                    var isAHolmes = (from ap in dbActor.Appearances
                                     where ap.CharacterID == (int)CanonCharacter.Holmes
                                     select ap).Any();
                    var isAWatson = (from ap in dbActor.Appearances
                                     where ap.CharacterID == (int)CanonCharacter.Watson
                                     select ap).Any();

                    var dbApp = dbActor.Appearances.OrderBy(a => a.Episode.Airdate).First();
                    var dbAdapt = dbApp.Episode.Season.Adaptation;
                    var dbRename = dbApp.GetRename();
                    var charName = null == dbRename ? dbApp.Character.LongName : dbRename.LongName;

                    if (isAHolmes)
                    {
                        dbApp = (from ap in dbActor.Appearances
                                    where ap.CharacterID == (int)CanonCharacter.Holmes
                                 select ap).First();
                        dbAdapt = dbApp.Episode.Season.Adaptation;
                        dbRename = dbApp.GetRename();
                        charName = null == dbRename ? dbApp.Character.LongName : dbRename.LongName;

                        stringOut = "Appledore has a page on <a href='/Actor/" + dbActor.ID + "'>" + actorName + "</a>!<br><br>";
                        stringOut += actorName + " played " + charName + " in '" + dbAdapt.DisplayName + "'.";
                        stringOut += "<br><br>" + actorName + "'s Holmes number is 0.";
                        return stringOut;
                    }

                    if (isAWatson)
                    {
                        dbApp = (from ap in dbActor.Appearances
                                 where ap.CharacterID == (int)CanonCharacter.Watson
                                 select ap).First();
                        dbAdapt = dbApp.Episode.Season.Adaptation;
                        dbRename = dbApp.GetRename();
                        charName = null == dbRename ? dbApp.Character.LongName : dbRename.LongName;
                        stringOut = "Appledore has a page on <a href='/Actor/" + dbActor.ID + "'>" + actorName + "</a>!<br><br>";
                        stringOut += actorName + " played " + charName + " in '" + dbAdapt.DisplayName + "', which starred <a href='/Actor/" + new AdaptView(dbAdapt).HolmesActors.First().ID + "'>" + (new AdaptView(dbAdapt).HolmesActors.First()).ShortName + "</a> as Sherlock Holmes.";
                        stringOut += "<br><br>" + actorName + "'s Holmes number is 1.";
                        return stringOut;
                    }

                    // Or if they're just some randomer.
                    stringOut = "Appledore has a page on <a href='/Actor/" + dbActor.ID + "'>" + actorName + "</a>!<br><br>";
                    stringOut += actorName + " played " + charName + " in '" + dbAdapt.DisplayName + "', which starred <a href='/Actor/" + new AdaptView(dbAdapt).HolmesActors.First().ID + "'>" + (new AdaptView(dbAdapt).HolmesActors.First()).ShortName + "</a> as Sherlock Holmes.";
                    stringOut += "<br><br>" + actorName + "'s Holmes number is 1.";
                    return stringOut;
                }

                // if they're not in the database, we'll need to hit the Oracle
                // check spelling of target
                targetImdbName = targetImdbName.Trim();

                var spellcheckBN = new BaconNumber(1, Shared.JeremyBrettImdb(), targetImdbName);
                if (spellcheckBN.Number == -2)
                {
                    return "Could not find anyone called '" + targetImdbName
                           + "' in IMDb. You may need to add (I) or (II) to the name. Try searching: <a href='https://www.imdb.com/find?s=nm&q="
                           + targetImdbName + "'>click here</a>";
                }

                if (spellcheckBN.Number == -1)
                {
                    return "Unknown error during spellcheck!";
                }


                // if we can get out early, do it
                int lowestHolmesNum;
                if (spellcheckBN.Number == 1)
                {
                    return stringOut + spellcheckBN.Message;
                }
                // otherwise, keep going
                else
                {
                    lowestHolmesNum = spellcheckBN.Number;
                }

                // get holmes list
                var holmeses = (from a in Db.Actors
                                where a.IMDbName != null
                                select new ToyHolmes { ID = a.ID, Name = a.IMDbName }).ToList();

                var r = new Random();
                holmeses = holmeses.OrderBy(x => r.Next()).ToList();

                // spelling passed, calculate number
                var count = holmeses.Count();
                string holmesStr = string.Empty;

                for (int i = 0; i < count; i++)
                {
                    var holmesTesting = holmeses[i];

                    // this saves checking Jeremy twice
                    BaconNumber baconNum;
                    if (holmesTesting.ID == 1)
                    {
                        baconNum = spellcheckBN;
                    }
                    else
                    {
                        baconNum = new BaconNumber(holmesTesting.ID, holmesTesting.Name, targetImdbName);
                    }

                    if (baconNum.Number < 0)
                    {
                        // awards show kludge
                        if (baconNum.Number == -10)
                        {
                            continue;
                        }

                        // genuinely unlinkable actors
                        if (baconNum.Number == -42)
                        {
                            continue;
                        }

                        // spellcheck
                        if (baconNum.Number == -2)
                        {
                            return stringOut
                                + "Please spellcheck this actor's IMDb name: "
                                   + holmesTesting.Name + ".";
                        }

                        return stringOut
                            + "Got a " + baconNum.Number
                               + " result when testing holmes " + i + ": "
                               + holmesTesting.Name + ".";
                    }

                    if (baconNum.Number < lowestHolmesNum || holmesTesting.ID == 1 && baconNum.Number == lowestHolmesNum)
                    {
                        holmesStr = baconNum.Message;
                        lowestHolmesNum = baconNum.Number;
                    }

                    if (lowestHolmesNum == 1)
                    {
                        break;
                    }
                }

                if (string.IsNullOrEmpty(holmesStr))
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

        public ViewResult Scraps()
        {
            var holmesID = (int)CanonCharacter.Holmes;
            
            // How about an expansion of the HolmesNum to generate a network?
            // Inspired by talking to Ian Rennie about connections between Holmeses
            // 'here's one for you: Jeremy Brett was in Mad Dogs And Englishmen with C Thomas Howell, who was in the show Smith with JLM.'

            // Colour code the links for closeness, rather than showing the intermediate non-Holmes actors
            // Make it so you can click on one to see the details
            // I'm envisaging something like the navigation map on my tablet for Elite Dangerous star charts
            // The links are the important thing, not the layout. Is there a library to make them fit on the page / bounce around until they fit?

            // Answer: yes there is, check out d3.js http://bl.ocks.org/mbostock/4062045

            // This is going to be computation heavy, so store the links in a new database table and have a recalculate action you can call on demand
            // e.g. when you've added someone new
            // Initially might need to make a few links by hand and work on the display / on the network drawing
            // then we can think about using the Bacon code to generate links

            // Perhaps two views: closeness of net, and chronological...ness, which means we'll need dates on the links
            // They'll be similar but not identical: actors have to both be alive, to appear together
            // (unless they are Basil Rathbone who managed to be in TGMD anyway)
            // Basically we're looking to chase back the lineage of the character into the very early days of film

            string[] blockedNames = { "Nobody" };

            var nodeList = new List<string>();

            var holmesActors = Db.Actors.Where(a => a.Appearances.Where(ap => ap.CharacterID == holmesID).Any()).ToList();
            var holmesLinkActors = Db.HolmesLinkActors.ToList();

            var updated = false;
            foreach (var h in holmesActors)
            {
                if (!holmesLinkActors.Where(hla => hla.ActorID == h.ID).Any())
                {
                    var newHLA = new HolmesLinkActor
                    {
                        ActorID = h.ID
                    };
                    Db.HolmesLinkActors.Add(newHLA);
                    updated = true;
                }
            }
            if (updated)
            {
                Db.SaveChanges();
            }

            foreach (HolmesLinkActor act in holmesLinkActors)
            {
                var myName = act.Actor.Surname; 
                var groupID = 1;

                if (!blockedNames.Contains(myName))
                {
                    nodeList.Add("{\"id\": \"" + act.ID + "\", \"name\": \"" + myName.Replace("'","").ToUpper() + "\", \"group\": " + groupID + "}");
                }
            }

            var linkList = new List<string>();

            foreach (HolmesLinkActor act in Db.HolmesLinks.SelectMany(l => l.HolmesLinkAppearances).Select(app => app.HolmesLinkActor).Distinct())
            {
                var myName = act.Actor.ShortName;
                foreach (HolmesLinkActor linkedActor in act.HolmesLinkAppearances.Select(app => app.HolmesLink).SelectMany(link => link.HolmesLinkAppearances).Select(app => app.HolmesLinkActor).Distinct())
                {
                    if (linkedActor.ID > act.ID)
                    {
                        var theirName = linkedActor.Actor.ShortName;

                        if (!blockedNames.Contains(myName) && !blockedNames.Contains(theirName))
                        {
                            linkList.Add("{\"source\": \"" + act.ID + "\", \"target\": \"" + linkedActor.ID + "\", \"value\": 1}");
                        }
                    }
                }
            }

            var jsonString = "{ \"nodes\" : [ " + string.Join(", ", nodeList) + " ] , \"links\" : [ " + string.Join(", ", linkList) + " ] }";  

            var model = new ScrapsView { HolmesLinks = jsonString };

            return View(model);
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult HolmesNumToy()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult MultiTimelineToy()
        {
            var adaptList = (from ad in Db.Adaptations
                             where ad.Seasons.Any()
                             && ad.Seasons.FirstOrDefault().Episodes.Any()
                             && ad.Medium != (int)Medium.Literature
                             && ad.Medium != (int)Medium.Stage //to_do_theatre
                             orderby ad.Seasons.FirstOrDefault().Episodes.FirstOrDefault().Airdate
                             select ad).ToList();

            return View(adaptList);
        }

        [AllowAnonymous]
        public ActionResult PhotoCollageToy()
        {
            var holmesList = (from app in Db.Appearances
                              where app.CharacterID == (int)CanonCharacter.Holmes
                              && app.ActorID > 0
                              && !string.IsNullOrEmpty(app.Actor.Pic)
                              group app by app.ActorID into grp
                              orderby grp.FirstOrDefault().Episode.Airdate
                              select grp.FirstOrDefault().Actor).ToList();

            return View(holmesList);
        }

        [AllowAnonymous]
        public ActionResult MapPinToy()
        {
            var holmesList = (from app in Db.Appearances
                              where app.CharacterID == (int)CanonCharacter.Holmes
                              && app.ActorID > 0
                              && !string.IsNullOrEmpty(app.Actor.Birthplace)
                              group app by app.ActorID into grp
                              orderby grp.FirstOrDefault().Episode.Airdate
                              select grp.FirstOrDefault().Actor).ToList();

            return View(holmesList);
        }
    }
}
