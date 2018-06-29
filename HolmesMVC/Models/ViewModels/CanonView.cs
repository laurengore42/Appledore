namespace HolmesMVC.Models.ViewModels
{
    using System.Collections.Generic;
    using System.Linq;

    using HolmesMVC.Enums;

    public class CanonView
    {
        public CanonView(Adaptation adapt,  UserProfile profile)
        {
            const int Publish = (int)CanonOrder.Published;
            const int Baring = (int)CanonOrder.Baring;

            Adaptation = new AdaptView(adapt);

            Stories = (from s in adapt.Seasons.SelectMany(s => s.Episodes).Select(e => e.Story1)
                       select s).ToList();

            UserCanonOrder = profile.PreferredCanonOrder;

            const int NumberOfCanonOptions = 3;

            EpisodeDateString = new string[NumberOfCanonOptions];
            StartYear = new int[NumberOfCanonOptions];
            StartMonth = new int[NumberOfCanonOptions];
            EndYear = new int[NumberOfCanonOptions];
            EndMonth = new int[NumberOfCanonOptions];

            StartYear[Publish] = Adaptation.DateOfFirstEpisode.Year;
            StartMonth[Publish] = Adaptation.DateOfFirstEpisode.Month;
            EndYear[Publish] = Adaptation.Episodes.OrderBy(a => a.Airdate).Last().Airdate.Year;
            EndMonth[Publish] = Adaptation.Episodes.OrderBy(a => a.Airdate).Last().Airdate.Month;

            StartYear[Baring] = Stories.OrderBy(a => a.Date.BaringGouldStart).First().Date.BaringGouldStart.Year;
            StartMonth[Baring] = Stories.OrderBy(a => a.Date.BaringGouldStart).First().Date.BaringGouldStart.Month;
            EndYear[Baring] = Stories.OrderBy(a => a.Date.BaringGouldStart).Last().Date.BaringGouldStart.Year;
            EndMonth[Baring] = Stories.OrderBy(a => a.Date.BaringGouldStart).Last().Date.BaringGouldStart.Month;


            EpisodeDateString[Publish] = string.Empty;
            foreach (var ep in Adaptation.Episodes.OrderBy(a => a.Airdate))
            {
                EpisodeDateString[Publish] += ep.Airdate.Year + ", " + ep.Airdate.Month + ", ";
            }

            EpisodeDateString[Publish] = EpisodeDateString[Publish].Substring(0, EpisodeDateString[Publish].Length - 2);
            EpisodeDateString[Publish] = "[" + EpisodeDateString[Publish] + "]";


            EpisodeDateString[Baring] = string.Empty;
            foreach (var ep in Stories.OrderBy(a => a.Date.BaringGouldStart))
            {
                EpisodeDateString[Baring] += ep.Date.BaringGouldStart.Year + ", " + ep.Date.BaringGouldStart.Month + ", ";
            }

            EpisodeDateString[Baring] = EpisodeDateString[Baring].Substring(0, EpisodeDateString[Baring].Length - 2);
            EpisodeDateString[Baring] = "[" + EpisodeDateString[Baring] + "]";
        }

        public int UserCanonOrder { get; set; }

        public AdaptView Adaptation { get; set; }

        public List<Story> Stories { get; set; }

        public string[] EpisodeDateString { get; set; }

        public int[] StartYear { get; set; }

        public int[] StartMonth { get; set; }

        public int[] EndYear { get; set; }

        public int[] EndMonth { get; set; }
    }
}