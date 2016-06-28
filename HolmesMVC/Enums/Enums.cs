namespace HolmesMVC.Enums
{
    public enum CanonOrder
    {
        Published = 0,
        Baring = 1
    }

    public enum DatePrecision
    {
        Full = 0,
        Month = 1,
        Season = 2,
        Year = 3
    }

    public enum Villain
    {
        None = 99,
        Theft = 1,
        Abduction = 11,
        Murder = 2,
        AttemptedMurder = 21,
        FrameForMurder = 22,
        Blackmail = 3,
        Spying = 4,
        Unclear = 0
    }

    public enum Outcome
    {
        None = 99,
        Released = 1,
        Escaped = 2,
        Arrested = 3,
        Died = 4,
        DiedInCustody = 41,
        Unclear = 0
    }
}