using System;

namespace HolmesMVC.Models
{
    public partial class Rename
    {
        public int ID { get; set; }
        public int AdaptationID { get; set; }
        public int CharacterID { get; set; }
        public int ActorID { get; set; }
        public Nullable<int> HonorificID { get; set; }
        public string Forename { get; set; }
        public string Surname { get; set; }
        public virtual Actor Actor { get; set; }
        public virtual Adaptation Adaptation { get; set; }
        public virtual Character Character { get; set; }
        public virtual Honorific Honorific { get; set; }
    }
}
