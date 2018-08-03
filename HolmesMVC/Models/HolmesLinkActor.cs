using System.Collections.Generic;

namespace HolmesMVC.Models
{
    public partial class HolmesLinkActor
    {
        public HolmesLinkActor()
        {
            HolmesLinkAppearances = new List<HolmesLinkAppearance>();
        }

        public int ID { get; set; }
        public int? ActorID { get; set; }
        public string Name { get; set; }
        public virtual Actor Actor { get; set; }
        public virtual ICollection<HolmesLinkAppearance> HolmesLinkAppearances { get; set; }
    }
}
