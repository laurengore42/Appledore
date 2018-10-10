using System.Collections.Generic;
using System.Linq;

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
        public string MediumUrlName => Medium == (int)Enums.Medium.Radio ? "radio" : Seasons.SelectMany(s => s.Episodes).Count() == 1 ? "film" : Medium == (int)Enums.Medium.Television ? "tv" : "adaptation";

        public virtual ICollection<Rename> Renames { get; set; }
        public virtual ICollection<Season> Seasons { get; set; }
    }
}
