namespace HolmesMVC.Models.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using HolmesMVC.Enums;
    using HolmesMVC.Models;

    public class ActorView
    {
        public int ID { get; set; }

        public string ShortName { get; set; }

        public string LongName { get; set; }

        public string IMDbName { get; set; }

        public int? Age { get; set; }

        public string Pic { get; set; }

        public DateTime? Birthdate { get; set; }

        public DatePrecision BirthdatePrecision { get; set; }

        public string Birthplace { get; set; }

        public DateTime? Deathdate { get; set; }

        public DatePrecision DeathdatePrecision { get; set; }

        public string IMDb { get; set; }

        public string Wikipedia { get; set; }

        public IEnumerable<ActorHistory> Histories { get; set; }

        public IEnumerable<ActorHistorySummary> Summaries { get; set; }

        public ActorView(Actor actor)
        {
            ID = actor.ID;
            Pic = actor.Pic;
            Birthdate = actor.Birthdate;
            Birthplace = actor.Birthplace;
            BirthdatePrecision = (DatePrecision)actor.BirthdatePrecision;
            Deathdate = actor.Deathdate;
            DeathdatePrecision = (DatePrecision)actor.DeathdatePrecision;
            IMDb = actor.IMDb;
            IMDbName = actor.IMDbName;
            Wikipedia = actor.Wikipedia;

            ShortName = Shared.ShortName(actor);
            LongName = Shared.LongName(actor);
            Age = Shared.AgeInYears(actor);

            // Get actor's appearances
            Histories = (from ap in actor.Appearances
                           orderby ap.Episode1.Airdate ascending
                           select new ActorHistory(ap)).ToList();

            // Group episodes
            Summaries = (from ap in actor.Appearances
                                   group ap by new
                                   {
                                       ap.Episode1.Season1.Adaptation,
                                       ap.Character
                                   }
                                       into grp
                                       orderby grp.Min(a => a.Episode1.Airdate)
                                       select new ActorHistorySummary(
                                               grp.Key.Adaptation,
                                               grp.Key.Character,
                                               grp.ToList()
                                           )).ToList();

            if (actor.Appearances.Any())
            {
                YearOfFirstApp = actor.Appearances.OrderBy(a => a.Episode1.Airdate).First().Episode1.Airdate.Year;
                YearOfLastApp = actor.Appearances.OrderBy(a => a.Episode1.Airdate).Last().Episode1.Airdate.Year;
            }
        }

        public int YearOfFirstApp { get; set; }

        public int YearOfLastApp { get; set; }
    }
}