using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace HolmesMVC.Models
{
    public partial class Episode
    {
        public Episode()
        {
            HolmesLinks = new List<HolmesLink>();
            Appearances = new List<Appearance>();
            References = new List<Reference>();
        }

        public int ID { get; set; }
        public int SeasonID { get; set; }
        public string StoryID { get; set; }
        public string Poster { get; set; }
        public System.DateTime Airdate { get; set; }
        public string Title { get; set; }
        public string Translation { get; set; }
        public int AirdatePrecision { get; set; }
        public string SeasonCode => Season.Adaptation.Seasons.Count() == 1 ? AirOrder.ToString(CultureInfo.InvariantCulture) : AirOrder < 10 ? Season.AirOrder + "x0" + AirOrder : Season.AirOrder + "x" + AirOrder;
        public int AirOrder => Season.Episodes.OrderBy(e => e.Airdate).ThenBy(e => e.Title).Select((episode, index) => new { episode.ID, Rank = index + 1 }).Where(e => e.ID == ID).FirstOrDefault().Rank;
        public virtual ICollection<Appearance> Appearances { get; set; }
        public virtual Season Season { get; set; }
        public virtual Story Story { get; set; }
        public virtual ICollection<Reference> References { get; set; }
        public virtual ICollection<HolmesLink> HolmesLinks { get; set; }


        public string DisplayName
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(Title))
                {
                    return Title;
                }
                if (!string.IsNullOrWhiteSpace(StoryID))
                {
                    return Story.Name;
                }
                return "Error in Episode DisplayName!";
            }
        }
    }
}
