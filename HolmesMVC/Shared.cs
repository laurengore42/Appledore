namespace HolmesMVC
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Web;
    using System.Xml.Serialization;

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

        public static string CheckRename(Appearance app)
        {
            var rename = (from r in app.Episode1.Season1.Adaptation1.Renames
                          where
                              r.Actor == app.Actor
                              && r.Character == app.Character
                          select r).FirstOrDefault();
            return (rename == null)
                ? null
                : LongName(new Character
                {
                    Honorific = rename.Honorific,
                    Honorific1 =
                        rename.Honorific1,
                    Forename = rename.Forename,
                    Surname = rename.Surname
                });
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
                    where a.Episode1.Season1.Adaptation1.Name == "Canon"
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
            if (string.IsNullOrWhiteSpace(forename) && character.Honorific != null)
            {
                forename = character.Honorific1.Name;
            }

            var surname = character.Surname;
            return BuildName(new[] { forename, surname }, ' ');
        }

        // returns 'Professor James Moriarty'
        public static string LongName(Character character)
        {
            var honorific = character.Honorific1 != null ? character.Honorific1.Name : string.Empty;
            var forename = character.Forename ?? string.Empty;
            var surname = character.Surname ?? string.Empty;

            return BuildName(new[] { honorific, forename, surname }, ' ');
        }

        // returns 'Moriarty, Professor James'
        public static string DisplayName(Character c)
        {
            return !c.Forename.IsNullOrWhiteSpace()
                       ? c.Honorific != null
                             ? c.Surname + ", " + c.Honorific1.Name + " "
                               + c.Forename
                             : c.Surname + ", " + c.Forename
                       : c.Honorific != null
                             ? c.Surname + ", " + c.Honorific1.Name
                             : c.Surname;
        }

        public static string DisplayName(Episode e)
        {
            return e.Title ?? (e.Story == null ? "Error in Episode DisplayName!" : e.Story1.Name);
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

            var medium = a.Medium1.Name;
            if (medium == "Television")
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
                (from e in thisepisode.Season1.Episodes
                 where e.Airdate < thisepisode.Airdate
                 select e).Count() + 1;

            if (thisepisode.Season1.Adaptation1.Seasons.Count() == 1)
            {
                return episodeNumber.ToString(CultureInfo.InvariantCulture);
            }

            return (episodeNumber < 10)
                       ? thisepisode.Season1.AirOrder + "x0" + episodeNumber
                       : thisepisode.Season1.AirOrder + "x" + episodeNumber;
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

        public static int GetHolmes(List<Character> allCharacters)
        {
            var charId = (from c in allCharacters
                          where
                              c.Surname == "Holmes" && c.Forename == "Sherlock"
                          select c.ID).FirstOrDefault();

            if (charId <= 0)
            {
                return -1;
            }

            return charId;
        }

        public static int GetWatson(List<Character> allCharacters)
        {
            var charId = (from c in allCharacters
                          where
                              c.Surname == "Watson" && c.Forename == "John"
                          select c.ID).FirstOrDefault();

            if (charId <= 0)
            {
                return -1;
            }

            return charId;
        }

        public static List<Actor> PlayedBy(int charId, Adaptation adapt)
        {
            return (from a in adapt.Seasons.SelectMany(s => s.Episodes).SelectMany(e => e.Appearances)
                    where a.Character == charId
                    group a by a.Actor1 into grp
                    orderby grp.Count() descending
                    select grp.Key).ToList();
        }

        public static List<Actor> PlayedBy(string name, Adaptation adapt)
        {
            var allCharacters = adapt.Seasons.SelectMany(s => s.Episodes).SelectMany(e => e.Appearances).Select(a => a.Character1).ToList();

            if (!allCharacters.Any())
            {
                return new List<Actor>();
            }

            int charId;
            switch (name)
            {
                case "Holmes":
                    charId = GetHolmes(allCharacters);
                    if (charId < 0)
                    {
                        return new List<Actor>();
                    }

                    break;
                case "Watson":
                    charId = GetWatson(allCharacters);
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
    }
}