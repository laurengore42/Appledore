namespace HolmesMVC.Models.ViewModels
{
    using System.Collections.Generic;
    using System.Linq;

    using HolmesMVC.Enums;
    using HolmesMVC.Models;

    public class StoryView
    {
        public StoryView(Story story)
        {
            var storyCode = story.ID;
            ID = storyCode.ToUpper();

            // the canon 'episode', i.e. story
            var rawEp = (from e in story.Episodes
                         where
                             null != e.Season1.Adaptation1.Name
                             && e.Season1.Adaptation1.Name == "Canon"
                         select e).FirstOrDefault();

            Episode = new EpisodeView(rawEp);

            Date = rawEp.Story1.Date;

            VillainType = rawEp.Story1.VillainType;
            OutcomeType = rawEp.Story1.OutcomeType;

            BaringGouldStartString = Shared.VagueDate(
                Date.BaringGouldStart,
                (DatePrecision)Date.BaringGouldPrecision,
                true,
                false);

            if (null != Date.BaringGouldEnd)
            {
                BaringGouldEndString = Shared.VagueDate(
                    Date.BaringGouldEnd,
                    (DatePrecision)Date.BaringGouldPrecision,
                    true,
                    false);
            }
            else
            {
                BaringGouldEndString = string.Empty;
            }

            KeefauverString = Shared.VagueDate(
                    Date.Keefauver,
                    DatePrecision.Full,
                    true,
                    false);

            // these episodes are all the adaptations of that story
            Adapteds = (from e in story.Episodes
                        where
                            (null == e.Season1.Adaptation1.Name
                             || e.Season1.Adaptation1.Name != "Canon")
                            && e.Season1.Adaptation1.Medium1.Name != "Stage" // to_do_theatre
                        select e).OrderBy(e => e.Airdate).ToList();
        }

        public string ID { get; set; }

        public EpisodeView Episode { get; set; }

        public Date Date { get; set; }

        public string BaringGouldStartString { get; set; }

        public string BaringGouldEndString { get; set; }

        public string KeefauverString { get; set; }

        public List<Episode> Adapteds { get; set; }

        public int ChunkStart { get; set; }

        public int ChunkLength { get; set; }

        public Villain VillainType { get; set; }

        public Outcome OutcomeType { get; set; }
    }
}