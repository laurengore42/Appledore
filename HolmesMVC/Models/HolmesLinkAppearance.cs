using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HolmesMVC.Models
{
    public partial class HolmesLinkAppearance
    {
        public int ID { get; set; }
        public int HolmesLinkActorID { get; set; }
        public int HolmesLinkID { get; set; }
        public virtual HolmesLinkActor HolmesLinkActor { get; set; }
        public virtual HolmesLink HolmesLink { get; set; }
    }
}