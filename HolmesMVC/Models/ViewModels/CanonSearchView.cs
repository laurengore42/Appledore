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

        private int excerptBufferSize = 40;
        private string highlightColor = "yellow";

        #endregion

        #region Private Functions

        private List<CanonSearchNode> GetRelevantNodes(HolmesDBEntities Db, XDocument xmlDoc, string query)
        {
            List<CanonSearchNode> nodes = new List<CanonSearchNode>();
            if (String.IsNullOrEmpty(query) || xmlDoc == null)
            {
                return nodes;
            }

            Dictionary<String, Story> nodeNames = GetNodeNames(Db);
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

        private Dictionary<String, Story> GetNodeNames(HolmesDBEntities Db)
        {
            Dictionary<String, Story> nodeNames = new Dictionary<String, Story>();

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
            if (text == String.Empty || keywords == String.Empty || highlightColor == String.Empty)
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

        private List<char> MainTerminatorCharacters()
        {
            List<char> terminatorCharacters = new List<char>();
            terminatorCharacters.Add('<');
            terminatorCharacters.Add('>');
            terminatorCharacters.Add('!');
            terminatorCharacters.Add('.');
            terminatorCharacters.Add('?');
            terminatorCharacters.Add('"');
            terminatorCharacters.Add('\'');
            terminatorCharacters.Add('—');
            return terminatorCharacters;
        }

        private List<char> AdditionalTerminatorCharacters()
        {
            List<char> terminatorCharacters = new List<char>();
            terminatorCharacters.Add(',');
            terminatorCharacters.Add(':');
            terminatorCharacters.Add(';');
            terminatorCharacters.Add('-');
            return terminatorCharacters;
        }

        private string TrimEndToWord(string text)
        {
            var rT = new string(text.Reverse().ToArray());
            rT = TrimStartToWord(rT, true, false);
            text = new string(rT.Reverse().ToArray());
            return text;
        }

        private string TrimStartToWord(string text, bool reversed, bool ellipsis)
        {
            List<char> terminatorCharacters = MainTerminatorCharacters();
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

            if (text.IndexOf(" ") != 0)
            {
                text = text.Substring(text.IndexOf(' ') + 1, text.Length - (text.IndexOf(' ') + 1));
            }
            else if (text.IndexOf(" ") == 0)
            {
                text = text.Substring(1, text.Length - 1);
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
            if (text[0].ToString() != text[0].ToString().ToUpper())
            {
                text = "..." + text;
            }
            text = TrimEndToWord(text);
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

                string excerptingPattern = endPattern + "(.{" + excerptBufferSize + "}).{" + excerptBufferSize * 2 + ",9999}?(.{" + excerptBufferSize + "})" + startPattern;
                text = Regex.Replace(text, excerptingPattern, endPattern + "$1" + "|" + "$2" + startPattern);

                // Trim the raw edges of the cuts to nearest word-ending

                String[] textArray = text.Split('|');

                textArray[0] = TrimEndToWord(textArray[0]);
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

            string reverseText = new string(text.Reverse().ToArray());
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
                var xmlDoc = new XDocument();

                var storyUrl = HostingEnvironment.MapPath("~/Services/Stories/ALL.xml");

                if (storyUrl != null)
                {
                    xmlDoc = XDocument.Load(storyUrl);

                    var reverseQuery = new string(query.Reverse().ToArray());
                    if (query.IndexOf('"') == 0 && reverseQuery.IndexOf('"') == 0)
                    {
                        query = query.Replace("\"", String.Empty);
                        Query = "\"" + query + "\"";
                        Nodes.AddRange(GetRelevantNodes(Db, xmlDoc, query));
                    }
                    else
                    {
                        var splitQuery = query.Split(' ');
                        Query = String.Join(", ", splitQuery);
                        foreach (var keyword in splitQuery)
                        {
                            Nodes.AddRange(GetRelevantNodes(Db, xmlDoc, keyword));
                        }
                    }
                }

                var unsortedNodes = Nodes;
                var sortedNodes = new List<CanonSearchNode>();

                var sortedStories = (from e in Db.Episodes
                                     where e.Season1.Adaptation1.Name == "Canon"
                                     select e.Story).ToList();

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
                        highlightedSnippet = HighlightKeyWords(node.Snippet, Query.Replace("\"", String.Empty));
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