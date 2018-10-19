namespace HolmesMVC.Models.ViewModels
{

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Web.Hosting;
    using System.Xml.Linq;

    public class CanonSearchNode
    {
        public string Snippet;

        public Story Story;
    }

    public class CanonSearchView
    {
        #region Fields

        private readonly int excerptBufferSize = 40;
        private readonly string highlightColor = "yellow";
        private List<char> _mainTerminatorCharacters;
        private List<char> _additionalTerminatorCharacters;

        private List<char> MainTerminatorCharacters()
        {
            if (_mainTerminatorCharacters == null)
            {
                List<char> terminatorCharacters = new List<char>
                {
                    '<',
                    '>',
                    '!',
                    '.',
                    '?',
                    '"',
                    '\'',
                    '—'
                };
                _mainTerminatorCharacters = terminatorCharacters;
            }
            return _mainTerminatorCharacters;
        }

        private List<char> AdditionalTerminatorCharacters()
        {
            if (_additionalTerminatorCharacters == null)
            {
                List<char> terminatorCharacters = new List<char>
                {
                    ',',
                    ':',
                    ';',
                    '-'
                };
                _additionalTerminatorCharacters = terminatorCharacters;
            }
            return _additionalTerminatorCharacters;
        }

        #endregion

        #region Private Functions

        private List<CanonSearchNode> GetRelevantNodes(HolmesDBEntities Db, XDocument xmlDoc, string query)
        {
            List<CanonSearchNode> nodes = new List<CanonSearchNode>();
            if (string.IsNullOrEmpty(query) || xmlDoc == null)
            {
                return nodes;
            }

            Dictionary<string, Story> nodeNames = GetNodeNames(Db);
            query = query.ToLower();
            nodes = (from item in xmlDoc.Descendants("p")
                     where item.Value.ToLower().Contains(query)
                     select new CanonSearchNode
                     {
                         Story = nodeNames[item.Ancestors("story").Descendants("title").First().Value.ToLower().Trim()],
                         Snippet = item.Value
                     }
                                ).ToList();

            return nodes;
        }

        private static Dictionary<string, Story> GetNodeNames(HolmesDBEntities Db)
        {
            Dictionary<string, Story> nodeNames = new Dictionary<string, Story>();

            foreach (var s in Db.Stories)
            {
                nodeNames.Add(s.Name.ToLower().Trim(), s);
            }

            return nodeNames;
        }


        /// <summary>
        /// Wraps matched strings in HTML span elements styled with a background-color
        /// Credit to Mike Brind at mikesdotnetting.com
        /// </summary>
        /// <param name="text"></param>
        /// <param name="keywords">Comma-separated list of strings to be highlighted</param>
        /// <param name="cssColor">The Css color to apply</param>
        /// <returns>string</returns>
        private string HighlightKeyWords(string text, string keywords)
        {
            if (string.IsNullOrEmpty(text) || string.IsNullOrEmpty(keywords) || string.IsNullOrEmpty(highlightColor))
                return text;
            var words = keywords.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            return words.Select(word => word.Trim()).Aggregate(text,
                         (current, pattern) =>
                         Regex.Replace(current,
                                         pattern,
                                           string.Format("<span style=\"background-color:{0}\">{1}</span>",
                                           highlightColor,
                                           "$0"),
                                           RegexOptions.IgnoreCase));

        }

        private string TrimEndToWord(string text, bool blockEllipsis)
        {
            var rT = text;
            rT = new string(rT.Reverse().ToArray());
            rT = TrimStartToWord(rT, true, false);
            text = new string(rT.Reverse().ToArray());

            if (!blockEllipsis)
            {
                List<char> terminatorCharacters = MainTerminatorCharacters();
                if (!terminatorCharacters.Contains(text[text.Length - 1]))
                {
                    text = text + "...";
                }
                terminatorCharacters = AdditionalTerminatorCharacters();
                if (terminatorCharacters.Contains(text[text.Length - 4]))
                {
                    text = text.Substring(0, text.Length - 4) + "...";
                }
            }
            return text;
        }

        private string TrimStartToWord(string text, bool reversed, bool forceEllipsis)
        {
            List<char> terminatorCharacters = new List<char>();
            terminatorCharacters.AddRange(MainTerminatorCharacters());

            var ellipsis = forceEllipsis;

            if (reversed)
            {
                terminatorCharacters.AddRange(AdditionalTerminatorCharacters());
            }

            foreach (char c in terminatorCharacters)
            {
                if (text.IndexOf(c) == 0)
                {
                    return text;
                }
            }

            var startsWithSpace = text.IndexOf(" ") == 0;

            if (!startsWithSpace)
            {
                if (text.IndexOf(' ') > 0)
                {
                    var prevChar = text[text.IndexOf(' ') - 1];
                    if (!terminatorCharacters.Contains(prevChar)) 
                    {
                        ellipsis = true;
                    }
                }
                text = text.Substring(text.IndexOf(' ') + 1, text.Length - (text.IndexOf(' ') + 1));
            }
            else
            {
                text = text.Substring(1, text.Length - 1);
                ellipsis = true;
            }

            if (ellipsis)
            {
                text = "..." + text;
            }

            return text;
        }

        private string TrimBothEndsToWord(string text)
        {
            text = TrimStartToWord(text, false, false);
            text = TrimEndToWord(text, false);
            return text;
        }

        private string ExcerptText(string text)
        {
            string startPattern = "<span style=\"background-color:" + highlightColor + "\">";
            string endPattern = "</span>";

            if (text.Split('/').Count() > 2)
            {
                // More than one keyword in the excerpt
                // Cut out extraneous text between the keywords

                string excerptingPattern = endPattern + "(.{" + excerptBufferSize + "})[^<>]{" + excerptBufferSize * 2 + ",9999}?(.{" + excerptBufferSize + "})" + startPattern;
                text = Regex.Replace(text, excerptingPattern, endPattern + "$1" + "|" + "$2" + startPattern);

                // Trim the raw edges of the cuts to nearest word-ending

                string[] textArray = text.Split('|');

                textArray[0] = TrimEndToWord(textArray[0], true);
                for (var i = 0; i < textArray.Length - 1; i++)
                {
                    textArray[i] = TrimBothEndsToWord(textArray[i]);
                }
                textArray[textArray.Length - 1] = TrimStartToWord(textArray[textArray.Length - 1], false, true);

                text = string.Join("<br /><br />", textArray);
            }

            // Trim ends to within proximity of the keywords

            int startOfKeyword = text.IndexOf(startPattern);
            if (startOfKeyword > excerptBufferSize)
            {
                text = text.Substring(startOfKeyword - excerptBufferSize, text.Length - (startOfKeyword - excerptBufferSize));
            }

            string reverseText = text;
            reverseText = new string(reverseText.Reverse().ToArray());
            endPattern = new string(endPattern.Reverse().ToArray());
            int endOfKeyword = text.Length - (reverseText.IndexOf(endPattern));
            if (text.Length > endOfKeyword + excerptBufferSize)
            {
                text = text.Substring(0, endOfKeyword + excerptBufferSize);
            }

            // Trim ends to nearest word-ending

            text = TrimBothEndsToWord(text);

            return text;
        }

#endregion

        #region Properties

        public string Query;

        public List<CanonSearchNode> Nodes;

        #endregion

        #region Constructor

        public CanonSearchView(HolmesDBEntities Db, string query)
        {
            Nodes = new List<CanonSearchNode>();

            if (query != null)
            {
                var storiesLocation = "~/Services/Stories/";
                var storiesExtension = ".xml";

                var xmlDoc = new XDocument();

                foreach (var s in Db.Stories)
                {
                    var storyUrl = HostingEnvironment.MapPath(storiesLocation + s.ID.ToUpper() + storiesExtension);

                    if (storyUrl != null)
                    {
                        xmlDoc = XDocument.Load(storyUrl);

                        string reverseQuery = query;
                        reverseQuery = new string(reverseQuery.Reverse().ToArray());
                        if (query.IndexOf('"') == 0 && reverseQuery.IndexOf('"') == 0)
                        {
                            query = query.Replace("\"", string.Empty);
                            Query = "\"" + query + "\"";
                            Nodes.AddRange(GetRelevantNodes(Db, xmlDoc, query));
                        }
                        else
                        {
                            var splitQuery = query.Split(' ');
                            Query = string.Join(", ", splitQuery);
                            foreach (var keyword in splitQuery)
                            {
                                Nodes.AddRange(GetRelevantNodes(Db, xmlDoc, keyword));
                            }
                        }
                    }
                }

                var unsortedNodes = Nodes;
                var sortedNodes = new List<CanonSearchNode>();

                var sortedStories = (from e in Db.Episodes
                                     where e.Season.Adaptation.Name == "Canon"
                                     select e.StoryID).ToList();

                foreach (var s in sortedStories)
                {
                    sortedNodes.AddRange(unsortedNodes.Where(n => n.Story.ID == s));
                }

                Nodes = sortedNodes;

                foreach (var node in Nodes)
                {
                    string highlightedSnippet = node.Snippet;

                    if (Query.IndexOf('"') > -1)
                    {
                        highlightedSnippet = HighlightKeyWords(node.Snippet, Query.Replace("\"", string.Empty));
                    }
                    else
                    {
                        highlightedSnippet = HighlightKeyWords(node.Snippet, Query.Replace(' ', ','));
                    }

                    string excerptedSnippet = ExcerptText(highlightedSnippet);

                    node.Snippet = excerptedSnippet;
                }
            }
        }

        #endregion
    }

}