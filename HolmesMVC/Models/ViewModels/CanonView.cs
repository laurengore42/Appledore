namespace HolmesMVC.Models.ViewModels
{
    using System.Collections.Generic;
    using System.Linq;

    using HolmesMVC.Enums;

    public class CanonView
    {
        public CanonView(Adaptation adapt, UserProfile profile)
        {
            const int Publish = (int)CanonOrder.Published;
            const int Baring = (int)CanonOrder.Baring;

            Adaptation = new AdaptView(adapt);

            Stories = (from s in adapt.Seasons.SelectMany(s => s.Episodes).Select(e => e.Story)
                       select s).ToList();

            UserCanonOrder = profile.PreferredCanonOrder;

            const int NumberOfCanonOptions = 3;

            episodeDateString = new string[NumberOfCanonOptions];
            startYear = new int[NumberOfCanonOptions];
            startMonth = new int[NumberOfCanonOptions];
            endYear = new int[NumberOfCanonOptions];
            endMonth = new int[NumberOfCanonOptions];

            startYear[Publish] = Adaptation.DateOfFirstEpisode.Year;
            startMonth[Publish] = Adaptation.DateOfFirstEpisode.Month;
            endYear[Publish] = Adaptation.Episodes.OrderBy(a => a.Airdate).Last().Airdate.Year;
            endMonth[Publish] = Adaptation.Episodes.OrderBy(a => a.Airdate).Last().Airdate.Month;

            startYear[Baring] = Stories.OrderBy(a => a.Date.BaringGouldStart).First().Date.BaringGouldStart.Year;
            startMonth[Baring] = Stories.OrderBy(a => a.Date.BaringGouldStart).First().Date.BaringGouldStart.Month;
            endYear[Baring] = Stories.OrderBy(a => a.Date.BaringGouldStart).Last().Date.BaringGouldStart.Year;
            endMonth[Baring] = Stories.OrderBy(a => a.Date.BaringGouldStart).Last().Date.BaringGouldStart.Month;


            episodeDateString[Publish] = string.Empty;
            foreach (var ep in Adaptation.Episodes.OrderBy(a => a.Airdate))
            {
                episodeDateString[Publish] += ep.Airdate.Year + ", " + ep.Airdate.Month + ", ";
            }

            episodeDateString[Publish] = episodeDateString[Publish].Substring(0, episodeDateString[Publish].Length - 2);
            episodeDateString[Publish] = "[" + episodeDateString[Publish] + "]";


            episodeDateString[Baring] = string.Empty;
            foreach (var ep in Stories.OrderBy(a => a.Date.BaringGouldStart))
            {
                episodeDateString[Baring] += ep.Date.BaringGouldStart.Year + ", " + ep.Date.BaringGouldStart.Month + ", ";
            }

            episodeDateString[Baring] = episodeDateString[Baring].Substring(0, episodeDateString[Baring].Length - 2);
            episodeDateString[Baring] = "[" + episodeDateString[Baring] + "]";
        }


        private readonly string[] episodeDateString;
        private readonly int[] startYear;
        private readonly int[] startMonth;
        private readonly int[] endYear;
        private readonly int[] endMonth;

        public int UserCanonOrder { get; set; }

        public AdaptView Adaptation { get; set; }

        public List<Story> Stories { get; set; }

        public IReadOnlyCollection<string> EpisodeDateString
        {
            get
            {
                return episodeDateString;
            }
        }

        public IReadOnlyCollection<int> StartYear
        {
            get
            {
                return startYear;
            }
        }

        public IReadOnlyCollection<int> StartMonth
        {
            get
            {
                return startMonth;
            }
        }

        public IReadOnlyCollection<int> EndYear
        {
            get
            {
                return endYear;
            }
        }

        public IReadOnlyCollection<int> EndMonth
        {
            get
            {
                return endMonth;
            }
        }
    }
}