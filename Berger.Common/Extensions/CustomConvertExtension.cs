﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Berger.Common.Extensions
{
    public static class CustomConvertExtension
    {
        private const string _dateTimeStringFormat = "yyyy-MM-dd";

        public static void NullToEmptyString(object parameterObject)
        {
            if (parameterObject == null) return;

            Type type = parameterObject.GetType();
            foreach (var property in type.GetProperties())
            {
                if (property.PropertyType == typeof(string) && property.GetValue(parameterObject) == null)
                    property.SetValue(parameterObject, string.Empty);
            }
        }

        public static double ObjectToDouble(object value)
        {
            if (value == null) return 0.0;
            return double.TryParse(value.ToString(), out double result) ? result : 0.0;
        }

        public static decimal ObjectToDecimal(object value)
        {
            if (value == null) return decimal.Zero;
            return decimal.TryParse(value.ToString(), out decimal result) ? result : decimal.Zero;
        }

        public static string ObjectToDateString(object value)
        {
            if (value == null) return string.Empty;
            return DateTime.TryParse(value.ToString(), out DateTime result) ? result.ToString(_dateTimeStringFormat) : string.Empty;
        }

        public static string ObjectToDateString(DateTime? value)
        {
            return value.HasValue ? value.Value.ToString(_dateTimeStringFormat) : string.Empty;
        }
    }
}
