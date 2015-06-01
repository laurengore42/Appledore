using System;
using System.Collections.Generic;

namespace HolmesMVC.Models
{
    public partial class Medium
    {
        public Medium()
        {
            this.Adaptations = new List<Adaptation>();
        }

        public int ID { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Adaptation> Adaptations { get; set; }
    }
}
