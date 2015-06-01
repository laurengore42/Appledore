using System;
using System.Collections.Generic;

namespace HolmesMVC.Models
{
    public partial class Appearance
    {
        public int ID { get; set; }
        public int Actor { get; set; }
        public int Character { get; set; }
        public int Episode { get; set; }
        public string Pic { get; set; }
        public virtual Actor Actor1 { get; set; }
        public virtual Character Character1 { get; set; }
        public virtual Episode Episode1 { get; set; }
    }
}
