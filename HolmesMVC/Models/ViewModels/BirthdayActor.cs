namespace HolmesMVC.Models.ViewModels
{
    using System;

    public class BirthdayActor
    {
        public BirthdayActor(Actor a)
        {
            UrlName = a.UrlName;
            Birthdate = (DateTime)a.Birthdate;
            Name = a.ShortName;
        }

        public string UrlName { get; set; }

        public DateTime Birthdate { get; set; }

        public string Name { get; set; }
    }
}
