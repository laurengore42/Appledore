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
                          && e.Season.Adaptation.Name != "Canon" // IsCanon not valid in LINQ to Entities
                          && e.Airdate.Month == thisMonth
                          && e.Airdate.Day == thisDay
                          && e.AirdatePrecision == (int)DatePrecision.Full
                          orderby e.Airdate
                          select e).ToList();
            AnnivEpisodes = (from e in epList
                             select new AnnivEpisode(e) // AnnivEpisode constructor not valid in LINQ to Entities
                             ).ToList();

            //var canonChars = from ap in db.Appearances
            //                 where ap.Episode.Season.Adaptation.Name == "Canon" // IsCanon not valid in LINQ to Entities
            //                 && ap.CharacterID > 0
            //                 select ap.CharacterID;

            //var canonApps = from ap in db.Appearances
            //                where canonChars.Contains(ap.CharacterID)
            //                && ap.Episode.Season.Adaptation.Medium != (int)Medium.Stage
            //                group ap by ap.ActorID into grp
            //                select grp;
            
            //var multiCharActors = (from ap in canonApps
            //                       where ap.Select(a => a.CharacterID).Distinct().Count() > 1
            //                       && ap.FirstOrDefault().ActorID > 0
            //                       select ap.FirstOrDefault().Actor).OrderBy(a => a.Surname).ToList();
            //MultiActor = new MultiActor
            //{
            //    Actor = multiCharActors[(new Random()).Next(multiCharActors.Count())]
            //};
            //MultiActor.Characters = (from ap in db.Appearances
            //                         where ap.ActorID == MultiActor.Actor.ID
            //                         && canonChars.Contains(ap.CharacterID)
            //                         select ap.Character).Distinct().ToList();
        }

        public MultiActor MultiActor { get; set; }

        public List<BirthdayActor> BirthdayActors { get; set; }

        public List<AnnivEpisode> AnnivEpisodes { get; set; } 
    }
}