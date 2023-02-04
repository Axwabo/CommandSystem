using System;

namespace Axwabo.CommandSystem.Selectors.Filtering;

public readonly struct FilterEntry {

    public readonly HubFilter Filter;

    public readonly string[] Aliases;

    public bool IsValid => Filter != null;

    public FilterEntry(HubFilter filter, params string[] aliases) {
        Aliases = aliases is {Length: not 0} ? aliases : throw new ArgumentException("At least one alias must be specified", nameof(aliases));
        Filter = filter ?? throw new ArgumentNullException(nameof(filter));
    }

}
