namespace HolmesMVC.Models.ViewModels
{

    using System.Collections.Generic;
    using System.Linq;

    public class CanonSearchView
    {
        public string Query;

        public List<CanonSearchNode> Nodes;

        public CanonSearchView()
        {
            Nodes = new List<CanonSearchNode>();
        }

    }

    public class CanonSearchNode
    {
        public string Snippet;

        public Story Story;
    }

}