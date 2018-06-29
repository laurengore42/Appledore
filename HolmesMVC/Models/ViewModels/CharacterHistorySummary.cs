namespace HolmesMVC.Models.ViewModels
{
    using System.Collections.Generic;
    using System.Linq;

    public class CharacterHistorySummary
    {
        public CharacterHistorySummary(int actor, int adaptation, List<Appearance> groupedApps)
        {
            ActorId = actor;
            AdaptId = adaptation;

            Times = Shared.Times(groupedApps.Count());

            FirstYear =
                groupedApps.OrderBy(a => a.Episode1.Airdate)
                    .First()
                    .Episode1.Airdate.Year;
            LastYear =
                groupedApps.OrderBy(a => a.Episode1.Airdate)
                    .Last()
                    .Episode1.Airdate.Year;

            var sampleApp = groupedApps.First();
            ActorName = Shared.ShortName(sampleApp.Actor1);
            MediumName = sampleApp.Episode1.Season1.Adaptation1.Medium1.Name;
            AdaptName = Shared.DisplayName(sampleApp.Episode1.Season1.Adaptation1);
            AdaptTranslation = sampleApp.Episode1.Season1.Adaptation1.Translation;

            var rename = (from r in sampleApp.Episode1.Season1.Adaptation1.Renames
                              where r.Actor == ActorId
                              && r.Character == sampleApp.Character
                              select r).FirstOrDefault();
            if (null != rename)
            {
                Rename = Shared.LongName(new Character
                    {
                        Forename = rename.Forename,
                        Honorific = rename.Honorific,
                        Honorific1 = rename.Honorific1,
                        Surname = rename.Surname
                    });
            }

            Histories = (from ap in groupedApps select new CharacterHistory(ap)).ToList();
        }

        public int ActorId { get; set; }

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
    }
}