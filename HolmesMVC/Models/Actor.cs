using System;
using System.Collections.Generic;

namespace HolmesMVC.Models
{
    public partial class Actor
    {
        public Actor()
        {
            this.Appearances = new List<Appearance>();
            this.Renames = new List<Rename>();
        }

        private string _pic;
        private string _picCredit;

        public int ID { get; set; }
        public string Forename { get; set; }
        public string Surname { get; set; }
        public Nullable<System.DateTime> Birthdate { get; set; }
        public Nullable<System.DateTime> Deathdate { get; set; }
        public string Pic
        {
            get
            {
                if (_pic != null && _pic.IndexOf('.') > -1)
                {
                    return _pic;
                }
                else if (_pic == null || _pic == "")
                {
                    return null;
                }
                else
                {
                    return "/Content/ActorPhotos/" + _pic + ".jpg";
                }
            }
            set
            {
                _pic = value;
            }
        }
        public string PicCredit
        {
            get
            {
                if (_picCredit != null && _picCredit != "")
                {
                    return _picCredit;
                }
                else if (_pic != null && _pic.IndexOf('.') == -1)
                {
                    return "unknown - can you help?";
                }
                else
                {
                    return null;
                }
            }
            set
            {
                _picCredit = value;
            }
        }
        public string Middlenames { get; set; }
        public Nullable<int> Gender { get; set; }
        public Nullable<int> Species { get; set; }
        public string IMDb { get; set; }
        public string IMDbName { get; set; }
        public string Wikipedia { get; set; }
        public string Birthplace { get; set; }
        public int BirthdatePrecision { get; set; }
        public int DeathdatePrecision { get; set; }
        public virtual Gender Gender1 { get; set; }
        public virtual Species Species1 { get; set; }
        public virtual ICollection<Appearance> Appearances { get; set; }
        public virtual ICollection<Rename> Renames { get; set; }
    }
}
