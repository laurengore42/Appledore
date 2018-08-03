using System.Collections.Generic;

namespace HolmesMVC.Models
{
    public partial class Species
    {
        public Species()
        {
            Actors = new List<Actor>();
            Characters = new List<Character>();
        }

        public int ID { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Actor> Actors { get; set; }
        public virtual ICollection<Character> Characters { get; set; }
    }
}
