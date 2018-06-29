using System;
using System.Collections.Generic;

namespace HolmesMVC.Models
{
    public partial class Reference
    {
        public int ID { get; set; }
        public string Description { get; set; }
        public string Story { get; set; }
        public int Episode { get; set; }
        public virtual Episode Episode1 { get; set; }
        public virtual Story Story1 { get; set; }
    }
}
