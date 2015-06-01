namespace HolmesMVC.Models.ViewModels
{
    using System;
    using System.Linq;
    using System.ComponentModel.DataAnnotations;

    using HolmesMVC.Enums;

    public class EpisodeView
    {
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true,
            DataFormatString = "{0:d MMMM yyyy}")]
        public DateTime Airdate { get; set; }

        public DatePrecision AirdatePrecision { get; set; }

        [Key]
        public int EpId { get; set; }

        public int EpNext { get; set; }
        public string EpNextName { get; set; }
        public string EpNextTranslation { get; set; }

        public int EpPrev { get; set; }
        public string EpPrevName { get; set; }
        public string EpPrevTranslation { get; set; }

        public int Adaptation { get; set; }
        public string AdaptationName { get; set; }
        public string AdaptationTranslation { get; set; }

        public int Season { get; set; }

        public string SeasonName { get; set; }

        public string Title { get; set; }

        public string Translation { get; set; }

        public string StoryCode { get; set; }

        public string Story { get; set; }

        public int SeasonAirOrder { get; set; }

        public string SeasonCode { get; set; }

        public bool Locked { get; set; }

        public EpisodeView()
        {
        }

        public EpisodeView(Episode episode)
        {
            var adapt = episode.Season1.Adaptation1;
            var adaptEps = adapt.Seasons.SelectMany(s => s.Episodes);
            var orderedList = (from e in adaptEps
                              orderby e.Airdate
                              select e.ID).ToList();

            var epLocation = orderedList.IndexOf(episode.ID);
            if (epLocation > 0)
            {
                EpPrev = orderedList[epLocation - 1];
                var episodePrev = (from e in adaptEps
                                   where e.ID == EpPrev
                                   select e).First();
                EpPrevName = Shared.DisplayName(episodePrev);
                EpPrevTranslation = episodePrev.Translation;
            }
            if (epLocation < orderedList.Count-1)
            {
                EpNext = orderedList[epLocation + 1];
                var episodeNext = (from e in adaptEps
                              where e.ID == EpNext
                              select e).First();
                EpNextName = Shared.DisplayName(episodeNext);
                EpNextTranslation = episodeNext.Translation;
            }

            Airdate = episode.Airdate;
            AirdatePrecision = (DatePrecision)episode.AirdatePrecision;
            EpId = episode.ID;
            Adaptation = episode.Season1.Adaptation;
            AdaptationName =
                string.IsNullOrWhiteSpace(episode.Season1.Adaptation1.Name)
                    ? Shared.DisplayName(episode.Season1.Adaptation1)
                    : episode.Season1.Adaptation1.Name;
            AdaptationTranslation = episode.Season1.Adaptation1.Translation;
            Season = episode.Season;
            SeasonAirOrder = episode.Season1.AirOrder;
            SeasonName = episode.Season1.Name;
            Title = episode.Title;
            Translation = episode.Translation;
            DisplayName = Shared.DisplayName(episode);
            MediumName = episode.Season1.Adaptation1.Medium1.Name;
            StoryCode = episode.Story;
            Story = episode.Story1 != null ? episode.Story1.Name : string.Empty;
            SeasonCode = Shared.GetSeasonCode(episode);

            var lockApp = from a in episode.Appearances
                          where a.Actor == 0
                          && a.Character == 0
                          && a.Episode == EpId
                          select a.ID;

            Locked = lockApp.Any();
        }

        public string DisplayName { get; set; }

        public string MediumName { get; set; }
    }
}