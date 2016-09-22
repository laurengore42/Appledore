using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HolmesMVC.Models
{
    public partial class HolmesLink
    {
        public HolmesLink()
        {
            this.HolmesLinkAppearances = new List<HolmesLinkAppearance>();
        }

        public int ID { get; set; }
        public int Episode { get; set; }
        public System.DateTime Airdate { get; set; }
        public string Title { get; set; }
        public int AirdatePrecision { get; set; }
        public virtual Episode Episode1 { get; set; }
        public virtual ICollection<HolmesLinkAppearance> HolmesLinkAppearances { get; set; }
    }
}