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
            CharacterName = null == rename ? Shared.LongName(sampleApp.Character1) : Shared.LongName(rename);
            MediumName = sampleApp.Episode1.Season1.Adaptation1.Medium1.Name;
            AdaptName = sampleApp.Episode1.Season1.Adaptation1.Name
                        ?? Shared.DisplayName(
                            sampleApp.Episode1.Season1.Adaptation1);
            AdaptTranslation = sampleApp.Episode1.Season1.Adaptation1.Translation;
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
