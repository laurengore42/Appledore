using System.Collections.Generic;

namespace HolmesMVC.Models
{
    public partial class Adaptation
    {
        public Adaptation()
        {
            Renames = new List<Rename>();
            Seasons = new List<Season>();
        }

        public int ID { get; set; }
        public string Name { get; set; }
        public string Translation { get; set; }
        public int Medium { get; set; }
        public string Company { get; set; }
        public string UrlName { get; set; }
        public virtual ICollection<Rename> Renames { get; set; }
        public virtual ICollection<Season> Seasons { get; set; }
    }
}
