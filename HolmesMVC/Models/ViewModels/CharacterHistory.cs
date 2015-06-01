namespace HolmesMVC.Models.ViewModels
{
    using HolmesMVC.Enums;

    public class CharacterHistory
    {
        public CharacterHistory(Appearance ap)
        {
            EpId = ap.Episode;
            EpName = Shared.DisplayName(ap.Episode1);
            EpTranslation = ap.Episode1.Translation;
            AdaptName = Shared.DisplayName(ap.Episode1.Season1.Adaptation1);

            var airdatePrecision = (DatePrecision)ap.Episode1.AirdatePrecision;
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

            AirdateString = ap.Episode1.Airdate.ToString(airdateFormat);
        }

        public string AirdateString { get; set; }

        public int EpId { get; set; }

        public string EpName { get; set; }

        public string AdaptName { get; set; }

        public string EpTranslation { get; set; }
    }
}
