using System;
using System.Collections.Generic;

namespace HolmesMVC.Models
{
    public partial class Honorific
    {
        public Honorific()
        {
            this.Characters = new List<Character>();
            this.Renames = new List<Rename>();
        }

        public int ID { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Character> Characters { get; set; }
        public virtual ICollection<Rename> Renames { get; set; }
    }
}
