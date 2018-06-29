using System;

namespace HolmesMVC.Models
{
    public partial class Rename
    {
        public int ID { get; set; }
        public int Adaptation { get; set; }
        public int Character { get; set; }
        public int Actor { get; set; }
        public Nullable<int> Honorific { get; set; }
        public string Forename { get; set; }
        public string Surname { get; set; }
        public virtual Actor Actor1 { get; set; }
        public virtual Adaptation Adaptation1 { get; set; }
        public virtual Character Character1 { get; set; }
        public virtual Honorific Honorific1 { get; set; }
    }
}
