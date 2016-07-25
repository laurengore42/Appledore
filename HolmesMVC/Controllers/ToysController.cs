﻿namespace HolmesMVC.Controllers
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

    public class ToysController : HolmesDbController
    {
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
            var holmesId = Shared.GetHolmes();
            var watsonId = Shared.GetWatson();

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
        public ViewResult Scraps()
        {
            var model = new ScrapsView { };

            return View(model);
        }

        [AllowAnonymous]
        [Stopwatch]
        [HttpGet]
        public ActionResult HolmesNumToy()
        {
            return View();
        }

        [AllowAnonymous]
        [Stopwatch]
        public ActionResult MultiTimelineToy()
        {
            var adaptList = (from ad in Db.Adaptations
                             where ad.Seasons.Any()
                             && ad.Seasons.FirstOrDefault().Episodes.Any()
                             && ad.Medium1.Name != "Literature"
                             && ad.Medium1.Name != "Stage" //to_do_theatre
                             orderby ad.Seasons.FirstOrDefault().Episodes.FirstOrDefault().Airdate
                             select ad).ToList();

            return View(adaptList);
        }

        [AllowAnonymous]
        [Stopwatch]
        public ActionResult PhotoCollageToy()
        {
            var holmesId = Shared.GetHolmes();

            var holmesList = (from app in Db.Appearances
                              where app.Character == holmesId
                              && app.Actor > 0
                              && app.Actor1.Pic != null && app.Actor1.Pic != ""
                              group app by app.Actor into grp
                              orderby grp.FirstOrDefault().Episode1.Airdate
                              select grp.FirstOrDefault().Actor1).ToList();

            return View(holmesList);
        }

        [AllowAnonymous]
        [Stopwatch]
        public ActionResult MapPinToy()
        {
            var holmesId = Shared.GetHolmes();

            var holmesList = (from app in Db.Appearances
                              where app.Character == holmesId
                              && app.Actor > 0
                              && app.Actor1.Birthplace != null && app.Actor1.Birthplace != ""
                              group app by app.Actor into grp
                              orderby grp.FirstOrDefault().Episode1.Airdate
                              select grp.FirstOrDefault().Actor1).ToList();

            return View(holmesList);
        }
    }
}
