using System;
using System.Linq;
using System.Net;
using System.Xml;
using System.Xml.Serialization;

namespace HolmesMVC.Services.BaconNumber
{
    public class BaconNumber
    {
        public int Number {get; private set; }

        public string Message { get; private set; }

        public BaconNumber(int thisHolmesId, string holmesImdbName, string targetImdbName)
        {
            try
            {
                var proclink = GetProcLink(holmesImdbName, targetImdbName) ?? GetFilmOnlyProcLink(holmesImdbName, targetImdbName);

                if (proclink == null)
                {
                    Number = -10; // possible banned word found, throw out this Holmes
                }
                else
                {
                    Number = proclink.ProcessedMovies.Count();
                    Message = LowestHolmesNumberString(thisHolmesId, holmesImdbName, targetImdbName, proclink);
                }
            }
            catch (InvalidOperationException e)
            {
                if (e.InnerException != null)
                {
                    if (e.InnerException.Message.Contains("error"))
                    {
                        Number = -42; // the actors are unlinkable!
                    }

                    else if (e.InnerException.Message.Contains("spellcheck"))
                    {
                        Number = -2;
                    }

                    else
                    {
                        throw;
                    }
                }
                else
                {
                    throw;
                }
            }
        }

        public static string BrettNumber(string targetImdbName)
        {
            return (new BaconNumber(1, Shared.JeremyBrettImdb(), targetImdbName)).Message.Replace("Holmes number", "Brett number");
        }

        private static string LowestHolmesNumberString(int thisHolmesId, string holmesImdbName, string targetImdbName, ProcessedLink proclink)
        {
            var stringOut = string.Empty;

            try
            {
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
                    return "Error in Shared.HolmesNumber: possible ambiguous HolmesName? Was testing '" + holmesImdbName + "'.";
                }

                return "Unknown error when deserialising XML: " + e.Message + "¦¦¦" + e.StackTrace;
            }
        }

        private static ProcessedLink GetProcLink(
            string holmesImdbName,
            string targetImdbName)
        {
            var uri =
                new Uri(
                    "https://oracleofbacon.org/cgi-bin/xml/post?enc=utf-8"
                    + "&a=" + holmesImdbName
                    + "&b=" + targetImdbName
                    + "&u=3&p=38b99ce9ec87&gm=0xef3ef7f");

            try
            {
                var xr =
                    XmlReader.Create(
                        WebRequest.Create(uri).GetResponse().GetResponseStream());

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

        private static ProcessedLink GetFilmOnlyProcLink(
            string holmesImdbName,
            string targetImdbName)
        {
            var uri =
                new Uri(
                    "https://oracleofbacon.org/cgi-bin/xml/post?enc=utf-8"
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

        private static ProcessedLink CheckForBannedWords(ProcessedLink proclink)
        {
            var allNames = proclink.ProcessedMovies.Select(a => a.Name).ToList();
            var concatNames = string.Join(" ", allNames).ToLower();

            if (concatNames.Contains("award") || concatNames.Contains("oscars") || concatNames.Contains("emmys") || concatNames.Contains("night of 100 stars") || concatNames.Contains("live") || concatNames.Contains("stage") || concatNames.Contains("show business") || concatNames.Contains("anniversary") || concatNames.Contains("years of") || concatNames.Contains("greatest") || concatNames.Contains("tribute") || concatNames.Contains("stars") || concatNames.Contains("relief") || concatNames.Contains("red nose day") || concatNames.Contains("royal gala") || concatNames.Contains("salute to") || concatNames.Contains("stand up to"))
            {
                // throw it out, start again
                return null;
            }

            return proclink;
        }
    }
}