namespace Axwabo.CommandSystem.Extensions;

/// <summary>Extension methods for strings.</summary>
public static class StringExtensions
{

    /// <summary>
    /// Pluralizes the given phrase with the given count.
    /// </summary>
    /// <param name="phrase">The phrase to pluralize.</param>
    /// <param name="count">An amount of things.</param>
    /// <returns>A string containing the count and the pluralized phrase.</returns>
    public static string PluralizeWithCount(this string phrase, int count) => $"{count} {phrase.Pluralize(count)}";

    /// <summary>
    /// Makes the given phrase plural if the count is not 1.
    /// </summary>
    /// <param name="phrase">The phrase to pluralize.</param>
    /// <param name="count">An amount of things.</param>
    /// <returns>The pluralized phrase.</returns>
    public static string Pluralize(this string phrase, int count) => count == 1 ? phrase : phrase + "s";

    /// <summary>
    /// Returns a substring of the given string until the first occurrence of the given separators.
    /// </summary>
    /// <param name="s">The string to get the substring from.</param>
    /// <param name="searchStart">The index to start searching from.</param>
    /// <param name="separators">The separators to search for.</param>
    /// <returns>The substring.</returns>
    public static string Substring(this string s, int searchStart, params char[] separators)
    {
        var index = s.IndexOfAny(separators, searchStart);
        return index == -1 ? s.Substring(searchStart) : s.Substring(searchStart, index - searchStart);
    }

    /// <summary>
    /// Returns a substring of the given string until the first occurrence of the given separators.
    /// </summary>
    /// <param name="s">The string to get the substring from.</param>
    /// <param name="separators">The separators to search for.</param>
    /// <returns>The substring.</returns>
    public static string Substring(this string s, params char[] separators) => s.Substring(0, separators);

    /// <summary>
    /// Determines whether the given string contains the given value, ignoring case.
    /// </summary>
    /// <param name="s">The string to search in.</param>
    /// <param name="value">The value to search for.</param>
    /// <returns><see langword="true"/> if the string contains the value; otherwise, <see langword="false"/>.</returns>
    public static bool ContainsIgnoreCase(this string s, string value) => s.IndexOf(value, StringComparison.OrdinalIgnoreCase) >= 0;

}
