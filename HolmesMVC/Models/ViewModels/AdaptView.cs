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
            DisplayName = adapt.DisplayName;
            Translation = adapt.Translation;
            Medium = adapt.Medium;
            MediumName = ((Medium)adapt.Medium).ToString();
            UrlName = adapt.UrlName;
            Company = adapt.Company;

            HolmesActors = adapt.PlayedBy(CanonCharacter.Holmes);
            WatsonActors = adapt.PlayedBy(CanonCharacter.Watson);

            Episodes = (from e in adapt.Seasons.SelectMany(s => s.Episodes)
                        orderby e.Airdate, e.ID
                        select e).ToList();

            if (Episodes.Any())
            {
                DateOfFirstEpisode = Episodes.First().Airdate;
            }

            SingleEpisode = Episodes.Count() == 1;
            SingleSeason = adapt.Seasons.Count() == 1;

            if (SingleEpisode)
            {
                ActorNames = from a in Episodes.First().Appearances
                             where a.ActorID > 0
                             select a.Actor.ShortName;
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

        public bool SingleEpisode { get; set; }

        public IEnumerable<string> ActorNames { get; set; }

        public DateTime DateOfFirstEpisode { get; set; }

        public List<Episode> Episodes { get; set; }

        public List<Appearance> Appearances { get; set; }
    }
}