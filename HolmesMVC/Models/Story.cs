using HolmesMVC.Enums;
using System.Collections.Generic;

namespace HolmesMVC.Models
{

    public partial class Story
    {
        public Story()
        {
            Episodes = new List<Episode>();
            References = new List<Reference>();
            Characters = new List<Character>();
        }

        public string ID { get; set; }
        public string Name { get; set; }
        public Villain VillainType { get; set; }
        public Outcome OutcomeType { get; set; }
        public virtual Date Date { get; set; }
        public virtual ICollection<Episode> Episodes { get; set; }
        public virtual ICollection<Reference> References { get; set; }
        public virtual ICollection<Character> Characters { get; set; }
    }
}
