using System.Collections.Generic;
using System.Linq;

namespace HolmesMVC.Models.ViewModels
{
    public class AdaptListView
    {
        const int HolmesId = 1;

        public AdaptListView(HolmesDBEntities Db)
        {
            AdaptsFilm = (from a in Db.Adaptations
                          where a.Medium.Name == "Film"
                         && a.Seasons.Any()
                         && a.Seasons.SelectMany(s => s.Episodes).Count() > 1
                          select new AdaptListAdapt
                          {
                              ID = a.ID,

                              Name =
                                    (
                                    !(null == a.Name || string.Empty == a.Name)
                                    ? a.Name.Replace("\"", string.Empty)
                                    : (a.Seasons.Count() == 1 && a.Seasons.FirstOrDefault().Episodes.Count() == 1)
                                    ? (null == a.Seasons.FirstOrDefault().Episodes.FirstOrDefault().Title || string.Empty == a.Seasons.FirstOrDefault().Episodes.FirstOrDefault().Title)
                                    ? a.Seasons.FirstOrDefault().Episodes.FirstOrDefault().Story.Name
                                    : a.Seasons.FirstOrDefault().Episodes.FirstOrDefault().Title
                                    : (a.Company + " " + (a.Medium.Name == "Television" ? "TV" : a.Medium.Name))
                                   ).Replace("\"", string.Empty),

                              Translation = (a.Translation == null || a.Translation == string.Empty || a.Translation.Length == 0)
                                    ? string.Empty
                                    : a.Translation.Replace("\"", string.Empty),

                              Holmes = (from ap in a.Seasons
                                        .SelectMany(s => s.Episodes)
                                        .SelectMany(e => e.Appearances)
                                        where ap.CharacterID == HolmesId
                                        select ap).Any()
                                    ? (from ap in a.Seasons
                                       .SelectMany(s => s.Episodes)
                                       .SelectMany(e => e.Appearances)
                                       where ap.CharacterID == HolmesId
                                       group ap by ap.ActorID into grp
                                       orderby grp.Count() descending
                                       select grp.FirstOrDefault().Actor.Surname).FirstOrDefault()
                                    : string.Empty,

                              Year = a.Seasons
                              .OrderBy(s => s.AirOrder)
                              .FirstOrDefault()
                              .Episodes
                              .OrderBy(e => e.Airdate)
                              .FirstOrDefault()
                              .Airdate
                              .Year,

                              EpCount = a.Seasons
                              .SelectMany(s => s.Episodes)
                              .Count(),

                              Medium = a.Medium.Name
                          }).ToList();

            AdaptsSingleFilm = (from a in Db.Adaptations
                                where a.Medium.Name == "Film"
                         && a.Seasons.Any()
                         && a.Seasons.SelectMany(s => s.Episodes).Count() == 1
                          select new AdaptListAdapt
                          {
                              ID = a.ID,

                              Name =
                                    (
                                    !(null == a.Name || string.Empty == a.Name)
                                    ? a.Name.Replace("\"", string.Empty)
                                    : (a.Seasons.Count() == 1 && a.Seasons.FirstOrDefault().Episodes.Count() == 1)
                                    ? (null == a.Seasons.FirstOrDefault().Episodes.FirstOrDefault().Title || string.Empty == a.Seasons.FirstOrDefault().Episodes.FirstOrDefault().Title)
                                    ? a.Seasons.FirstOrDefault().Episodes.FirstOrDefault().Story.Name
                                    : a.Seasons.FirstOrDefault().Episodes.FirstOrDefault().Title
                                    : (a.Company + " " + (a.Medium.Name == "Television" ? "TV" : a.Medium.Name))
                                   ).Replace("\"", string.Empty),

                              Translation = (a.Translation == null || a.Translation == string.Empty || a.Translation.Length == 0)
                                    ? string.Empty
                                    : a.Translation.Replace("\"", string.Empty),

                              Holmes = (from ap in a.Seasons
                                        .SelectMany(s => s.Episodes)
                                        .SelectMany(e => e.Appearances)
                                        where ap.CharacterID == HolmesId
                                        select ap).Any()
                                    ? (from ap in a.Seasons
                                       .SelectMany(s => s.Episodes)
                                       .SelectMany(e => e.Appearances)
                                       where ap.CharacterID == HolmesId
                                       group ap by ap.ActorID into grp
                                       orderby grp.Count() descending
                                       select grp.FirstOrDefault().Actor.Surname).FirstOrDefault()
                                    : string.Empty,

                              Year = a.Seasons
                              .OrderBy(s => s.AirOrder)
                              .FirstOrDefault()
                              .Episodes
                              .OrderBy(e => e.Airdate)
                              .FirstOrDefault()
                              .Airdate
                              .Year,

                              EpCount = a.Seasons
                              .SelectMany(s => s.Episodes)
                              .Count(),

                              Medium = a.Medium.Name
                          }).ToList();

            AdaptsTV = (from a in Db.Adaptations
                        where a.Medium.Name == "Television"
                         && a.Seasons.Any()
                         && a.Seasons.SelectMany(s => s.Episodes).Count() > 1
                          select new AdaptListAdapt
                          {
                              ID = a.ID,

                              Name =
                                    (
                                    !(null == a.Name || string.Empty == a.Name)
                                    ? a.Name.Replace("\"", string.Empty)
                                    : (a.Seasons.Count() == 1 && a.Seasons.FirstOrDefault().Episodes.Count() == 1)
                                    ? (null == a.Seasons.FirstOrDefault().Episodes.FirstOrDefault().Title || string.Empty == a.Seasons.FirstOrDefault().Episodes.FirstOrDefault().Title)
                                    ? a.Seasons.FirstOrDefault().Episodes.FirstOrDefault().Story.Name
                                    : a.Seasons.FirstOrDefault().Episodes.FirstOrDefault().Title
                                    : (a.Company + " " + (a.Medium.Name == "Television" ? "TV" : a.Medium.Name))
                                   ).Replace("\"", string.Empty),

                              Translation = (a.Translation == null || a.Translation == string.Empty || a.Translation.Length == 0)
                                    ? string.Empty
                                    : a.Translation.Replace("\"", string.Empty),

                              Holmes = (from ap in a.Seasons
                                        .SelectMany(s => s.Episodes)
                                        .SelectMany(e => e.Appearances)
                                        where ap.CharacterID == HolmesId
                                        select ap).Any()
                                    ? (from ap in a.Seasons
                                       .SelectMany(s => s.Episodes)
                                       .SelectMany(e => e.Appearances)
                                       where ap.CharacterID == HolmesId
                                       group ap by ap.ActorID into grp
                                       orderby grp.Count() descending
                                       select grp.FirstOrDefault().Actor.Surname).FirstOrDefault()
                                    : string.Empty,

                              Year = a.Seasons
                              .OrderBy(s => s.AirOrder)
                              .FirstOrDefault()
                              .Episodes
                              .OrderBy(e => e.Airdate)
                              .FirstOrDefault()
                              .Airdate
                              .Year,

                              EpCount = a.Seasons
                              .SelectMany(s => s.Episodes)
                              .Count(),

                              Medium = a.Medium.Name
                          }).ToList();

            AdaptsSingleRadio = (from a in Db.Adaptations
                                 where a.Medium.Name == "Radio"
                          && a.Seasons.Any()
                          && a.Seasons.SelectMany(s => s.Episodes).Count() == 1
                           select new AdaptListAdapt {
                                ID = a.ID,

                                Name = 
                                    (
                                    !(null == a.Name || string.Empty == a.Name)
                                    ? a.Name.Replace("\"", string.Empty)
                                    : (a.Seasons.Count() == 1 && a.Seasons.FirstOrDefault().Episodes.Count() == 1)
                                    ? (null == a.Seasons.FirstOrDefault().Episodes.FirstOrDefault().Title || string.Empty == a.Seasons.FirstOrDefault().Episodes.FirstOrDefault().Title)
                                    ? a.Seasons.FirstOrDefault().Episodes.FirstOrDefault().Story.Name
                                    : a.Seasons.FirstOrDefault().Episodes.FirstOrDefault().Title
                                    : (a.Company + " " + (a.Medium.Name == "Television" ? "TV" : a.Medium.Name))
                                   ).Replace("\"", string.Empty),

                                Translation = (a.Translation == null || a.Translation == string.Empty || a.Translation.Length == 0)
                                    ? string.Empty
                                    : a.Translation.Replace("\"", string.Empty),

                               Holmes = (from ap in a.Seasons
                                         .SelectMany(s => s.Episodes)
                                         .SelectMany(e => e.Appearances)
                                         where ap.CharacterID == HolmesId
                                         select ap).Any()
                                    ? (from ap in a.Seasons
                                       .SelectMany(s => s.Episodes)
                                       .SelectMany(e => e.Appearances)
                                       where ap.CharacterID == HolmesId
                                       group ap by ap.ActorID into grp
                                       orderby grp.Count() descending
                                       select grp.FirstOrDefault().Actor.Surname).FirstOrDefault()
                                    : string.Empty,

                               Year = a.Seasons
                              .OrderBy(s => s.AirOrder)
                              .FirstOrDefault()
                              .Episodes
                              .OrderBy(e => e.Airdate)
                              .FirstOrDefault()
                              .Airdate
                              .Year,

                               EpCount = a.Seasons
                              .SelectMany(s => s.Episodes)
                              .Count(),

                               Medium = a.Medium.Name
                           }).ToList();

            AdaptsRadio = (from a in Db.Adaptations
                           where a.Medium.Name == "Radio"
                       && a.Seasons.Any()
                       && a.Seasons.SelectMany(s => s.Episodes).Count() > 1
                        select new AdaptListAdapt
                        {
                            ID = a.ID,

                            Name =
                                    (
                                    !(null == a.Name || string.Empty == a.Name)
                                    ? a.Name.Replace("\"", string.Empty)
                                    : (a.Seasons.Count() == 1 && a.Seasons.FirstOrDefault().Episodes.Count() == 1)
                                    ? (null == a.Seasons.FirstOrDefault().Episodes.FirstOrDefault().Title || string.Empty == a.Seasons.FirstOrDefault().Episodes.FirstOrDefault().Title)
                                    ? a.Seasons.FirstOrDefault().Episodes.FirstOrDefault().Story.Name
                                    : a.Seasons.FirstOrDefault().Episodes.FirstOrDefault().Title
                                    : (a.Company + " " + (a.Medium.Name == "Television" ? "TV" : a.Medium.Name))
                                   ).Replace("\"", string.Empty),

                            Translation = (a.Translation == null || a.Translation == string.Empty || a.Translation.Length == 0)
                                    ? string.Empty
                                    : a.Translation.Replace("\"", string.Empty),

                            Holmes = (from ap in a.Seasons
                                      .SelectMany(s => s.Episodes)
                                      .SelectMany(e => e.Appearances)
                                      where ap.CharacterID == HolmesId
                                      select ap).Any()
                                    ? (from ap in a.Seasons
                                       .SelectMany(s => s.Episodes)
                                       .SelectMany(e => e.Appearances)
                                       where ap.CharacterID == HolmesId
                                       group ap by ap.ActorID into grp
                                       orderby grp.Count() descending
                                       select grp.FirstOrDefault().Actor.Surname).FirstOrDefault()
                                    : string.Empty,

                            Year = a.Seasons
                              .OrderBy(s => s.AirOrder)
                              .FirstOrDefault()
                              .Episodes
                              .OrderBy(e => e.Airdate)
                              .FirstOrDefault()
                              .Airdate
                              .Year,

                            EpCount = a.Seasons
                              .SelectMany(s => s.Episodes)
                              .Count(),

                            Medium = a.Medium.Name
                        }).ToList();

            AdaptsOther = (from a in Db.Adaptations
                           where a.Medium.Name != "Television" && a.Medium.Name != "Radio" && a.Medium.Name != "Film" && a.Medium.Name != "Stage" // to_do_theatre
                          && a.Seasons.Any()
                          && a.Seasons.SelectMany(s => s.Episodes).Count() > 1
                           select new AdaptListAdapt
                           {
                               ID = a.ID,

                               Name =
                                    (
                                    !(null == a.Name || string.Empty == a.Name)
                                    ? a.Name.Replace("\"", string.Empty)
                                    : (a.Seasons.Count() == 1 && a.Seasons.FirstOrDefault().Episodes.Count() == 1)
                                    ? (null == a.Seasons.FirstOrDefault().Episodes.FirstOrDefault().Title || string.Empty == a.Seasons.FirstOrDefault().Episodes.FirstOrDefault().Title)
                                    ? a.Seasons.FirstOrDefault().Episodes.FirstOrDefault().Story.Name
                                    : a.Seasons.FirstOrDefault().Episodes.FirstOrDefault().Title
                                    : (a.Company + " " + (a.Medium.Name == "Television" ? "TV" : a.Medium.Name))
                                   ).Replace("\"", string.Empty),

                               Translation = (a.Translation == null || a.Translation == string.Empty || a.Translation.Length == 0)
                                    ? string.Empty
                                    : a.Translation.Replace("\"", string.Empty),

                               Holmes = (from ap in a.Seasons
                                         .SelectMany(s => s.Episodes)
                                         .SelectMany(e => e.Appearances)
                                         where ap.CharacterID == HolmesId
                                         select ap).Any()
                                    ? (from ap in a.Seasons
                                       .SelectMany(s => s.Episodes)
                                       .SelectMany(e => e.Appearances)
                                       where ap.CharacterID == HolmesId
                                       group ap by ap.ActorID into grp
                                       orderby grp.Count() descending
                                       select grp.FirstOrDefault().Actor.Surname).FirstOrDefault()
                                    : string.Empty,

                               Year = a.Seasons
                              .OrderBy(s => s.AirOrder)
                              .FirstOrDefault()
                              .Episodes
                              .OrderBy(e => e.Airdate)
                              .FirstOrDefault()
                              .Airdate
                              .Year,

                               EpCount = a.Seasons
                              .SelectMany(s => s.Episodes)
                              .Count(),

                               Medium = a.Medium.Name
                           }).ToList();
        }

        public List<AdaptListAdapt> AdaptsSingleFilm { get; set; }

        public List<AdaptListAdapt> AdaptsFilm { get; set; }

        public List<AdaptListAdapt> AdaptsSingleRadio { get; set; }

        public List<AdaptListAdapt> AdaptsTV { get; set; }

        public List<AdaptListAdapt> AdaptsRadio { get; set; }

        public List<AdaptListAdapt> AdaptsOther { get; set; }
    }    

    public class AdaptListAdapt
    {
        public int ID;

        public string Name;

        public string Translation;

        public string Holmes;

        public int Year;

        public int EpCount;

        public string Medium;
    }
}