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
        public Nullable<int> Gender { get; set; }
        public Nullable<int> Species { get; set; }
        public Nullable<int> Honorific { get; set; }
        public string Wikipedia { get; set; }
        public virtual ICollection<Appearance> Appearances { get; set; }
        public virtual Gender Gender1 { get; set; }
        public virtual Honorific Honorific1 { get; set; }
        public virtual Species Species1 { get; set; }
        public virtual ICollection<Rename> Renames { get; set; }
    }
}
