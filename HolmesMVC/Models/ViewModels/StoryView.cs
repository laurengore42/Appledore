namespace HolmesMVC.Models.ViewModels
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Web.Hosting;
    using System.Xml;

    using HolmesMVC.Enums;
    using HolmesMVC.Models;

    public class StoryView
    {
        public StoryView(Story story, int chunkStart, int chunkLength)
        {
            var storyCode = story.ID;
            ID = storyCode.ToUpper();

            // the canon 'episode', i.e. story
            var rawEp = (from e in story.Episodes
                         where
                             null != e.Season.Adaptation.Name
                             && e.Season.Adaptation.Name == "Canon"
                         select e).FirstOrDefault();

            Episode = new EpisodeView(rawEp);

            Date = rawEp.Story.Date;

            VillainType = rawEp.Story.VillainType;
            OutcomeType = rawEp.Story.OutcomeType;

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

            // these episodes are all the adaptations of that story
            Adapteds = (from e in story.Episodes
                        where
                            (null == e.Season.Adaptation.Name
                             || e.Season.Adaptation.Name != "Canon")
                            && e.Season.Adaptation.Medium.Name != "Stage" // to_do_theatre
                        select e).OrderBy(e => e.Airdate).ToList();

            GetChunk(chunkStart, chunkLength);
        }

        public string ID { get; set; }

        public EpisodeView Episode { get; set; }

        public Date Date { get; set; }

        public string BaringGouldStartString { get; set; }

        public string BaringGouldEndString { get; set; }

        public List<Episode> Adapteds { get; set; }

        public Villain VillainType { get; set; }

        public Outcome OutcomeType { get; set; }

        public string ChunkXml { get; set; }
        public string ChunkText { get; set; }

        private void GetChunk(int chunkStart, int chunkLength)
        {
            ChunkXml = null;
            ChunkText = null;
            if (chunkStart >= 0 && chunkLength >= 0)
            {
                var storyXml = GetStoryXml();
                if (storyXml != null)
                {
                    var storyWithoutTags = HtmlTrim(storyXml);
                    var storyChunk = storyWithoutTags.Substring(chunkStart, chunkLength);
                    var linebreaker = storyChunk;
                    while (linebreaker.Contains("*¦"))
                    {
                        linebreaker = linebreaker.Replace("*¦", "</p><p>");
                    }
                    ChunkXml = "<p>" + linebreaker + "</p>";
                    ChunkText = Regex.Replace(ChunkXml, @"<.*?>", " ");
                    while (ChunkText.Contains("  "))
                    {
                        ChunkText = ChunkText.Replace("  ", " ");
                    }
                    ChunkText = ChunkText.Trim();
                }
            }
        }

        private string GetStoryXml()
        {
            var xmlDoc = new XmlDocument();
            var storyUrl = HostingEnvironment.MapPath("/Services/Stories/" + ID + ".xml");
            if (storyUrl != null)
            {
                xmlDoc.Load(storyUrl);
                return xmlDoc.DocumentElement.OuterXml;
            }
            return null;
        }

        private static string HtmlTrim(string storyWithTags)
        {
            var tagless = storyWithTags;

            // special case for line breaks
            tagless = Regex.Replace(tagless, @"<br.*?>", "*¦");
            tagless = Regex.Replace(tagless, @"<\/?p.*?>", "*¦");
            tagless = Regex.Replace(tagless, @"<\/?div.*?>", "*¦");

            // kill all tags
            tagless = Regex.Replace(tagless, @"<.*?>", "");

            // kill newlines
            tagless = Regex.Replace(tagless, @"\r", "");
            tagless = Regex.Replace(tagless, @"\n{2,}", "*¦");
            tagless = Regex.Replace(tagless, @"\n", "*¦");

            // tidy
            while (tagless.Contains("*¦*¦"))
            {
                tagless = tagless.Replace("*¦*¦", "*¦");
            }
            while (tagless.Contains("*¦ "))
            {
                tagless = tagless.Replace("*¦ ", "*¦");
            }
            while (tagless.Contains(" *¦"))
            {
                tagless = tagless.Replace(" *¦", "*¦");
            }

            // whitespace handling
            while (tagless.Contains("  "))
            {
                tagless = tagless.Replace("  ", " ");
            }
            tagless = tagless.Trim();

            while (tagless.IndexOf("*¦") == 0)
            {
                // Sometimes this has to be done twice. Don't ask me why
                // If you don't trim this off, you get an off-by-2 error when chunking
                tagless = tagless.Substring(2, tagless.Length);
            }

            return tagless;
        }
    }
}