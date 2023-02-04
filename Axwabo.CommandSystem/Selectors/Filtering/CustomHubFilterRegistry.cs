using System;
using System.Collections.Generic;

namespace Axwabo.CommandSystem.Selectors.Filtering;

public static class CustomHubFilterRegistry {

    private static readonly List<FilterEntry> RegisteredFilters = new();

    public static bool Register(HubFilter filter, params string[] aliases) {
        var failing = 0;
        foreach (var alias in aliases)
            if (TryGetExisting(alias, out _))
                failing++;
        if (failing == aliases.Length)
            return false;
        RegisteredFilters.Add(new FilterEntry(filter, aliases));
        return true;
    }

    public static bool TryGetExisting(string alias, out HubFilter filter) {
        foreach (var entry in RegisteredFilters) {
            if (!entry.IsValid)
                continue;
            foreach (var entryAlias in entry.Aliases) {
                if (!string.Equals(entryAlias, alias, StringComparison.OrdinalIgnoreCase))
                    continue;
                filter = entry.Filter;
                return true;
            }
        }

        filter = null;
        return false;
    }

    public static HubFilter Get(string alias) => TryGetExisting(alias, out var filter) ? filter : null;

}
