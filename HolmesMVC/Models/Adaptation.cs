using System;
using System.Collections.Generic;
using System.Linq;
using HolmesMVC.Enums;

namespace HolmesMVC.Models
{
    public partial class Adaptation
    {
        public Adaptation()
        {
            Renames = new List<Rename>();
            Seasons = new List<Season>();
        }

        public int ID { get; set; }
        public string Name { get; set; }
        public string Translation { get; set; }
        public int Medium { get; set; }
        public string Company { get; set; }
        public string UrlName { get; set; }
		public string Imdb { get; set; }
		public string Letterboxd { get; set; }
        public string MediumUrlName => Medium == (int)Enums.Medium.Radio ? "radio" : Medium == (int)Enums.Medium.Film ? "film" : Medium == (int)Enums.Medium.Television ? "tv" : "adaptation";

        public virtual ICollection<Rename> Renames { get; set; }
        public virtual ICollection<Season> Seasons { get; set; }

        public bool IsCanon
        {
            get
            {
                return Name == "Canon";
            }
        }

        /// <summary>
        /// returns '1984 Granada TV'
        /// </summary>
        public string DisplayName
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(Name))
                {
                    return Name;
                }

                if (Seasons.Count() == 1
                    && Seasons.First().Episodes.Count() == 1)
                {
                    return Seasons.First().Episodes.First().Title;
                }

                var medium = ((Medium)Medium).ToString();
                if (Medium == (int)Enums.Medium.Television)
                {
                    medium = "TV"; // special case
                }

                var company = string.IsNullOrWhiteSpace(Company) ? string.Empty : Company;

                var startYear = (from e in Seasons.SelectMany(s => s.Episodes)
                                 orderby e.Airdate
                                 select e.Airdate.Year).FirstOrDefault();

                return startYear > 0 ? (startYear + " " + company + " " + medium).Trim() : (company + " " + medium).Trim();
            }
        }

        public List<Actor> PlayedBy(CanonCharacter character)
        {
            return (from a in Seasons.SelectMany(s => s.Episodes).SelectMany(e => e.Appearances)
                    where a.CharacterID == (int)character
                    group a by a.Actor into grp
                    orderby grp.Count() descending
                    select grp.Key).ToList();
        }
    }
}
