namespace Axwabo.CommandSystem.Selectors.Filtering;

/// <summary>Stores custom filters that can be used in advanced player selectors.</summary>
public static class CustomHubFilterRegistry
{

    private static readonly List<FilterEntry> RegisteredFilters = new();

    private static bool ValidateRegistration(IReadOnlyCollection<string> aliases) => aliases.Count != aliases.Count(alias => TryGetExisting(alias, out _));

    /// <summary>
    /// Registers a custom hub filter.
    /// </summary>
    /// <param name="filter">The filter to register.</param>
    /// <param name="aliases">The aliases that can be used to refer to the filter.</param>
    /// <returns>Whether any aliases were registered.</returns>
    public static bool Register(HubFilter filter, params string[] aliases)
    {
        if (!ValidateRegistration(aliases))
            return false;
        RegisteredFilters.Add(new FilterEntry(filter, aliases));
        return true;
    }

    /// <summary>
    /// Registers a custom hub filter that accepts a string value.
    /// </summary>
    /// <param name="filterSupplier">The filter supplier to register.</param>
    /// <param name="aliases">The aliases that can be used to refer to the filter.</param>
    /// <returns>Whether any aliases were registered.</returns>
    public static bool Register(FilterSupplier filterSupplier, params string[] aliases)
    {
        if (!ValidateRegistration(aliases))
            return false;
        RegisteredFilters.Add(new FilterEntry(filterSupplier, aliases));
        return true;
    }

    /// <summary>
    /// Attempts to get an existing filter entry based on an alias.
    /// </summary>
    /// <param name="alias">The alias to search for.</param>
    /// <param name="supplier">The filter entry that was found.</param>
    /// <returns>Whether a filter entry was found.</returns>
    public static bool TryGetExisting(string alias, out FilterEntry supplier)
    {
        foreach (var entry in RegisteredFilters)
        {
            if (!entry.IsValid)
                continue;
            foreach (var entryAlias in entry.Aliases)
            {
                if (!string.Equals(entryAlias, alias, StringComparison.OrdinalIgnoreCase))
                    continue;
                supplier = entry;
                return true;
            }
        }

        supplier = default;
        return false;
    }

    /// <summary>
    /// Gets an existing filter entry based on an alias.
    /// </summary>
    /// <param name="alias">The alias to search for.</param>
    /// <param name="value">The value to pass to the filter if it requires it.</param>
    /// <returns>The filter entry that was found.</returns>
    public static HubFilter Get(string alias, string value) => TryGetExisting(alias, out var filter) ? filter.CreateFilter(value) : null;

}
