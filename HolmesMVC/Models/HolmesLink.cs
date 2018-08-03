using System.Collections.Generic;

namespace HolmesMVC.Models
{
    public partial class HolmesLink
    {
        public HolmesLink()
        {
            HolmesLinkAppearances = new List<HolmesLinkAppearance>();
        }

        public int ID { get; set; }
        public int? EpisodeID { get; set; }
        public System.DateTime? Airdate { get; set; }
        public string Title { get; set; }
        public int AirdatePrecision { get; set; }
        public virtual Episode Episode { get; set; }
        public virtual ICollection<HolmesLinkAppearance> HolmesLinkAppearances { get; set; }
    }
}