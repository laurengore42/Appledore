using System.Collections.Generic;

namespace HolmesMVC.Models
{
    public partial class Season
    {
        public Season()
        {
            Episodes = new List<Episode>();
        }

        public int ID { get; set; }
        public string Name { get; set; }
        public string Translation { get; set; }
        public int AdaptationID { get; set; }
        public int AirOrder { get; set; }
        public virtual Adaptation Adaptation { get; set; }
        public virtual ICollection<Episode> Episodes { get; set; }
    }
}
