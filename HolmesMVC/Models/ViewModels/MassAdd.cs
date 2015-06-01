namespace HolmesMVC.Models.ViewModels
{
    public class MassAdd
    {
        public int Actor { get; set; }

        public int Character { get; set; }

        public int Adaptation { get; set; }

        public int[] Episodes { get; set; }

        public virtual Actor Actor1 { get; set; }

        public virtual Character Character1 { get; set; }

        public virtual Adaptation Adaptation1 { get; set; }
    }
}