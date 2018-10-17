namespace HolmesMVC.Models.ViewModels
{
    using System.Collections.Generic;
    using System.Linq;
    using HolmesMVC.Enums;

    public class ActorHistorySummary
    {
        public ActorHistorySummary(int adapt, int character, List<Appearance> histories)
        {
            CharacterId = character;
            AdaptId = adapt;

            Count = histories.Count();
            
            var sampleApp = histories.First();

            var rename = sampleApp.GetRename();
            CharacterName = null == rename ? sampleApp.Character.LongName : Shared.LongName(rename);
            CharacterUrlName = sampleApp.Character.UrlName;
            MediumName = ((Medium)sampleApp.Episode.Season.Adaptation.Medium).ToString();
            AdaptName = sampleApp.Episode.Season.Adaptation.DisplayName;
            AdaptTranslation = sampleApp.Episode.Season.Adaptation.Translation;
            AdaptUrlName = sampleApp.Episode.Season.Adaptation.UrlName;
        }

        public int AdaptId { get; set; }

        public string AdaptUrlName { get; set; }

        public int CharacterId { get; set; }

        public int Count { get; set; }

        public string CharacterName { get; set; }

        public string CharacterUrlName { get; set; }

        public string MediumName { get; set; }

        public string AdaptName { get; set; }

        public string AdaptTranslation { get; set; }
    }
}
