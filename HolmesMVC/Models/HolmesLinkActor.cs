using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HolmesMVC.Models
{
    public partial class HolmesLinkActor
    {
        public HolmesLinkActor()
        {
            this.HolmesLinkAppearances = new List<HolmesLinkAppearance>();
        }

        public int ID { get; set; }
        public int Actor { get; set; }
        public string Name { get; set; }
        public virtual Actor Actor1 { get; set; }
        public virtual ICollection<HolmesLinkAppearance> HolmesLinkAppearances { get; set; }
    }
}
