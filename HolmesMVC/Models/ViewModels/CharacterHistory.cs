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
            AdaptName = Shared.DisplayName(ap.Episode.Season.Adaptation);

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

        public string AdaptName { get; set; }

        public string EpTranslation { get; set; }
    }
}
