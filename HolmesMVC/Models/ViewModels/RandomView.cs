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
            const int FullPrecision = (int)DatePrecision.Full;

            var rightNow = DateTime.Now;

            var thisMonth = rightNow.Month;
            var thisDay = rightNow.Day;

            var holmesId = Shared.GetHolmes();
            var watsonId = Shared.GetWatson();

            var actorList = (from a in db.Actors
                             where null != a.Birthdate
                             && a.Birthdate.Value.Month == thisMonth
                             && a.Birthdate.Value.Day == thisDay
                             && a.BirthdatePrecision == FullPrecision
                             orderby a.Birthdate
                             select a).ToList();
            BirthdayActors = (from a in actorList
                              select new BirthdayActor(a)
                              ).ToList();
            var epList = (from e in db.Episodes
                          where 
                          e.Season1.Adaptation1.Medium1.Name != "Stage"
                          && e.Season1.Adaptation1.Name != "Canon"
                          && e.Airdate.Month == thisMonth
                          && e.Airdate.Day == thisDay
                          && e.AirdatePrecision == FullPrecision
                          orderby e.Airdate
                          select e).ToList();
            AnnivEpisodes = (from e in epList
                             select new AnnivEpisode(e, holmesId)
                             ).ToList();

            var canonChars = from ap in db.Appearances
                             where ap.Episode1.Season1.Adaptation1.Name == "Canon"
                             && ap.Character > 0
                             select ap.Character;

            var canonApps = from ap in db.Appearances
                            where canonChars.Contains(ap.Character)
                            && ap.Episode1.Season1.Adaptation1.Medium1.Name != "Stage"
                            group ap by ap.Actor into grp
                            select grp;
            
            var multiCharActors = (from ap in canonApps
                                   where ap.Select(a => a.Character).Distinct().Count() > 1
                                   && ap.FirstOrDefault().Actor > 0
                                   select ap.FirstOrDefault().Actor1).OrderBy(a => a.Surname).ToList();
            MultiActor = new MultiActor();
            MultiActor.Actor =
                multiCharActors[(new Random()).Next(multiCharActors.Count())];
            MultiActor.Characters =
                                     (from ap in db.Appearances
                                      where ap.Actor == MultiActor.Actor.ID
                                      select ap.Character1).Distinct().ToList();
        }

        public MultiActor MultiActor { get; set; }

        public List<BirthdayActor> BirthdayActors { get; set; }

        public List<AnnivEpisode> AnnivEpisodes { get; set; } 
    }
}