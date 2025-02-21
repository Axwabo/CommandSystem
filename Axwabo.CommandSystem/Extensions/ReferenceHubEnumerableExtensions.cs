namespace Axwabo.CommandSystem.Extensions;

/// <summary>Extension methods for <see cref="ReferenceHub"/> <see cref="IEnumerable{T}">enumerables</see>.</summary>
public static class ReferenceHubEnumerableExtensions
{

    /// <summary>
    /// Combines the nicknames of the given hubs into a single string.
    /// </summary>
    /// <param name="hubs">The hubs to combine.</param>
    /// <param name="separator">The string to use as a separator.<paramref name="separator" /> is included in the returned string only if <paramref name="hubs" /> has more than one element.</param>
    /// <returns>The combined string.</returns>
    public static string CombineNicknames(this IEnumerable<ReferenceHub> hubs, string separator = ", ")
        => string.Join(separator, hubs.Select(p => p.nicknameSync.MyNick));

}
