namespace HolmesMVC.BaconXml
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Xml;
    using System.Xml.Serialization;

    [XmlTypeAttribute(IncludeInSchema = false)]
    public enum ItemsChoiceType
    {
        actor,
        movie
    }

    public static class BaconXmlTools
    {
        public static ProcessedLink CheckForBannedWords(ProcessedLink proclink)
        {
            var allNames = proclink.ProcessedMovies.Select(a => a.Name).ToList();
            var concatNames = string.Join(" ", allNames).ToLower();

            if (concatNames.Contains("award") || concatNames.Contains("oscars") || concatNames.Contains("night of 100 stars") || concatNames.Contains("live") || concatNames.Contains("stage") || concatNames.Contains("show business") || concatNames.Contains("anniversary") || concatNames.Contains("years of") || concatNames.Contains("greatest") || concatNames.Contains("tribute") || concatNames.Contains("stars"))
            {
                // throw it out, start again
                return null;
            }

            return proclink;
        }

        public static string BrettNumber(string targetImdbName)
        {
            return HolmesNumber(1, "Jeremy Brett (I)", targetImdbName).Replace("Holmes number", "Brett number");
        }

        public static ProcessedLink GetProcLink(
            string holmesImdbName,
            string targetImdbName)
        {
            var uri =
                new Uri(
                    "http://oracleofbacon.org/cgi-bin/xml/post?enc=utf-8"
                    + "&a=" + holmesImdbName
                    + "&b=" + targetImdbName
                    + "&u=3&p=38b99ce9ec87&gm=0xef3ef7f");

            try
            {
                var xr =
                    XmlReader.Create(
                        WebRequest.Create(uri).GetResponse().GetResponseStream());

                if (xr.ReadOuterXml().Contains("spellcheck"))
                {
                    var scszr = new XmlSerializer(typeof(spellcheck));
                    var scheck = (spellcheck)scszr.Deserialize(xr);
                    if (scheck.match.Count() == 1)
                    {
                        return GetProcLink(holmesImdbName, scheck.match.First());
                    }
                }

                var szr = new XmlSerializer(typeof(link));
                var link = (link)szr.Deserialize(xr);
                var proclink = new ProcessedLink(link);

                proclink = CheckForBannedWords(proclink);

                return proclink;
            }
            catch (InvalidOperationException e)
            {
                if (null != e.InnerException
                    && e.InnerException.Message.Contains("error"))
                {
                    throw;
                }

                if (null != e.InnerException
                    && e.InnerException.Message.Contains("spellcheck"))
                {
                    throw;
                }

                throw new Exception(
                    "Exception while getting ProcLink for " + holmesImdbName
                    + ": " + e.Message);
            }
            catch (Exception e)
            {
                throw new Exception(
                    "Exception while getting ProcLink for " + holmesImdbName
                    + ": " + e.Message);
            }
        }

        public static ProcessedLink GetFilmOnlyProcLink(
            string holmesImdbName,
            string targetImdbName)
        {
            var uri =
                new Uri(
                    "http://oracleofbacon.org/cgi-bin/xml/post?enc=utf-8"
                    + "&a=" + holmesImdbName
                    + "&b=" + targetImdbName
                    + "&u=1&p=38b99ce9ec87&gm=0xef3ef7f");

            try
            {
                var xr = XmlReader.Create(
                        WebRequest.Create(uri).GetResponse().GetResponseStream()
                    );

                var szr = new XmlSerializer(typeof(link));
                var link = (link)szr.Deserialize(xr);
                var proclink = new ProcessedLink(link);

                return CheckForBannedWords(proclink);
            }
            catch (InvalidOperationException e)
            {
                if (null != e.InnerException
                    && e.InnerException.Message.Contains("error"))
                {
                    throw;
                }

                if (null != e.InnerException
                    && e.InnerException.Message.Contains("spellcheck"))
                {
                    throw;
                }

                throw new Exception(
                    "Exception while getting ProcLink for " + holmesImdbName
                    + ": " + e.Message);
            }
            catch (Exception e)
            {
                throw new Exception(
                    "Exception while getting ProcLink for " + holmesImdbName
                    + ": " + e.Message);
            }
        }

        public static int IntegerHolmesNumber(string holmesImdbName, string targetImdbName)
        {
            try
            {
                var proclink = GetProcLink(holmesImdbName, targetImdbName) ?? GetFilmOnlyProcLink(holmesImdbName, targetImdbName);

                if (proclink == null)
                {
                    return -10; // possible banned word found, throw out this Holmes
                }

                return proclink.ProcessedMovies.Count();
            }
            catch (InvalidOperationException e)
            {
                if (e.InnerException != null)
                {
                    if (e.InnerException.Message.Contains("error"))
                    {
                        return -42; // the actors are unlinkable!
                    }

                    if (e.InnerException.Message.Contains("spellcheck"))
                    {
                        return -2;
                    }
                }

                throw;
            }
        }

        public static string HolmesNumber(int thisHolmesId, string holmesImdbName, string targetImdbName)
        {
            var stringOut = string.Empty;

            try
            {
                var proclink = GetProcLink(holmesImdbName, targetImdbName) ?? GetFilmOnlyProcLink(holmesImdbName, targetImdbName);

                foreach (var processedMovie in proclink.ProcessedMovies)
                {
                    stringOut += processedMovie.Actor1 + "<br>\\<br>--- "
                        + processedMovie.Name + "<br>/<br>";
                }

                if (!proclink.ProcessedMovies.Any())
                {
                    return "Located a 0 result which was not caught in the initial 'Holmes' check. Suspect wrong entry in the ImdbName field.";
                }

                stringOut += "<a href='/Actor/" + thisHolmesId + "'>" + proclink.ProcessedMovies.Last().Actor2
                                 + "</a>, who has played Sherlock Holmes.";
                    stringOut += "<br><br>" + proclink.ProcessedMovies.First().Actor1 + "'s Holmes number is "
                                 + proclink.ProcessedMovies.Count() + ".";

                    return stringOut;
            }
            catch (InvalidOperationException e)
            {
                if (null != e.InnerException
                    && e.InnerException.Message.Contains("error"))
                {
                    throw;
                }

                if (null != e.InnerException
                    && e.InnerException.Message.Contains("spellcheck"))
                {
                    if (!holmesImdbName.Contains("("))
                    {
                        return HolmesNumber(thisHolmesId, holmesImdbName + " (I)", targetImdbName);
                    }

                    return "Error in Shared.HolmesNumber: possible ambiguous HolmesName? Was testing '" + holmesImdbName + "'.";
                }

                return "Unknown error when deserialising XML: " + e.Message + "¦¦¦" + e.StackTrace;
            }
        }
    }
    
    [XmlTypeAttribute(AnonymousType = true)]
    [XmlRootAttribute(Namespace = "", IsNullable = false)]
    public class link
    {
        [XmlElementAttribute("actor", typeof(string))]
        [XmlElementAttribute("movie", typeof(string))]
        [XmlChoiceIdentifierAttribute("ItemsElementName")]
        public string[] Items { get; set; }

        [XmlElementAttribute("ItemsElementName")]
        [XmlIgnoreAttribute]
        public ItemsChoiceType[] ItemsElementName { get; set; }
    }
    
    [XmlTypeAttribute(AnonymousType = true)]
    [XmlRootAttribute(Namespace = "", IsNullable = false)]
    public class spellcheck
    {
        [XmlElementAttribute("match")]
        public string[] match { get; set; }

        [XmlAttributeAttribute]
        public string name { get; set; }
    }

    public class ProcessedLink
    {
        public ProcessedLink(link link)
        {
            ProcessedMovies = new List<ProcessedMovie>();
            for (int i = 1; i < link.Items.Length; i += 2)
            {
                if (string.IsNullOrWhiteSpace(link.Items[i - 1])
                    || string.IsNullOrWhiteSpace(link.Items[i])
                    || string.IsNullOrWhiteSpace(link.Items[i + 1]))
                {
                    continue;
                }

                var procM = new ProcessedMovie
                                    {
                                        Name = link.Items[i],
                                        Actor1 = link.Items[i - 1],
                                        Actor2 = link.Items[i + 1]
                                    };
                    ProcessedMovies.Add(procM);
            }
        }

        public List<ProcessedMovie> ProcessedMovies { get; set; }
    }

    public class ProcessedMovie
    {
        public string Name { get; set; }

        public string Actor1 { get; set; }
        
        public string Actor2 { get; set; }
    }
}