namespace HolmesMVC.Models
{
    public partial class Rename
    {
        public int ID { get; set; }
        public int AdaptationID { get; set; }
        public int CharacterID { get; set; }
        public int ActorID { get; set; }
        public int? HonorificID { get; set; }
        public string Forename { get; set; }
        public string Surname { get; set; }
        public virtual Actor Actor { get; set; }
        public virtual Adaptation Adaptation { get; set; }
        public virtual Character Character { get; set; }
        public virtual Honorific Honorific { get; set; }


        // returns 'Professor James Moriarty'
        public string LongName
        {
            get
            {
                return new Character
                    {
                        HonorificID = HonorificID,
                        Honorific = Honorific,
                        Forename = Forename,
                        Surname = Surname
                    }.LongName;
            }
        }
    }
}
