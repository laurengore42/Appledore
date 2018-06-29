namespace HolmesMVC.Models.ViewModels
{
    public class RenameView
    {
        public int ID;

        public int AdaptId;

        public string AdaptName;

        public int ActorId;

        public string ActorName;

        public int CharId;

        public string CharName;

        public string Honorific;

        public string Forename;

        public string Surname;

        public RenameView(Rename r)
        {
            ID = r.ID;
            AdaptId = r.Adaptation;
            AdaptName = Shared.DisplayName(r.Adaptation1);
            ActorId = r.Actor;
            ActorName = Shared.ShortName(r.Actor1);
            CharId = r.Character;
            CharName = Shared.LongName(r.Character1);
            Honorific = r.Honorific == null ? null : r.Honorific1.Name;
            Forename = r.Forename;
            Surname = r.Surname;
        }
    }
}