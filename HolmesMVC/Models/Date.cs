using System;
using System.Collections.Generic;

namespace HolmesMVC.Models
{
    public partial class Date
    {
        public string StoryID { get; set; }
        public System.DateTime BaringGouldStart { get; set; }
        public Nullable<System.DateTime> BaringGouldEnd { get; set; }
        public string Watson { get; set; }
        public int BaringGouldPrecision { get; set; }
        public virtual Story Story { get; set; }
    }
}
