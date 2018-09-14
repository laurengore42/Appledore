namespace HolmesMVC.Models.ViewModels
{
    using System;

    public class BirthdayActor
    {
        public BirthdayActor(Actor a)
        {
            UrlName = a.UrlName;
            Birthdate = (DateTime)a.Birthdate;
            Name = Shared.ShortName(a);
        }

        public string UrlName { get; set; }

        public DateTime Birthdate { get; set; }

        public string Name { get; set; }
    }
}
