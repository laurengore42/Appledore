namespace HolmesMVC.Models.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
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
        public int EpNextAirOrder { get; set; }
        public int EpNextSeason { get; set; }
        public string EpNextName { get; set; }
        public string EpNextTranslation { get; set; }

        public int EpPrev { get; set; }
        public int EpPrevAirOrder { get; set; }
        public int EpPrevSeason { get; set; }
        public string EpPrevName { get; set; }
        public string EpPrevTranslation { get; set; }

        public int Adaptation { get; set; }
        public string AdaptationName { get; set; }
        public string AdaptationTranslation { get; set; }
        public string AdaptationMediumUrlName { get; set; }
        public string AdaptationUrlName { get; set; }

        public int Season { get; set; }

        public string SeasonName { get; set; }

        public string Title { get; set; }

        public string Translation { get; set; }

        public string Poster { get; set; }

        public string StoryCode { get; set; }

        public string Story { get; set; }

        public int AirOrder { get; set; }

        public int SeasonAirOrder { get; set; }

        public string SeasonCode { get; set; }

        public bool Locked { get; set; }

        public IEnumerable<string> ActorNames { get; set; }

        public EpisodeView()
        {
        }

        public EpisodeView(Episode episode)
        {
            var adapt = episode.Season.Adaptation;
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
                EpPrevAirOrder = episodePrev.AirOrder;
                EpPrevSeason = episodePrev.Season.AirOrder;
                EpPrevName = Shared.DisplayName(episodePrev);
                EpPrevTranslation = episodePrev.Translation;
            }
            if (epLocation < orderedList.Count-1)
            {
                EpNext = orderedList[epLocation + 1];
                var episodeNext = (from e in adaptEps
                              where e.ID == EpNext
                              select e).First();
                EpNextAirOrder = episodeNext.AirOrder;
                EpNextSeason = episodeNext.Season.AirOrder;
                EpNextName = Shared.DisplayName(episodeNext);
                EpNextTranslation = episodeNext.Translation;
            }
            
            Airdate = episode.Airdate;
            AirdatePrecision = (DatePrecision)episode.AirdatePrecision;
            EpId = episode.ID;
            Adaptation = episode.Season.AdaptationID;
            AdaptationName = episode.Season.Adaptation.DisplayName;
            AdaptationTranslation = episode.Season.Adaptation.Translation;
            AdaptationMediumUrlName = episode.Season.Adaptation.MediumUrlName;
            AdaptationUrlName = episode.Season.Adaptation.UrlName;
            AirOrder = episode.AirOrder;
            Season = episode.SeasonID;
            SeasonAirOrder = episode.Season.AirOrder;
            SeasonName = episode.Season.Name;
            Title = episode.Title;
            Translation = episode.Translation;
            Poster = episode.Poster;
            DisplayName = Shared.DisplayName(episode);
            Medium = episode.Season.Adaptation.Medium;
            MediumName = ((Medium)episode.Season.Adaptation.Medium).ToString();
            StoryCode = episode.StoryID;
            Story = episode.Story != null ? episode.Story.Name : string.Empty;
            SeasonCode = episode.SeasonCode;

            var lockApp = from a in episode.Appearances
                          where a.ActorID == 0
                          && a.CharacterID == 0
                          && a.EpisodeID == EpId
                          select a.ID;

            Locked = lockApp.Any();

            ActorNames = from a in episode.Appearances
                         where a.ActorID > 0
                         select a.Actor.ShortName;
        }

        public string DisplayName { get; set; }

        public int Medium { get; set; }

        public string MediumName { get; set; }
    }
}