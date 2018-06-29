using System;
using System.Collections.Generic;

namespace HolmesMVC.Models
{
    public partial class Adaptation
    {
        public Adaptation()
        {
            this.Renames = new List<Rename>();
            this.Seasons = new List<Season>();
        }

        public int ID { get; set; }
        public string Name { get; set; }
        public string Translation { get; set; }
        public int MediumID { get; set; }
        public string Company { get; set; }
        public virtual Medium Medium { get; set; }
        public virtual ICollection<Rename> Renames { get; set; }
        public virtual ICollection<Season> Seasons { get; set; }
    }
}
