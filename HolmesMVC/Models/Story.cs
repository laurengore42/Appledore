using System;
using System.Collections.Generic;

namespace HolmesMVC.Models
{
    public partial class Story
    {
        public Story()
        {
            this.Episodes = new List<Episode>();
            this.References = new List<Reference>();
        }

        public string ID { get; set; }
        public string Name { get; set; }
        public virtual Date Date { get; set; }
        public virtual ICollection<Episode> Episodes { get; set; }
        public virtual ICollection<Reference> References { get; set; }
    }
}
