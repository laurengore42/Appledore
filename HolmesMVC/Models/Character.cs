using System;
using System.Collections.Generic;

namespace HolmesMVC.Models
{
    public partial class Character
    {
        public Character()
        {
            this.Appearances = new List<Appearance>();
            this.Renames = new List<Rename>();
        }

        public int ID { get; set; }
        public string Forename { get; set; }
        public string Surname { get; set; }
        public Nullable<int> GenderID { get; set; }
        public Nullable<int> SpeciesID { get; set; }
        public Nullable<int> HonorificID { get; set; }
        public string StoryID { get; set; }
        public string Wikipedia { get; set; }
        public virtual ICollection<Appearance> Appearances { get; set; }
        public virtual Gender Gender { get; set; }
        public virtual Honorific Honorific { get; set; }
        public virtual Species Species { get; set; }
        public virtual ICollection<Rename> Renames { get; set; }
        public virtual Story Story { get; set; }
    }
}
