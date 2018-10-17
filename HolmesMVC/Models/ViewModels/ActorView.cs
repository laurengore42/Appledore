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

        public string UrlName { get; set; }

        public string IMDbName { get; set; }

        public int? Age { get; set; }

        public string PicShow { get; set; }

        public string PicCreditShow { get; set; }

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
            PicShow = actor.PicShow;
            PicCreditShow = actor.PicCreditShow;
            Birthdate = actor.Birthdate;
            Birthplace = actor.Birthplace;
            BirthdatePrecision = (DatePrecision)actor.BirthdatePrecision;
            Deathdate = actor.Deathdate;
            DeathdatePrecision = (DatePrecision)actor.DeathdatePrecision;
            IMDb = actor.IMDb;
            IMDbName = actor.IMDbName;
            Wikipedia = actor.Wikipedia;
            UrlName = actor.UrlName;
            Age = actor.AgeInYears;

            ShortName = actor.ShortName;
            LongName = actor.LongName;

            // Get actor's appearances
            Histories = (from ap in actor.Appearances
                           orderby ap.Episode.Airdate, ap.EpisodeID, ap.CharacterID
                           select new ActorHistory(ap)).ToList();

            // Group episodes
            Summaries = (from ap in actor.Appearances
                                   group ap by new
                                   {
                                       ap.Episode.Season.AdaptationID,
                                       ap.CharacterID
                                   }
                                       into grp
                                       orderby grp.Min(a => a.Episode.Airdate)
                                       select new ActorHistorySummary(
                                               grp.Key.AdaptationID,
                                               grp.Key.CharacterID,
                                               grp.ToList()
                                           )).ToList();

            // Get actor's opposites
            
            HolmesActors = (from ap in
                                (
                                from ap in actor.Appearances
                                where ap.CharacterID == (int)CanonCharacter.Watson
                                from ap1 in ap.Episode.Appearances.Where(a => a.CharacterID == (int)CanonCharacter.Holmes)
                                select ap1
                                )
                            group ap by ap.Actor
                                into grp
                                select grp.Key).ToList();

            WatsonActors = (from ap in
                                (
                                from ap in actor.Appearances
                                where ap.CharacterID == (int)CanonCharacter.Holmes
                                from ap1 in ap.Episode.Appearances.Where(a => a.CharacterID == (int)CanonCharacter.Watson)
                                select ap1
                                )
                            group ap by ap.Actor
                                into grp
                                select grp.Key).ToList();

            if (actor.Appearances.Any())
            {
                YearOfFirstApp = actor.Appearances.OrderBy(a => a.Episode.Airdate).First().Episode.Airdate.Year;
                YearOfLastApp = actor.Appearances.OrderBy(a => a.Episode.Airdate).Last().Episode.Airdate.Year;
            }
        }

        public int YearOfFirstApp { get; set; }

        public int YearOfLastApp { get; set; }

        public List<Actor> HolmesActors { get; set; }

        public List<Actor> WatsonActors { get; set; }
    }
}