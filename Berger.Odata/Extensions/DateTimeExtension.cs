using System;

namespace Berger.Odata.Extensions
{
    public static class DateTimeExtension
    {
        public static string DateFormat(this DateTime date) => date.ToString("yyyy.MM.dd");
        public static string DateFormat(this DateTime? date) => date.HasValue ? date.Value.ToString("yyyy.MM.dd") : "";
        public static string DateFormat(this string date) => Convert.ToDateTime(date).DateFormat();

        public static string GetMonthName(this DateTime date, int number) => date.AddMonths(number).ToString("MMMM");
        public static DateTime GetMonthDate(this DateTime date, int number) => Convert.ToDateTime(date.AddMonths(number));

        public static DateTime GetLYFD(this DateTime date) => Convert.ToDateTime(new DateTime(date.AddYears(-1).Year, date.Month, 1));
        public static DateTime GetLYLCD(this DateTime date) => Convert.ToDateTime(new DateTime(date.AddYears(-1).Year, date.Month, date.Day));
        public static DateTime GetLYLD(this DateTime date) => Convert.ToDateTime(new DateTime(date.AddYears(-1).Year, date.Month, DateTime.DaysInMonth(date.Year, date.Month)));

        public static DateTime GetCYFD(this DateTime date) => Convert.ToDateTime(new DateTime(date.Year, date.Month, 1));
        public static DateTime GetCYLCD(this DateTime date) => Convert.ToDateTime(new DateTime(date.Year, date.Month, date.Day));
        public static DateTime GetCYLD(this DateTime date) => Convert.ToDateTime(new DateTime(date.Year, date.Month, DateTime.DaysInMonth(date.Year, date.Month)));

        public static DateTime GetLFYFD(this DateTime date, int startMonth) => Convert.ToDateTime(new DateTime(date.Month >= startMonth ? date.AddYears(-1).Year : date.AddYears(-2).Year, startMonth, 1));
        public static DateTime GetLFYLCD(this DateTime date, int startMonth) => Convert.ToDateTime(new DateTime(date.AddYears(-1).Year, date.Month, date.Day));
        public static DateTime GetLFYLD(this DateTime date, int startMonth) => Convert.ToDateTime(new DateTime(date.Month >= startMonth ? date.Year : date.AddYears(-1).Year, startMonth, 1).AddDays(-1));

        public static DateTime GetCFYFD(this DateTime date, int startMonth) => Convert.ToDateTime(new DateTime(date.Month >= startMonth ? date.Year : date.AddYears(-1).Year, startMonth, 1));
        public static DateTime GetCFYLCD(this DateTime date, int startMonth) => Convert.ToDateTime(new DateTime(date.Year, date.Month, date.Day));
        public static DateTime GetCFYLD(this DateTime date, int startMonth) => Convert.ToDateTime(new DateTime(date.Month >= startMonth ? date.AddYears(1).Year : date.Year, startMonth, 1).AddDays(-1));
        
        public static (DateTime, DateTime) GetCurrentFinacialYearDateRange(this DateTime date, int startMonth)
        {
            if (date.Month >= startMonth)
            {
                DateTime startDate = new DateTime(date.Year, startMonth, 1);
                DateTime endDate = new DateTime(date.Year+1, startMonth, 1).AddDays(-1);
                return (startDate, endDate);
            }
            else
            {
                DateTime startDate = new DateTime(date.Year-1, startMonth, 1);
                DateTime endDate = new DateTime(date.Year, startMonth, 1).AddDays(-1);
                return (startDate, endDate);
            }
        }
    }
}
