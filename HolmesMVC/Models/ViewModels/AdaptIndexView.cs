namespace HolmesMVC.Models.ViewModels
{
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    public class AdaptIndexView
    {
        public AdaptIndexView(Adaptation adapt, int holmesId)
        {
            ID = adapt.ID;
            DisplayName = Shared.DisplayName(adapt);
            MediumName = adapt.Medium1.Name;

            // uses pre-calculated holmesId to reduce loading times
            var starActor = Shared.PlayedBy(holmesId, adapt).FirstOrDefault();
            if (null != starActor)
            {
                StarSurname = starActor.Surname;
                StarForename = starActor.Forename;
            }

            Year = (from e in adapt.Seasons.SelectMany(s => s.Episodes)
                    orderby e.Airdate
                    select e).First().Airdate.Year;
        }

        [Key]
        public int ID { get; set; }

        public string DisplayName { get; set; }

        public string MediumName { get; set; }

        public int Year { get; set; }

        public string StarSurname { get; set; }

        public string StarForename { get; set; }
    }
}