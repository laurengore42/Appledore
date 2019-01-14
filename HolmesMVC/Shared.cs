namespace HolmesMVC
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using HolmesMVC.Enums;

    public static class Shared
    {
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
                        case 1:
                        case 2:
                            return "Winter " + date.ToString("yyyy");
                        case 3:
                        case 4:
                        case 5:
                            return "Spring " + date.ToString("yyyy");
                        case 6:
                        case 7:
                        case 8:
                            return "Summer " + date.ToString("yyyy");
                        case 9:
                        case 10:
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

        private static string TrimCharactersForValidUrlName(string name) =>
            name
            .Replace(".", "")
            .Replace(":", "")
            .Replace("/", "")
			.Replace("+", "")
			.Replace("  ", " ")
            .Replace(" ", "_")
            .ToLower();

        public static string BuildUrlName(string forename, string surname)
        {
            int throwaway;
            return string.Concat(!string.IsNullOrEmpty(forename) || int.TryParse(surname, out throwaway) ? TrimCharactersForValidUrlName(forename) + "_" : string.Empty, TrimCharactersForValidUrlName(surname));
        }

        public static string BuildName(IEnumerable<string> names, string divider) =>
            string.Join(divider, names.Where(s => !string.IsNullOrEmpty(s)));

        public static string JeremyBrettImdb()
        {
            return "Jeremy Brett";
        }
    }
}