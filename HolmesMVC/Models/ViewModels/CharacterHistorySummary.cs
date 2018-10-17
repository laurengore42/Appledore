namespace HolmesMVC.Models.ViewModels
{
    using System.Collections.Generic;
    using System.Linq;
    using HolmesMVC.Enums;

    public class CharacterHistorySummary
    {
        public CharacterHistorySummary(int actor, int adaptation, List<Appearance> groupedApps)
        {
            ActorId = actor;
            AdaptId = adaptation;

            Times = Shared.Times(groupedApps.Count());

            FirstYear =
                groupedApps.OrderBy(a => a.Episode.Airdate)
                    .First()
                    .Episode.Airdate.Year;
            LastYear =
                groupedApps.OrderBy(a => a.Episode.Airdate)
                    .Last()
                    .Episode.Airdate.Year;

            var sampleApp = groupedApps.First();
            ActorName = sampleApp.Actor.ShortName;
            ActorUrlName = sampleApp.Actor.UrlName;
            MediumName = ((Medium)sampleApp.Episode.Season.Adaptation.Medium).ToString();
            AdaptName = sampleApp.Episode.Season.Adaptation.DisplayName;
            AdaptTranslation = sampleApp.Episode.Season.Adaptation.Translation;
            AdaptUrlName = sampleApp.Episode.Season.Adaptation.UrlName;

            var rename = (from r in sampleApp.Episode.Season.Adaptation.Renames
                              where r.ActorID == ActorId
                              && r.CharacterID == sampleApp.CharacterID
                              select r).FirstOrDefault();
            if (null != rename)
            {
                Rename = new Character
                    {
                        Forename = rename.Forename,
                        HonorificID = rename.HonorificID,
                        Honorific = rename.Honorific,
                        Surname = rename.Surname
                    }.LongName;
            }

            Histories = (from ap in groupedApps select new CharacterHistory(ap)).ToList();
        }

        public int ActorId { get; set; }

        public string ActorUrlName { get; set; }

        public int AdaptId { get; set; }

        public string Times { get; set; }

        public string ActorName { get; set; }

        public string MediumName { get; set; }

        public string AdaptName { get; set; }

        public string Rename { get; set; }

        public List<CharacterHistory> Histories { get; set; }

        public int FirstYear { get; set; }

        public int LastYear { get; set; }

        public string AdaptTranslation { get; set; }

        public string AdaptUrlName { get; set; }
    }
}