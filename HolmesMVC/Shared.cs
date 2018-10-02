namespace HolmesMVC
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Web;
    using System.Xml.Serialization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    using HolmesMVC.Enums;
    using HolmesMVC.Models;
    using HolmesMVC.Models.ViewModels;

    using Microsoft.Ajax.Utilities;

    public static class Shared
    {

        public static void SomethingChanged(HttpApplicationStateBase app)
        {
            app["LastDbUpdate"] = DateTime.UtcNow.ToString(CultureInfo.InvariantCulture);
            app["SearchDataFull"] = null;
            app["SearchDataShort"] = null;
        }

        public static Rename GetRename(Appearance app)
        {
            return (from r in app.Episode.Season.Adaptation.Renames
                    where
                        r.ActorID == app.ActorID
                        && r.CharacterID == app.CharacterID
                    select r).FirstOrDefault();
        }

        public static string SortSurname(Appearance app)
        {
            var rename = GetRename(app);
            return (rename == null)
                ? app.Character.Surname
                : rename.Surname;
        }

        public static string SortForename(Appearance app)
        {
            var rename = GetRename(app);
            return (rename == null)
                ? app.Character.Forename
                : rename.Forename;
        }

        public static string VagueDate(DateTime? nullableDate, DatePrecision precision, bool longMonth, bool longDay)
        {
            if (null == nullableDate)
            {
                return "VagueDate() was passed null date.";
            }

            var date = (DateTime)nullableDate;

            switch (precision)
            {
                case DatePrecision.Year:
                    return date.ToString("yyyy");
                case DatePrecision.Month:
                    return longMonth
                               ? date.ToString("MMMM yyyy")
                               : date.ToString("MMM yyyy");
                case DatePrecision.Season:
                    switch (date.Month)
                    {
                        case 12:
                            return "Winter " + date.ToString("yyyy");
                        case 1:
                            return "Winter " + date.ToString("yyyy");
                        case 2:
                            return "Winter " + date.ToString("yyyy");
                        case 3:
                            return "Spring " + date.ToString("yyyy");
                        case 4:
                            return "Spring " + date.ToString("yyyy");
                        case 5:
                            return "Spring " + date.ToString("yyyy");
                        case 6:
                            return "Summer " + date.ToString("yyyy");
                        case 7:
                            return "Summer " + date.ToString("yyyy");
                        case 8:
                            return "Summer " + date.ToString("yyyy");
                        case 9:
                            return "Autumn " + date.ToString("yyyy");
                        case 10:
                            return "Autumn " + date.ToString("yyyy");
                        case 11:
                            return "Autumn " + date.ToString("yyyy");
                    }

                    return "Bad data found! 'Season' precision. Date " + date.ToString("d MMMM yyyy");
                default:
                    return longMonth
                               ? longDay
                                     ? date.ToString("dd MMMM yyyy")
                                     : date.ToString("d MMMM yyyy")
                               : longDay
                                     ? date.ToString("dd MMM yyyy")
                                     : date.ToString("d MMM yyyy");
            }
        }

        public static bool IsCanon(Character c)
        {
            return (from a in c.Appearances
                    where a.Episode.Season.Adaptation.Name == "Canon"
                    select a).Any();
        }

        // returns 'Jeremy Brett'
        public static string ShortName(Actor actor)
        {
            var forename = actor.Forename;
            var surname = actor.Surname;
            return BuildName(new[] { forename, surname }, ' ');
        }

        // returns 'Edward Cedric Hardwicke'
        public static string LongName(Actor actor)
        {
            var forename = actor.Forename ?? string.Empty;
            var middlenames = actor.Middlenames ?? string.Empty;
            var surname = actor.Surname ?? string.Empty;

            return BuildName(new[] { forename, middlenames, surname }, ' ');
        }

        // returns 'Brett, Jeremy'
        public static string DisplayName(Actor a)
        {
            return a.Surname + (!a.Forename.IsNullOrWhiteSpace()
                ? ", " + a.Forename
                : string.Empty);
        }

        // returns 'Professor Moriarty' || 'James Moriarty'
        public static string ShortName(Character character)
        {
            var forename = character.Forename;
            if (string.IsNullOrWhiteSpace(forename) && character.HonorificID != null)
            {
                forename = character.Honorific.Name;
            }

            var surname = character.Surname;
            return BuildName(new[] { forename, surname }, ' ');
        }

        // returns 'Professor James Moriarty'
        public static string LongName(Character character)
        {
            var honorific = character.Honorific != null ? character.Honorific.Name : string.Empty;
            var forename = character.Forename ?? string.Empty;
            var surname = character.Surname ?? string.Empty;

            return BuildName(new[] { honorific, forename, surname }, ' ');
        }

        // returns 'Professor James Moriarty'
        public static string LongName(Rename rename)
        {
            return (rename == null)
                ? null
                : LongName(new Character
                {
                    HonorificID = rename.HonorificID,
                    Honorific =
                        rename.Honorific,
                    Forename = rename.Forename,
                    Surname = rename.Surname
                });
        }

        // returns 'Moriarty, Professor James'
        public static string DisplayName(Character c)
        {
            return !c.Forename.IsNullOrWhiteSpace()
                       ? c.HonorificID != null
                             ? c.Surname + ", " + c.Honorific.Name + " "
                               + c.Forename
                             : c.Surname + ", " + c.Forename
                       : c.HonorificID != null
                             ? c.Surname + ", " + c.Honorific.Name
                             : c.Surname;
        }

        public static string DisplayName(Episode e)
        {
            if (!e.Title.IsNullOrWhiteSpace())
            {
                return e.Title;
            }
            if (!e.StoryID.IsNullOrWhiteSpace())
            {
                return e.Story.Name;
            }
            return "Error in Episode DisplayName!";
        }

        // returns '1984 Granada TV'
        public static string DisplayName(Adaptation a)
        {
            if (!string.IsNullOrWhiteSpace(a.Name))
            {
                return a.Name;
            }

            if (a.Seasons.Count() == 1
                && a.Seasons.First().Episodes.Count() == 1)
            {
                return a.Seasons.First().Episodes.First().Title;
            }

            var medium = ((Medium)a.Medium).ToString();
            if (a.Medium == (int)Medium.Television)
            {
                medium = "TV"; // special case
            }

            var company = string.IsNullOrWhiteSpace(a.Company) ? string.Empty : a.Company;

            var startYear = (from e in a.Seasons.SelectMany(s => s.Episodes)
                             orderby e.Airdate
                             select e.Airdate.Year).FirstOrDefault();

            return startYear > 0 ? (startYear + " " + company + " " + medium).Trim() : (company + " " + medium).Trim();
        }

        public static string GetSeasonCode(Episode thisepisode)
        {
            var episodeNumber =
                (from e in thisepisode.Season.Episodes
                 where e.Airdate < thisepisode.Airdate
                 select e).Count() + 1;

            if (thisepisode.Season.Adaptation.Seasons.Count() == 1)
            {
                return episodeNumber.ToString(CultureInfo.InvariantCulture);
            }

            return (episodeNumber < 10)
                       ? thisepisode.Season.AirOrder + "x0" + episodeNumber
                       : thisepisode.Season.AirOrder + "x" + episodeNumber;
        }

        public static string Times(int count)
        {
            switch (count)
            {
                case 1:
                    return "once";
                case 2:
                    return "twice";
                default:
                    return count + " times";
            }
        }

        public static int GetHolmes()
        {
            return 1;
        }

        public static int GetWatson()
        {
            return 2;
        }

        public static List<Actor> PlayedBy(int charId, Adaptation adapt)
        {
            return (from a in adapt.Seasons.SelectMany(s => s.Episodes).SelectMany(e => e.Appearances)
                    where a.CharacterID == charId
                    group a by a.Actor into grp
                    orderby grp.Count() descending
                    select grp.Key).ToList();
        }

        public static List<Actor> PlayedBy(string name, Adaptation adapt)
        {
            var allCharacters = adapt.Seasons.SelectMany(s => s.Episodes).SelectMany(e => e.Appearances).Select(a => a.Character).ToList();

            if (!allCharacters.Any())
            {
                return new List<Actor>();
            }

            int charId;
            switch (name)
            {
                case "Holmes":
                    charId = GetHolmes();
                    if (charId < 0)
                    {
                        return new List<Actor>();
                    }

                    break;
                case "Watson":
                    charId = GetWatson();
                    if (charId < 0)
                    {
                        return new List<Actor>();
                    }

                    break;
                default:
                    throw new Exception("PlayedBy was called without a valid character name.");
            }

            return PlayedBy(charId, adapt);
        }

        public static int? AgeInYears(Actor actor)
        {
            var birth = actor.Birthdate;
            if (birth == null)
            {
                return null;
            }

            var bday = (DateTime)birth;
            var dday = actor.Deathdate ?? DateTime.Now;
            var age = dday.Year - bday.Year;
            if (dday.DayOfYear < bday.DayOfYear)
            {
                age--;
            }

            return age;
        }

        public static string BuildName(IEnumerable<string> names, char divider)
        {
            var outName = string.Empty;
            var namesList = names.ToList();

            for (var i = 0; i < namesList.Count(); i++)
            {
                if (string.IsNullOrWhiteSpace(namesList[i]))
                {
                    continue;
                }

                outName += namesList[i] + divider;
            }

            return outName.TrimEnd();
        }

        public static Dictionary<char, char> GetAccentDictionary()
        {
            return new Dictionary<char, char>() { 
                                                     { 'á', 'a' }
                                                    ,{ 'à', 'a' }
                                                    ,{ 'é', 'e' }
                                                    ,{ 'è', 'e' }
                                                    ,{ 'ö', 'o' }
                                                    ,{ 'ô', 'o' }
                                                    ,{ 'ō', 'o' }
                                                    ,{ 'ø', 'o' }
                                                };
        }

        public static string GetAccentDictionaryAsJson()
        {
            var ad = GetAccentDictionary();
            string json = JsonConvert.SerializeObject(ad, new KeyValuePairConverter());
            return json;
        }

        public static string NormaliseString(string input)
        {
            var accentDictionary = GetAccentDictionary();
            var output = input;
            foreach (char n in accentDictionary.Keys)
            {
                output = output.Replace(n, accentDictionary[n]);
            }
            return output;
        }
    }
}