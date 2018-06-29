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
            Name = Shared.DisplayName(e);
            var HolmesActor = Shared.PlayedBy(holmesId, e.Season1.Adaptation1).FirstOrDefault();
            Holmes = HolmesActor == null ? "(nobody)" : HolmesActor.Surname;
        }

        public int ID { get; set; }

        public DateTime Airdate { get; set; }

        public string Name { get; set; }

        public string Holmes { get; set; }
    }
}
