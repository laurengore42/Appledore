namespace HolmesMVC.Models
{
    public partial class Appearance
    {
        public int ID { get; set; }
        public int ActorID { get; set; }
        public int CharacterID { get; set; }
        public int EpisodeID { get; set; }
        public string Pic { get; set; }
        public virtual Actor Actor { get; set; }
        public virtual Character Character { get; set; }
        public virtual Episode Episode { get; set; }
    }
}
