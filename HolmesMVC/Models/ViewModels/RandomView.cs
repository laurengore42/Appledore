namespace HolmesMVC.Models.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using HolmesMVC.Enums;

    public class RandomView
    {
        public RandomView(HolmesDBEntities db)
        {
            var rightNow = DateTime.Now;

            var thisMonth = rightNow.Month;
            var thisDay = rightNow.Day;

            var actorList = (from a in db.Actors
                             where null != a.Birthdate
                             && a.Birthdate.Value.Month == thisMonth
                             && a.Birthdate.Value.Day == thisDay
                             && a.BirthdatePrecision == (int)DatePrecision.Full
                             orderby a.Birthdate
                             select a).ToList();
            BirthdayActors = (from a in actorList
                              select new BirthdayActor(a)
                              ).ToList();
            var epList = (from e in db.Episodes
                          where 
                          e.Season.Adaptation.Medium != (int)Medium.Stage
                          && e.Airdate.Month == thisMonth
                          && e.Airdate.Day == thisDay
                          && e.AirdatePrecision == (int)DatePrecision.Full
                          orderby e.Airdate
                          select e).ToList();
            AnnivEpisodes = (from e in epList
                             select new AnnivEpisode(e) // AnnivEpisode constructor not valid in LINQ to Entities
                             ).ToList();
        }

        public List<BirthdayActor> BirthdayActors { get; set; }

        public List<AnnivEpisode> AnnivEpisodes { get; set; } 
    }
}