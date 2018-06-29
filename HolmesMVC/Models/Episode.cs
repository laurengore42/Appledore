using System;
using System.Collections.Generic;

namespace HolmesMVC.Models
{
    public partial class Episode
    {
        public Episode()
        {
            this.HolmesLinks = new List<HolmesLink>();
            this.Appearances = new List<Appearance>();
            this.References = new List<Reference>();
        }

        public int ID { get; set; }
        public int SeasonID { get; set; }
        public string StoryID { get; set; }
        public System.DateTime Airdate { get; set; }
        public string Title { get; set; }
        public string Translation { get; set; }
        public int AirdatePrecision { get; set; }
        public virtual ICollection<Appearance> Appearances { get; set; }
        public virtual Season Season { get; set; }
        public virtual Story Story { get; set; }
        public virtual ICollection<Reference> References { get; set; }
        public virtual ICollection<HolmesLink> HolmesLinks { get; set; }
    }
}
