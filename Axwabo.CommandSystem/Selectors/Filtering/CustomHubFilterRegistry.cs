using System;
using System.Collections.Generic;
using System.Linq;

namespace Axwabo.CommandSystem.Selectors.Filtering;

public static class CustomHubFilterRegistry {

    private static readonly List<FilterEntry> RegisteredFilters = new();

    private static bool ValidateRegistration(string[] aliases) => aliases.Length != aliases.Count(alias => TryGetExisting(alias, out _));

    public static bool Register(HubFilter filter, params string[] aliases) {
        if (!ValidateRegistration(aliases))
            return false;
        RegisteredFilters.Add(new FilterEntry(filter, aliases));
        return true;
    }

    public static bool Register(Func<string, HubFilter> filterSupplier, params string[] aliases) {
        if (!ValidateRegistration(aliases))
            return false;
        RegisteredFilters.Add(new FilterEntry(filterSupplier, aliases));
        return true;
    }

    public static bool TryGetExisting(string alias, out FilterEntry supplier) {
        foreach (var entry in RegisteredFilters) {
            if (!entry.IsValid)
                continue;
            foreach (var entryAlias in entry.Aliases) {
                if (!string.Equals(entryAlias, alias, StringComparison.OrdinalIgnoreCase))
                    continue;
                supplier = entry;
                return true;
            }
        }

        supplier = default;
        return false;
    }

    public static HubFilter Get(string alias, string value) => TryGetExisting(alias, out var filter) ? filter.CreateFilter(value) : null;

}
