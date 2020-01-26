using System.Globalization;
using System.Text.RegularExpressions;

namespace c_sharp_playground.Extensions
{
    public static class StringExtensions
    {
        public static readonly Regex UnderscoreRegex = new Regex("_", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Compiled);

        public static string WithoutUnderscores(this string text)
        {
            return UnderscoreRegex.Replace(text, " ");
        }

        public static string ToTitleCase(this string text, bool force = false)
        {
            if (force) text = text.ToLowerInvariant();
            return CultureInfo.InvariantCulture.TextInfo.ToTitleCase(text);
        }
    }
}
