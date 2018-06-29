namespace HolmesMVC.Models.ViewModels
{
    using System.Collections.Generic;
    using System.Linq;

    public class ActorHistorySummary
    {
        public ActorHistorySummary(int adapt, int character, List<Appearance> histories)
        {
            CharacterId = character;
            AdaptId = adapt;

            Count = histories.Count();
            
            var sampleApp = histories.First();

            var rename = Shared.GetRename(sampleApp);
            CharacterName = null == rename ? Shared.LongName(sampleApp.Character) : Shared.LongName(rename);
            MediumName = sampleApp.Episode.Season.Adaptation.Medium.Name;
            AdaptName = sampleApp.Episode.Season.Adaptation.Name
                        ?? Shared.DisplayName(
                            sampleApp.Episode.Season.Adaptation);
            AdaptTranslation = sampleApp.Episode.Season.Adaptation.Translation;
        }

        public int AdaptId { get; set; }

        public int CharacterId { get; set; }

        public int Count { get; set; }

        public string CharacterName { get; set; }

        public string MediumName { get; set; }

        public string AdaptName { get; set; }

        public string AdaptTranslation { get; set; }
    }
}
