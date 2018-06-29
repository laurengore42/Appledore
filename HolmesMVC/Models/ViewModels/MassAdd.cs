namespace HolmesMVC.Models.ViewModels
{
    public class MassAdd
    {
        public int ActorID { get; set; }

        public int CharacterID { get; set; }

        public int AdaptationID { get; set; }

        public int[] Episodes { get; set; }

        public virtual Actor Actor { get; set; }

        public virtual Character Character { get; set; }

        public virtual Adaptation Adaptation { get; set; }
    }
}