namespace HolmesMVC.Models.ViewModels
{
    using System;

    using HolmesMVC.Enums;

    public class ActorHistory
    {
        public ActorHistory(Appearance ap)
        {
            Airdate = ap.Episode1.Airdate;
            AirdatePrecision = (DatePrecision)ap.Episode1.AirdatePrecision;
            var rename = Shared.GetRename(ap);
            CharacterName = null == rename ? Shared.LongName(ap.Character1) : Shared.LongName(rename);
            EpId = ap.Episode;
            AdaptName = ap.Episode1.Season1.Adaptation1.Name
                        ?? Shared.DisplayName(ap.Episode1.Season1.Adaptation1);
            EpName = Shared.DisplayName(ap.Episode1);
            EpTranslation = ap.Episode1.Translation;
            SeasonCode = Shared.GetSeasonCode(ap.Episode1);
            MediumName = ap.Episode1.Season1.Adaptation1.Medium1.Name;
        }

        public DateTime Airdate { get; set; }

        public DatePrecision AirdatePrecision { get; set; }

        public string AdaptName { get; set; }

        public string EpName { get; set; }

        public string CharacterName { get; set; }

        public string SeasonCode { get; set; }

        public int EpId { get; set; }

        public string MediumName { get; set; }

        public string EpTranslation { get; set; }
    }
}
