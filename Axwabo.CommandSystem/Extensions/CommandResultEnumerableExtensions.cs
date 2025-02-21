using Axwabo.CommandSystem.Commands;

namespace Axwabo.CommandSystem.Extensions;

/// <summary>Extension methods for <see cref="CommandResult"/> and <see cref="CommandResultOnTarget"/> <see cref="IEnumerable{T}">enumerables</see>.</summary>
public static class CommandResultEnumerableExtensions
{

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

}
