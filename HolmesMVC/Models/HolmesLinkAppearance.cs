using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HolmesMVC.Models
{
    public partial class HolmesLinkAppearance
    {
        public int ID { get; set; }
        public int HolmesLinkActor { get; set; }
        public int HolmesLink { get; set; }
        public virtual HolmesLinkActor HolmesLinkActor1 { get; set; }
        public virtual HolmesLink HolmesLink1 { get; set; }
    }
}