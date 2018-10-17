namespace HolmesMVC.Models.ViewModels
{
    using System;
    using System.Linq;

    public class AnnivEpisode
    {
        public AnnivEpisode(Episode e, int holmesId)
        {
            ID = e.ID;
            Airdate = e.Airdate;
            Name = e.DisplayName;
            AdaptMediumUrlName = e.Season.Adaptation.MediumUrlName;
            AdaptUrlName = e.Season.Adaptation.UrlName;
            AirOrder = e.AirOrder;
            SeasonAirOrder = e.Season.AirOrder;
            var HolmesActor = Shared.PlayedBy(holmesId, e.Season.Adaptation).FirstOrDefault();
            Holmes = HolmesActor == null ? "(nobody)" : HolmesActor.Surname;
        }

        public int ID { get; set; }

        public DateTime Airdate { get; set; }

        public string Name { get; set; }

        public string AdaptMediumUrlName { get; set; }

        public string AdaptUrlName { get; set; }

        public int AirOrder { get; set; }

        public string Holmes { get; set; }

        public int SeasonAirOrder { get; set; }
    }
}
