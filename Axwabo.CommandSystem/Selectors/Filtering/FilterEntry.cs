using System;

namespace Axwabo.CommandSystem.Selectors.Filtering;

public readonly struct FilterEntry {

    public readonly HubFilter Filter;

    public readonly Func<string, HubFilter> FilterSupplier;

    public readonly string[] Aliases;

    public bool IsValid => FilterSupplier != null && Filter != null;

    public FilterEntry(HubFilter filter, params string[] aliases) {
        Aliases = aliases is {Length: not 0} ? aliases : throw new ArgumentException("At least one alias must be specified", nameof(aliases));
        Filter = filter ?? throw new ArgumentNullException(nameof(filter));
        FilterSupplier = null;
    }

    public FilterEntry(Func<string, HubFilter> supplier, params string[] aliases) {
        Aliases = aliases is {Length: not 0} ? aliases : throw new ArgumentException("At least one alias must be specified", nameof(aliases));
        FilterSupplier = supplier ?? throw new ArgumentNullException(nameof(supplier));
        Filter = null;
    }

    public HubFilter CreateFilter(string value) => FilterSupplier?.Invoke(value) ?? Filter;

}
