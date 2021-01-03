using System;

namespace Berger.Odata.Extensions
{
    public static class DateTimeExtension
    {
        public static string DateFormat(this DateTime date) => date.ToString("yyyy-MM-dd'T'00:00:00");
        public static string DateFormat(this string date) => Convert.ToDateTime(date).DateFormat();

        public static string GetMonthName(this DateTime date, int number) => date.AddMonths(number).ToString("MMMM");

        public static DateTime GetMonthDate(this DateTime date, int number) => Convert.ToDateTime(date.AddMonths(number));

        public static DateTime GetLYFD(this DateTime date) => Convert.ToDateTime(new DateTime(date.AddYears(-1).Year, date.Month, 1));
        public static DateTime GetLYLCD(this DateTime date) => Convert.ToDateTime(new DateTime(date.AddYears(-1).Year, date.Month, date.Day));
        public static DateTime GetLYLD(this DateTime date) => Convert.ToDateTime(new DateTime(date.AddYears(-1).Year, date.Month, DateTime.DaysInMonth(date.Year, date.Month)));



        public static DateTime GetCYFD(this DateTime date) => Convert.ToDateTime(new DateTime(date.Year, date.Month, 1));
        public static DateTime GetCYLCD(this DateTime date) => Convert.ToDateTime(new DateTime(date.Year, date.Month, date.Day));
        public static DateTime GetCYLD(this DateTime date) => Convert.ToDateTime(new DateTime(date.Year, date.Month, DateTime.DaysInMonth(date.Year, date.Month)));


    }
}
