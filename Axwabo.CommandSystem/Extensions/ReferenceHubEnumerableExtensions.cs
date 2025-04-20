using LabApi.Features.Wrappers;

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

    /// <summary>
    /// Maps each <see cref="ReferenceHub"/> of the enumerable to a LabApi <see cref="Player"/> wrapper.
    /// </summary>
    /// <param name="hubs">The enumerable to map.</param>
    /// <returns>An <see cref="IEnumerable{T}"/> of LabApi <see cref="Player"/> wrappers.</returns>
    public static IEnumerable<Player> ToPlayers(this IEnumerable<ReferenceHub> hubs) => hubs.Select(Player.Get);

    /// <summary>
    /// Maps and collects the <see cref="ReferenceHub"/> enumerable to a <see cref="List{T}"/> of LabApi <see cref="Player"/> wrappers.
    /// </summary>
    /// <param name="hubs">The enumerable to convert.</param>
    /// <returns>A <see cref="List{T}"/> with each <see cref="ReferenceHub"/> mapped to a <see cref="Player"/>.</returns>
    public static List<Player> ToPlayerList(this IEnumerable<ReferenceHub> hubs) => hubs.ToPlayers().ToList();

}
