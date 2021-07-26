using System;
using System.Globalization;

namespace Berger.Odata.Extensions
{
    public static class DateTimeExtension
    {
        public static string DateFormat(this DateTime date) => date.ToString("yyyy.MM.dd");
        public static string SalesSearchDateFormat(this DateTime date) => date.ToString("dd.MM.yyyy");
        public static string MTSSearchDateFormat(this DateTime date) => $"{string.Format("{0:0000}", date.Year)}.{string.Format("{0:00}", date.Month)}";
        public static DateTime SalesResultDateFormat(this string date, string format = "yyyyMMdd") => DateTime.ParseExact(date, format, CultureInfo.InvariantCulture);
        public static string DateTimeFormat(this DateTime date) => date.ToString("yyyy-MM-ddT00:00:00");
        public static string FinancialSearchDateTimeFormat(this DateTime date) => date.ToString("yyyy-MM-ddT00:00:00");
        public static string CollectionSearchDateTimeFormat(this DateTime date) => date.ToString("yyyy-MM-ddT00:00:00");
        public static string DeliverySearchDateTimeFormat(this DateTime date) => date.ToString("yyyy-MM-ddT00:00:00");
        public static string DateFormat(this DateTime? date) => date.HasValue ? date.Value.DateFormat() : string.Empty;
        public static string DateFormat(this string date) => Convert.ToDateTime(date).DateFormat();
        public static string DateFormat(this DateTime date, string format) => date.ToString(format);
        public static DateTime DateFormatDate(this string date, string format = "yyyyMMdd") => DateTime.ParseExact(date, format, CultureInfo.InvariantCulture);
        public static DateTime DateFormatTime(this string date, string format = "HH:mm:ss") => DateTime.ParseExact(date, format, CultureInfo.InvariantCulture);
        public static string ReturnDateFormatDate(this string date, string format = "yyyyMMdd") => DateFormatDate(date, format).ToString("dd MMM yyyy");
        public static string ReturnDateFormatTime(this string date, string format = "HH:mm:ss") => DateFormatTime(date, format).ToString("HH:mm:ss");

        public static string GetMonthName(this DateTime date, int number) => date.AddMonths(number).ToString("MMMM");
        public static DateTime GetMonthDate(this DateTime date, int number) => Convert.ToDateTime(date.AddMonths(number));

        public static DateTime GetLYFD(this DateTime date) => Convert.ToDateTime(new DateTime(date.AddYears(-1).Year, date.Month, 1));
        public static DateTime GetLYLCD(this DateTime date) => Convert.ToDateTime(new DateTime(date.AddYears(-1).Year, date.Month, DateTime.Now.AddYears(-1).Day));
        public static DateTime GetLYLD(this DateTime date) => Convert.ToDateTime(new DateTime(date.AddYears(-1).Year, date.Month, DateTime.DaysInMonth(date.AddYears(-1).Year, date.Month)));

        public static DateTime GetCYFD(this DateTime date) => Convert.ToDateTime(new DateTime(date.Year, date.Month, 1));
        public static DateTime GetCYLCD(this DateTime date) => Convert.ToDateTime(new DateTime(date.Year, date.Month, DateTime.Now.Day));
        public static DateTime GetCYLD(this DateTime date) => Convert.ToDateTime(new DateTime(date.Year, date.Month, DateTime.DaysInMonth(date.Year, date.Month)));

        public static DateTime GetLFYFD(this DateTime date, int startMonth = 4) => Convert.ToDateTime(new DateTime(date.Month >= startMonth ? date.AddYears(-1).Year : date.AddYears(-2).Year, startMonth, 1));
        public static DateTime GetLFYLCD(this DateTime date, int startMonth = 4) => Convert.ToDateTime(new DateTime(date.AddYears(-1).Year, date.Month, date.Day));
        public static DateTime GetLFYLD(this DateTime date, int startMonth = 4) => Convert.ToDateTime(new DateTime(date.Month >= startMonth ? date.Year : date.AddYears(-1).Year, startMonth, 1).AddDays(-1));

        public static DateTime GetCFYFD(this DateTime date, int startMonth = 4) => Convert.ToDateTime(new DateTime(date.Month >= startMonth ? date.Year : date.AddYears(-1).Year, startMonth, 1));
        public static DateTime GetCFYLCD(this DateTime date, int startMonth = 4) => Convert.ToDateTime(new DateTime(date.Year, date.Month, date.Day));
        public static DateTime GetCFYLD(this DateTime date, int startMonth = 4) => Convert.ToDateTime(new DateTime(date.Month >= startMonth ? date.AddYears(1).Year : date.Year, startMonth, 1).AddDays(-1));

        public static DateTime GetMonthFirstDate(this DateTime date) => new DateTime(date.Year, date.Month, 1);
        public static DateTime GetMonthLastDate(this DateTime date) => date.GetMonthFirstDate().AddMonths(1).AddDays(-1);


        public static int RemainingDays(this DateTime date) => (DateTime.DaysInMonth(date.Year, date.Month) - date.Day);
        public static int TillDays(this DateTime date) => date.Day;

        public static (DateTime, DateTime) GetCurrentFinacialYearDateRange(this DateTime date, int startMonth = 4)
        {
            if (date.Month >= startMonth)
            {
                DateTime startDate = new DateTime(date.Year, startMonth, 1);
                DateTime endDate = new DateTime(date.Year + 1, startMonth, 1).AddDays(-1);
                return (startDate, endDate);
            }
            else
            {
                DateTime startDate = new DateTime(date.Year - 1, startMonth, 1);
                DateTime endDate = new DateTime(date.Year, startMonth, 1).AddDays(-1);
                return (startDate, endDate);
            }
        }

        public static DateTime ChangeTime(this DateTime dateTime, int hours, int minutes, int seconds = default, int milliseconds = default)
        {
            return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, hours, minutes, seconds, milliseconds, dateTime.Kind);
        }
    }
}
