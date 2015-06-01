namespace HolmesMVC.Models.ViewModels
{
    using System;

    public class BirthdayActor
    {
        public BirthdayActor(Actor a)
        {
            ID = a.ID;
            Birthdate = (DateTime)a.Birthdate;
            Name = Shared.ShortName(a);
        }

        public int ID { get; set; }

        public DateTime Birthdate { get; set; }

        public string Name { get; set; }
    }
}
