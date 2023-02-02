using System;
using System.Collections.Generic;

namespace Axwabo.CommandSystem.Selectors;

public static class AtSelectorProcessor {

    private static readonly char[] ValidChars = "arsARS".ToCharArray();

    public static bool ProcessString(string formatted, bool keepEmptyEntries, out List<ReferenceHub> targets, out string[] newArgs) {
        char selectorChar;
        if (formatted.Length < 1 || !IsValidChar(selectorChar = formatted[0])) {
            targets = null;
            newArgs = null;
            return false;
        }

        if (formatted.Length < 2) {
            targets = ExecuteSelector(selectorChar, null, -1);
            newArgs = Array.Empty<string>();
            return true;
        }

        throw new NotImplementedException("Advanced @selector parsing");
    }

    private static List<ReferenceHub> ExecuteSelector(char selectorChar, List<HubFilter> filters, int limit) => GetDefaultTargets(
        GetAllFiltered(filters),
        selectorChar,
        limit
    );

    private static List<ReferenceHub> GetAllFiltered(List<HubFilter> filters) {
        var all = PlayerSelectionManager.AllPlayers;
        if (filters == null)
            return all;
        for (var i = 0; i < all.Count; i++) {
            var hub = all[i];
            var failed = MatchesAllFilters(hub, filters);
            if (!failed)
                continue;
            all.RemoveAt(i);
            i--;
        }

        return all;
    }

    private static bool MatchesAllFilters(ReferenceHub hub, List<HubFilter> filters) {
        var failed = false;
        foreach (var f in filters)
            if (!f(hub)) {
                failed = true;
                break;
            }

        return failed;
    }

    private static List<ReferenceHub> GetDefaultTargets(List<ReferenceHub> candidates, char selector, int limit) => selector switch {
        'a' or 'A' => candidates,
        'r' or 'R' => GetRandom(candidates, limit),
        's' or 'S' => Self,
        _ => throw new ArgumentOutOfRangeException(nameof(selector), selector, null)
    };

    private static List<ReferenceHub> GetRandom(List<ReferenceHub> all, int limit) {
        if (limit < 0)
            return new HubCollection(all.RandomItem());
        var list = new HubCollection(limit);
        for (var i = 0; i < limit; i++) {
            var count = all.Count;
            if (count == 0)
                break;
            var x = UnityEngine.Random.Range(0, count);
            list.Add(all[x]);
            all.RemoveAt(x);
        }

        return list;
    }

    private static List<ReferenceHub> Self => HubCollection.From(PlayerSelectionManager.CurrentSender);

    public static bool IsValidChar(char c) => ValidChars.Contains(c);

}
