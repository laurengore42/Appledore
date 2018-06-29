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
            AdaptId = r.AdaptationID;
            AdaptName = Shared.DisplayName(r.Adaptation);
            ActorId = r.ActorID;
            ActorName = Shared.ShortName(r.Actor);
            CharId = r.CharacterID;
            CharName = Shared.LongName(r.Character);
            Honorific = r.HonorificID == null ? null : r.Honorific.Name;
            Forename = r.Forename;
            Surname = r.Surname;
        }
    }
}