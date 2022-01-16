using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Text;
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

        public static string AddSpacesToSentence(this string text, bool preserveAcronyms)
        {
            if (string.IsNullOrWhiteSpace(text))
                return string.Empty;
            StringBuilder newText = new StringBuilder(text.Length * 2);
            newText.Append(text[0]);
            for (int i = 1; i < text.Length; i++)
            {
                if (char.IsUpper(text[i]))
                    if ((text[i - 1] != ' ' && !char.IsUpper(text[i - 1])) ||
                        (preserveAcronyms && char.IsUpper(text[i - 1]) &&
                         i < text.Length - 1 && !char.IsUpper(text[i + 1])))
                        newText.Append(' ');
                newText.Append(text[i]);
            }
            return newText.ToString();
        }

    }
}
