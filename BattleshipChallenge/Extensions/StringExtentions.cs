using System;

namespace BattleshipChallenge.Extensions
{
    internal static class StringExtentions
    {
        internal static bool EqualsIgnoreCase(this string input, string value)
        {
            return input?.Equals(value, StringComparison.CurrentCultureIgnoreCase) ?? false;
        }

        internal static string Indent(this string input, int indentLevel)
        {
            return $"{new string(' ', indentLevel * 4)}{input}";
        }
    }
}
