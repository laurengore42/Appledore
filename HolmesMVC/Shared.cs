namespace HolmesMVC
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Web;

    using HolmesMVC.Enums;

    public static class Shared
    {

        public static void SomethingChanged(HttpApplicationStateBase app)
        {
            app["LastDbUpdate"] = DateTime.UtcNow.ToString(CultureInfo.InvariantCulture);
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