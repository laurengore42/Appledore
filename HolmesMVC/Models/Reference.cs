using System;
using System.Collections.Generic;

namespace HolmesMVC.Models
{
    public partial class Reference
    {
        public int ID { get; set; }
        public string Description { get; set; }
        public string StoryID { get; set; }
        public int EpisodeID { get; set; }
        public virtual Episode Episode { get; set; }
        public virtual Story Story { get; set; }
    }
}
