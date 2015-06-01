namespace HolmesMVC.Models.ViewModels
{
    using System.Collections.Generic;

    public class MultiActor
    {
        public Actor Actor { get; set; }

        public List<Character> Characters { get; set; } 
    }
}