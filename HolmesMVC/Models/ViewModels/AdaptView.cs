namespace HolmesMVC.Models.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using HolmesMVC.Enums;

    public class AdaptView
    {
        public AdaptView(Adaptation adapt)
        {
            ID = adapt.ID;
            DisplayName = Shared.DisplayName(adapt);
            Translation = adapt.Translation;
            Medium = adapt.Medium;
            MediumName = ((Medium)adapt.Medium).ToString();
            UrlName = adapt.UrlName;
            Company = adapt.Company;

            HolmesActors = Shared.PlayedBy("Holmes", adapt);
            WatsonActors = Shared.PlayedBy("Watson", adapt);

            Episodes = (from e in adapt.Seasons.SelectMany(s => s.Episodes)
                        orderby e.Airdate, e.ID
                        select e).ToList();

            if (Episodes.Any())
            {
                DateOfFirstEpisode = Episodes.First().Airdate;
            }

            SingleFilm = Episodes.Count() == 1;
            SingleSeason = adapt.Seasons.Count() == 1;

            if (SingleFilm)
            {
                ActorNames = from a in Episodes.First().Appearances
                             where a.ActorID > 0
                             select Shared.ShortName(a.Actor);
            }
        }

        [Key]
        public int ID { get; set; }

        public string DisplayName { get; set; }

        public string Translation { get; set; }

        public int Medium { get; set; }

        public string MediumName { get; set; }

        public string UrlName { get; set; }

        public string Company { get; set; }

        public List<Actor> HolmesActors { get; set; }

        public List<Actor> WatsonActors { get; set; }

        public bool SingleSeason { get; set; }

        public bool SingleFilm { get; set; }

        public IEnumerable<string> ActorNames { get; set; }

        public DateTime DateOfFirstEpisode { get; set; }

        public List<Episode> Episodes { get; set; }

        public List<Appearance> Appearances { get; set; }
    }
}