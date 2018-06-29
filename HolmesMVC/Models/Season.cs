using System;
using System.Collections.Generic;

namespace HolmesMVC.Models
{
    public partial class Season
    {
        public Season()
        {
            this.Episodes = new List<Episode>();
        }

        public int ID { get; set; }
        public string Name { get; set; }
        public string Translation { get; set; }
        public int Adaptation { get; set; }
        public int AirOrder { get; set; }
        public virtual Adaptation Adaptation1 { get; set; }
        public virtual ICollection<Episode> Episodes { get; set; }
    }
}
