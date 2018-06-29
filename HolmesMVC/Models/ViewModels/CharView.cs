namespace HolmesMVC.Models.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Microsoft.Ajax.Utilities;

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
                              ap.Episode1.Season1.Adaptation1.Medium1.Name
                              != "Stage"
                          // to_do_theatre
                          orderby ap.Episode1.Airdate ascending
                          select ap;
            var groupedData = from ap in dataGet
                               group ap by
                                   new
                                       {
                                           ap.Actor,
                                           ap.Episode1.Season1.Adaptation
                                       }
                               into grp
                               select grp;

            // Compute average age of an actor's first appearance as this character
            AverageAge = -1;
            var allFirstAppearancesWithAges = from grp in groupedData
                                              where grp.First().Actor1.Birthdate != null
                                              select grp.First();

            if (allFirstAppearancesWithAges.Any())
            {
                AverageAge = (from ap in allFirstAppearancesWithAges
                              select
                                  ((TimeSpan)
                                   (ap.Episode1.Airdate - ap.Actor1.Birthdate))
                                  .TotalDays).Average() / 365.26;
            }

            // Choose a pic from the actors available
            Pics = (from ap in character.Appearances
                    where ap.Actor > 0
                    && ap.Episode1.Season1.Adaptation1.Medium1.Name != "Stage" // to_do_theatre
                    && ap.Actor1.PicShow != null
                    group ap by new {ap.Actor, ap.Actor1.Forename, ap.Actor1.Surname, ap.Actor1.PicShow} into grp
                    select new CharPic
                               {
                                   ID = grp.Key.Actor,
                                   Name = grp.Key.Forename + " " + grp.Key.Surname,
                                   PicShow = grp.Key.PicShow
                               })
                .ToList();

            AllSummaries = (from grp in groupedData
                            select
                                new CharacterHistorySummary(
                                grp.Key.Actor,
                                grp.Key.Adaptation,
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
        public int ID { get; set; }

        public string Name { get; set; }

        public string PicShow { get; set; }
    }
}