using System;
using System.Collections.Generic;
using System.Linq;
using Axwabo.CommandSystem.Structs;
using Utils;

namespace Axwabo.CommandSystem;

/// <summary>
/// Common extension methods.
/// </summary>
public static class Extensions {

    /// <summary>
    /// Combines the nicknames of the given hubs into a single string.
    /// </summary>
    /// <param name="hubs">The hubs to combine.</param>
    /// <param name="separator">The separator to use.</param>
    /// <returns>The combined string.</returns>
    public static string CombineNicknames(this IEnumerable<ReferenceHub> hubs, string separator = ", ")
        => string.Join(separator, hubs.Select(p => p.nicknameSync.MyNick));

    /// <summary>
    /// Combines the nicknames of the given hubs from <see cref="CommandResultOnTarget"/> objects into a single string.
    /// </summary>
    /// <param name="results">The results to combine.</param>
    /// <param name="separator">The separator to use.</param>
    /// <returns>The combined string.</returns>
    public static string CombineNicknames(this IEnumerable<CommandResultOnTarget> results, string separator = ", ")
        => string.Join(separator, results.Select(p => p.Target.nicknameSync.MyNick));

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
    /// Returns a substring of the given string until the first occurrence of any of the given separators.
    /// </summary>
    /// <param name="s">The string to get the substring from.</param>
    /// <param name="searchStart">The index to start searching from.</param>
    /// <param name="separators">The separators to search for.</param>
    /// <returns>The substring.</returns>
    public static string Substring(this string s, int searchStart, params char[] separators) {
        var index = s.IndexOfAny(separators, searchStart);
        return index == -1 ? s.Substring(searchStart) : s.Substring(searchStart, index - searchStart);
    }

    /// <summary>
    /// Returns a substring of the given string until the first occurrence of any of the given separators.
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

    /// <summary>
    /// Attempts to parse the given string as an enum value, ignoring case.
    /// </summary>
    /// <param name="value">The string to parse.</param>
    /// <param name="result">The result.</param>
    /// <typeparam name="T">The enum type.</typeparam>
    /// <returns>Whether the string was parsed successfully.</returns>
    public static bool TryParseIgnoreCase<T>(string value, out T result) where T : struct => Enum.TryParse(value.Trim(), true, out result);

    /// <summary>
    /// Attempts to parse the given string as an integer.
    /// </summary>
    /// <param name="value">The string to parse.</param>
    /// <param name="result">The result.</param>
    /// <returns>Whether the string was parsed successfully.</returns>
    public static bool TryParseInt(string value, out int result) => int.TryParse(value.Trim(), out result);

    /// <summary>
    /// Attempts to parse the given string as a float.
    /// </summary>
    /// <param name="value">The string to parse.</param>
    /// <param name="result">The result.</param>
    /// <returns>Whether the string was parsed successfully.</returns>
    public static bool TryParseFloat(string value, out float result) => float.TryParse(value.Trim(), out result);

    /// <summary>
    /// Parses the list of arguments into a list of <see cref="ReferenceHub"/> instances.
    /// </summary>
    /// <param name="arguments">The arguments to parse.</param>
    /// <param name="newArgs">The new arguments after the player IDs and names have been processed.</param>
    /// <param name="startIndex">The index to start processing from.</param>
    /// <returns>A list of <see cref="ReferenceHub"/> instances.</returns>
    public static List<ReferenceHub> GetTargets(this ArraySegment<string> arguments, out string[] newArgs, int startIndex = 0)
        => RAUtils.ProcessPlayerIdOrNamesList(arguments, startIndex, out newArgs);

    /// <summary>
    /// Sets the field reference if this value is not null.
    /// </summary>
    /// <param name="value">The value to use.</param>
    /// <param name="field">The field reference to set.</param>
    /// <typeparam name="T">The type of the field.</typeparam>
    public static void SetFieldIfNotNull<T>(this T value, ref T field) where T : class {
        if (value != null)
            field = value;
    }

    /// <summary>
    /// Safely casts this value and sets the referenced field if the field's value is not null.
    /// </summary>
    /// <param name="value">The value to cast.</param>
    /// <param name="field">The field reference to set.</param>
    /// <typeparam name="T">The type to cast to.</typeparam>
    public static void SafeCastAndSetIfNotNull<T>(this object value, ref T field) where T : class {
        if (field != null && value is T t)
            field = t;
    }

    /// <summary>
    /// Adds the given value to the list if it is not null.
    /// </summary>
    /// <param name="list">The list to add the object to.</param>
    /// <param name="value">The value to add.</param>
    /// <typeparam name="T">The type of the list.</typeparam>
    /// <returns>Whether the value was added.</returns>
    public static bool AddIfNotNull<T>(this List<T> list, T value) where T : class {
        if (value == null)
            return false;
        list.Add(value);
        return true;
    }

}
