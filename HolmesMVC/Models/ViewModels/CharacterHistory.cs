namespace HolmesMVC.Models.ViewModels
{
    using HolmesMVC.Enums;

    public class CharacterHistory
    {
        public CharacterHistory(Appearance ap)
        {
            EpId = ap.EpisodeID;
            EpName = Shared.DisplayName(ap.Episode);
            EpTranslation = ap.Episode.Translation;
            AdaptUrlName = ap.Episode.Season.Adaptation.UrlName;
            AdaptMediumUrlName = ap.Episode.Season.Adaptation.MediumUrlName;
            AirOrder = ap.Episode.AirOrder;
            SeasonAirOrder = ap.Episode.Season.AirOrder;
            AdaptName = ap.Episode.Season.Adaptation.DisplayName;
            SeasonCode = ap.Episode.SeasonCode;

            var airdatePrecision = (DatePrecision)ap.Episode.AirdatePrecision;
            var airdateFormat = "dd MMM yyyy";
            switch (airdatePrecision)
            {
                case DatePrecision.Month:
                    airdateFormat = "MMM yyyy";
                    break;
                case DatePrecision.Year:
                    airdateFormat = "yyyy";
                    break;
            }

            AirdateString = ap.Episode.Airdate.ToString(airdateFormat);
        }

        public string AirdateString { get; set; }

        public int EpId { get; set; }

        public string EpName { get; set; }

        public string AdaptUrlName { get; set; }

        public string AdaptMediumUrlName { get; set; }

        public int AirOrder { get; set; }

        public int SeasonAirOrder { get; set; }

        public string AdaptName { get; set; }

        public string SeasonCode { get; set; }

        public string EpTranslation { get; set; }
    }
}
