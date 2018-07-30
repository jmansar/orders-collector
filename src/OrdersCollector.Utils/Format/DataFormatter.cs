using System;

namespace OrdersCollector.Utils.Format
{
    public static class DataFormatter
    {
        public const string DayDateFormat = "yyyy-MM-dd";

        /// <summary>
        /// Formats date (without hours and minutes).
        /// </summary>
        /// <param name="date">Date</param>
        public static string FormatDayDate(DateTime? date)
        {
            if (date == null)
            {
                return String.Empty;
            }

            return date.Value.ToString(DayDateFormat);
        }
        
        /// <summary>
        /// Capitalizes first letter.
        /// </summary>
        public static string CapitalizeFirst(string text)
        {
            if (String.IsNullOrWhiteSpace(text))
            {
                return text;
            }

            return Char.ToUpper(text[0]) + text.Substring(1);
        }
    }
}
