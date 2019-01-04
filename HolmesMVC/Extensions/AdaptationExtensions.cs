using System.Collections.Generic;
using System.Linq;
using HolmesMVC.Enums;
using HolmesMVC.Models;
using HolmesMVC.Models.ViewModels;

namespace HolmesMVC.Extensions
{
    public static class AdaptationExtensions
    {
        public static IQueryable<Adaptation> Canon(this IQueryable<Adaptation> adaptations)
        {
            return from a in adaptations
                   where a.Name == "Canon"
                   select a;
        }

        public static IQueryable<Adaptation> AdaptsFilm(this IQueryable<Adaptation> adaptations)
        {
            return from a in adaptations
                   where a.Medium == (int)Medium.Film
                   && a.Seasons.Any()
                   && a.Seasons.SelectMany(s => s.Episodes).Count() > 1
                   select a;
        }

        public static IQueryable<Adaptation> AdaptsSingleFilm(this IQueryable<Adaptation> adaptations)
        {
            return from a in adaptations
                   where a.Medium == (int)Medium.Film
                   && a.Seasons.Any()
                   && a.Seasons.SelectMany(s => s.Episodes).Count() == 1
                   select a;
        }

        public static IQueryable<Adaptation> AdaptsTV(this IQueryable<Adaptation> adaptations)
        {
            return from a in adaptations
                   where a.Medium == (int)Medium.Television
                   && a.Seasons.Any()
                   && a.Seasons.SelectMany(s => s.Episodes).Count() > 1
                   select a;
        }

        public static IQueryable<Adaptation> AdaptsSingleTV(this IQueryable<Adaptation> adaptations)
        {
            return from a in adaptations
                   where a.Medium == (int)Medium.Television
                   && a.Seasons.Any()
                   && a.Seasons.SelectMany(s => s.Episodes).Count() == 1
                   select a;
        }

        public static IQueryable<Adaptation> AdaptsRadio(this IQueryable<Adaptation> adaptations)
        {
            return from a in adaptations
                   where a.Medium == (int)Medium.Radio
                   && a.Seasons.Any()
                   && a.Seasons.SelectMany(s => s.Episodes).Count() > 1
                   select a;
        }

        public static IQueryable<Adaptation> AdaptsSingleRadio(this IQueryable<Adaptation> adaptations)
        {
            return from a in adaptations
                   where a.Medium == (int)Medium.Radio
                   && a.Seasons.Any()
                   && a.Seasons.SelectMany(s => s.Episodes).Count() == 1
                   select a;
        }

        public static IQueryable<Adaptation> AdaptsOther(this IQueryable<Adaptation> adaptations)
        {
            return from a in adaptations
                   where a.Medium != (int)Medium.Television && a.Medium != (int)Medium.Radio && a.Medium != (int)Medium.Film && a.Medium != (int)Medium.Stage // to_do_theatre
                   && a.Seasons.Any()
                   && a.Seasons.SelectMany(s => s.Episodes).Count() > 1
                   select a;
        }

        public static List<AdaptListAdapt> ToAdaptListAdapts(this IQueryable<Adaptation> adaptations)
        {
            return adaptations.ToList().Select(a => new AdaptListAdapt
            {
                ID = a.ID,

                Name =
                        (
                        !string.IsNullOrEmpty(a.Name)
                        ? a.Name.Replace("\"", string.Empty)
                        : (a.Seasons.Count() == 1 && a.Seasons.FirstOrDefault().Episodes.Count() == 1)
                        ? string.IsNullOrEmpty(a.Seasons.FirstOrDefault().Episodes.FirstOrDefault().Title)
                        ? a.Seasons.FirstOrDefault().Episodes.FirstOrDefault().Story.Name
                        : a.Seasons.FirstOrDefault().Episodes.FirstOrDefault().Title
                        : (a.Company + " " + (a.Medium == (int)Medium.Television ? "TV" : ((Medium)a.Medium).ToString()))
                        ).Replace("\"", string.Empty),

                Translation = string.IsNullOrEmpty(a.Translation)
                                    ? string.Empty
                                    : a.Translation.Replace("\"", string.Empty),

                Holmes = (from ap in a.Seasons
                          .SelectMany(s => s.Episodes)
                          .SelectMany(e => e.Appearances)
                          where ap.CharacterID == (int)CanonCharacter.Holmes
                          select ap).Any()
                                    ? (from ap in a.Seasons
                                       .SelectMany(s => s.Episodes)
                                       .SelectMany(e => e.Appearances)
                                       where ap.CharacterID == (int)CanonCharacter.Holmes
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

                Medium = ((Medium)a.Medium).ToString(),

                UrlName = a.UrlName
            })
            .ToList();
        }
    }
}