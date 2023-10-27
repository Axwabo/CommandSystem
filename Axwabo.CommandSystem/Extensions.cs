using Utils;

namespace Axwabo.CommandSystem;

/// <summary>
/// Common extension methods.
/// </summary>
public static class Extensions
{

    /// <summary>
    /// Combines the nicknames of the given hubs into a single string.
    /// </summary>
    /// <param name="hubs">The hubs to combine.</param>
    /// <param name="separator">The string to use as a separator.<paramref name="separator" /> is included in the returned string only if <paramref name="hubs" /> has more than one element.</param>
    /// <returns>The combined string.</returns>
    public static string CombineNicknames(this IEnumerable<ReferenceHub> hubs, string separator = ", ")
        => string.Join(separator, hubs.Select(p => p.nicknameSync.MyNick));

    /// <summary>
    /// Combines the nicknames of the given hubs from <see cref="CommandResultOnTarget"/> objects into a single string.
    /// </summary>
    /// <param name="results">The results to combine.</param>
    /// <param name="separator">The string to use as a separator.<paramref name="separator" /> is included in the returned string only if <paramref name="results" /> has more than one element.</param>
    /// <returns>The combined string.</returns>
    public static string CombineNicknames(this IEnumerable<CommandResultOnTarget> results, string separator = ", ")
        => string.Join(separator, results.Select(p => p.Nick));

    /// <summary>
    /// Concatenates the responses of the given <see cref="CommandResult"/> instances using the specified separator.
    /// </summary>
    /// <param name="results">The results to concatenate.</param>
    /// <param name="separator">The string to use as a separator.<paramref name="separator" /> is included in the returned string only if <paramref name="results" /> has more than one element.</param>
    /// <returns>The concatenated string.</returns>
    public static string JoinResults(this IEnumerable<CommandResult> results, string separator = "\n")
        => string.Join(separator, results.Select(p => p.Response));

    /// <summary>
    /// Concatenates the responses of the given <see cref="CommandResultOnTarget"/> instances using the specified separator.
    /// </summary>
    /// <param name="results">The results to concatenate.</param>
    /// <param name="separator">The string to use as a separator.<paramref name="separator" /> is included in the returned string only if <paramref name="results" /> has more than one element.</param>
    /// <param name="includeNicknames">Whether the response should be prepended with the target's nickname.</param>
    /// <returns>The concatenated string.</returns>
    public static string JoinResults(this IEnumerable<CommandResultOnTarget> results, string separator = "\n", bool includeNicknames = true)
        => string.Join(separator, results.Select(p => (includeNicknames ? p.Nick + ": " : "") + p.Response));

    /// <summary>
    /// Concatenates the members of a string array segment starting from the given index using the specified separator between each member.
    /// </summary>
    /// <param name="arguments">The string to use as a separator.<paramref name="separator" /> is included in the returned string only if <paramref name="arguments" /> has more than one element.</param>
    /// <param name="start">The index to start joining from.</param>
    /// <param name="separator">The string to use as a separator.<paramref name="separator" /> is included in the returned string only if the new segment has more than one element.</param>
    /// <returns>The joined string.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown if the index indicates an array with a negative count:<br/>
    /// - the segment was already empty and the index is greater than zero<br/>
    /// - the index is greater than the segment's length.
    /// </exception>
    public static string Join(this ArraySegment<string> arguments, int start = 0, string separator = " ")
        => string.Join(separator, arguments.Segment(start));

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
    public static string Substring(this string s, int searchStart, params char[] separators)
    {
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
    public static void SetFieldIfNotNull<T>(this T value, ref T field) where T : class
    {
        if (value != null)
            field = value;
    }

    /// <summary>
    /// Safely casts this value and sets the referenced field if the field's value is null.
    /// </summary>
    /// <param name="value">The value to cast.</param>
    /// <param name="field">The field reference to set.</param>
    /// <typeparam name="T">The type to cast to.</typeparam>
    /// <returns>Whether the value was set.</returns>
    public static bool SafeCastAndSetIfNull<T>(this object value, ref T field) where T : class
    {
        if (field != null || value is not T t)
            return false;
        field = t;
        return true;
    }

    /// <summary>
    /// Adds the given value to the list if it is not null.
    /// </summary>
    /// <param name="list">The list to add the object to.</param>
    /// <param name="value">The value to add.</param>
    /// <typeparam name="T">The type of the list.</typeparam>
    /// <returns>Whether the value was added.</returns>
    public static bool AddIfNotNull<T>(this List<T> list, T value) where T : class
    {
        if (value == null)
            return false;
        list.Add(value);
        return true;
    }

    /// <summary>
    /// Safely casts the value and adds it to the list.
    /// </summary>
    /// <param name="list">The list to add the object to.</param>
    /// <param name="value">The value to add.</param>
    /// <typeparam name="T">The type of the list.</typeparam>
    /// <returns>Whether the value was added.</returns>
    public static bool SafeCastAndAdd<T>(this List<T> list, object value) where T : class
    {
        if (value is not T t)
            return false;
        list.Add(t);
        return true;
    }

    /// <summary>
    /// Invokes the instance method if the method has a single parameter and the argument can be assigned to that parameter.
    /// </summary>
    /// <param name="method">The method to invoke.</param>
    /// <param name="instance">The instance to invoke the method on.</param>
    /// <param name="argument">The argument to pass to the method.</param>
    /// <returns>The return value of the method, or <see langword="null"/> if the method could not be invoked.</returns>
    public static object InvokeIfSingleParameterMatchesType(this MethodInfo method, object instance, object argument)
    {
        if (method == null)
            return null;
        var parameters = method.GetParameters();
        return parameters is {Length: 1} && parameters[0].ParameterType.IsInstanceOfType(argument)
            ? method.Invoke(instance, new[] {argument})
            : null;
    }

    /// <summary>
    /// Invokes the instance method if the method has a single parameter and the argument can be assigned to that parameter and casts the result to <typeparamref name="TReturn"/>.
    /// </summary>
    /// <param name="method">The method to invoke.</param>
    /// <param name="instance">The instance to invoke the method on.</param>
    /// <param name="argument">The argument to pass to the method.</param>
    /// <typeparam name="TReturn">The type to cast the return value to.</typeparam>
    /// <returns>The return value of the method, or <see langword="default"/> if the method could not be invoked.</returns>
    public static TReturn InvokeIfSingleParameterMatchesType<TReturn>(this MethodInfo method, object instance, object argument)
        => (TReturn) method.InvokeIfSingleParameterMatchesType(instance, argument);

}
