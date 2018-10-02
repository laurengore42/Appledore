namespace HolmesMVC.Models.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using HolmesMVC.Enums;

    public class CharView
    {
        public CharView(Character character)
        {
            Char = character;

            ShortName = Shared.ShortName(character);
            LongName = Shared.LongName(character);

            // All appearances of character, grouped by actor and adaptation
            var dataGet = from ap in character.Appearances
                          where
                              ap.Episode.Season.Adaptation.Medium
                              != (int)Medium.Stage
                          // to_do_theatre
                          orderby ap.Episode.Airdate ascending
                          select ap;
            var groupedData = from ap in dataGet
                               group ap by
                                   new
                                       {
                                           ap.ActorID,
                                           ap.Episode.Season.AdaptationID
                                       }
                               into grp
                               select grp;

            // Compute average age of an actor's first appearance as this character
            AverageAge = -1;
            var allFirstAppearancesWithAges = from grp in groupedData
                                              where grp.First().Actor.Birthdate != null
                                              select grp.First();

            if (allFirstAppearancesWithAges.Any())
            {
                AverageAge = (from ap in allFirstAppearancesWithAges
                              select
                                  ((TimeSpan)
                                   (ap.Episode.Airdate - ap.Actor.Birthdate))
                                  .TotalDays).Average() / 365.26;
            }

            // Choose a pic from the actors available
            Pics = (from ap in character.Appearances
                    where ap.ActorID > 0
                    && ap.Episode.Season.Adaptation.Medium != (int)Medium.Stage // to_do_theatre
                    && !String.IsNullOrEmpty(ap.Actor.PicShow)
                    group ap by new {ap.Actor.UrlName, ap.Actor.Forename, ap.Actor.Surname, ap.Actor.PicShow} into grp
                    select new CharPic
                               {
                                   UrlName = grp.Key.UrlName,
                                   Name = grp.Key.Forename + " " + grp.Key.Surname,
                                   PicShow = grp.Key.PicShow
                               })
                .ToList();

            AllSummaries = (from grp in groupedData
                            select
                                new CharacterHistorySummary(
                                grp.Key.ActorID,
                                grp.Key.AdaptationID,
                                grp.ToList())).ToList();
        }

        public Character Char { get; set; }

        public List<CharPic> Pics { get; set; }

        public string ShortName { get; set; }

        public string LongName { get; set; }

        public double AverageAge { get; set; }

        public List<CharacterHistorySummary> AllSummaries { get; set; }
    }

    public class CharPic
    {
        public string UrlName { get; set; }

        public string Name { get; set; }

        public string PicShow { get; set; }
    }
}