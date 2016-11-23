using System;
using System.Linq;
using System.Text;

namespace Mjr.FeatureDriven.net4.Api.Infrastructure.Extensions
{
    public static class StringExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="maxCharsExcludingSuffixLength"></param>
        /// <param name="suffix"></param>
        /// <returns></returns>
        public static string SafeSubstring(this string value, int maxCharsExcludingSuffixLength, string suffix = "")
        {
            if (maxCharsExcludingSuffixLength < 0)
                throw new ArgumentException("maxChars must be a positive number", "maxCharsExcludingSuffixLength");
            if (value == null)
            {
                return null;
            }

            return value.Length <= maxCharsExcludingSuffixLength ?
                   value : value.Substring(0, maxCharsExcludingSuffixLength) + suffix;
        }
        /// <summary>
        /// Determines whether [contains only digits] [the specified s].
        /// </summary>
        /// <param name="s">The s.</param>
        /// <returns>
        /// 	<c>true</c> if [contains only digits] [the specified s]; otherwise, <c>false</c>.
        /// </returns>
        public static bool ContainsOnlyDigits(this string s)
        {
            return s.All(char.IsDigit);
        }

        /// <summary>
        /// Keeps the digits.
        /// </summary>
        /// <param name="s">The s.</param>
        /// <returns></returns>
        public static string KeepTheDigits(this string s)
        {
            var sb = new StringBuilder();

            foreach (var c in s.Where(char.IsDigit))
                sb.Append(c);

            return sb.ToString();
        }
        /// <summary>
        /// Removes all " from a string.
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string RemoveQuotes(this string str)
        {
            if (string.IsNullOrWhiteSpace(str)) return str;
            while (str.Contains("\""))
                str = str.Replace("\"", "");
            return str;
        }
    }
}