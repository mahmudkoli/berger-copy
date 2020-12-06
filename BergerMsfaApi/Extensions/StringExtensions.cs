using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Text.RegularExpressions;

namespace BergerMsfaApi.Extensions
{
    public static class StringExtensions
    {
        public static string ToUrlFriendly(this string value)
        {
            value = value.ToLowerInvariant().Replace(" ", "-");
            value = Regex.Replace(value, @"[^0-9a-z-]", string.Empty);

            return value;
        }

        public static string MakeId(this string maxValue, string prefix, int returnLength, char fillValue = '0')
        {
            const string uniq = "0";
            maxValue = string.IsNullOrEmpty(maxValue) ? prefix + uniq.PadLeft(returnLength, fillValue) : maxValue;
            var intPart = maxValue.Contains(prefix) ? maxValue.Substring(prefix.Length, maxValue.Length - prefix.Length) : maxValue;
            var id = (BigInteger.Parse(intPart, NumberStyles.Float, CultureInfo.InvariantCulture) + 1).ToString();
            if (returnLength > 1)
            {
                id = prefix + id.PadLeft(returnLength, fillValue);
            }
            return id;
        }

        public static string ObjectToString(this object value, string defaultValue = "")
        {
            if (value == null) return defaultValue;
            return value.ToString().Trim();
        }
        public static string RowToString(this object obj)
        {
            return string.Join(" ",
                obj.GetType().GetProperties().ToDictionary(x => x.Name, x => x.GetValue(obj)?.ToString() ?? "").Values);
        }
    }
}
