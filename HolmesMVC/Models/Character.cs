using System.Collections.Generic;
using System.Linq;

namespace HolmesMVC.Models
{
    public partial class Character
    {
        public Character()
        {
            Appearances = new List<Appearance>();
            Renames = new List<Rename>();
        }

        public int ID { get; set; }
        public string Forename { get; set; }
        public string Surname { get; set; }
        public int? SpeciesID { get; set; }
        public int? HonorificID { get; set; }
        public string StoryID { get; set; }
        public string Wikipedia { get; set; }
        public string UrlName { get; set; }
        public virtual ICollection<Appearance> Appearances { get; set; }
        public virtual Honorific Honorific { get; set; }
        public virtual Species Species { get; set; }
        public virtual ICollection<Rename> Renames { get; set; }
        public virtual Story Story { get; set; }

        public bool IsCanon
        {
            get {
                return (from a in Appearances
                        where a.Episode.Season.Adaptation.IsCanon
                        select a).Any();
            }
        }

        // returns 'Professor Moriarty' || 'James Moriarty'
        public string ShortName
        {
            get
            {
                var forename = Forename;
                if (string.IsNullOrWhiteSpace(forename) && HonorificID != null)
                {
                    forename = Honorific.Name;
                }

                var surname = Surname;
                return Shared.BuildName(new[] { forename, surname }, ' ');
            }
        }

        // returns 'Professor James Moriarty'
        public string LongName
        {
            get
            {
                var honorific = Honorific != null ? Honorific.Name : string.Empty;
                var forename = Forename ?? string.Empty;
                var surname = Surname ?? string.Empty;

                return Shared.BuildName(new[] { honorific, forename, surname }, ' ');
            }
        }

        // returns 'Moriarty, Professor James'
        public string DisplayName
        {
            get
            {
                return !string.IsNullOrWhiteSpace(Forename)
                           ? HonorificID != null
                                 ? Surname + ", " + Honorific.Name + " "
                                   + Forename
                                 : Surname + ", " + Forename
                           : HonorificID != null
                                 ? Surname + ", " + Honorific.Name
                                 : Surname;
            }
        }
    }
}
