using System;

namespace Axwabo.CommandSystem.Selectors.Filtering;

/// <summary>
/// A struct that stores a <see cref="HubFilter"/> or creates one based on a supplied value.
/// </summary>
public readonly struct FilterEntry
{

    /// <summary>An already defined <see cref="HubFilter"/>.</summary>
    public readonly HubFilter Filter;

    /// <summary>A method to create a <see cref="HubFilter"/> based on a string value.</summary>
    public readonly FilterSupplier Supplier;

    /// <summary>Possible names for the filter.</summary>
    public readonly string[] Aliases;

    /// <summary>Determines whether this entry is valid.</summary>
    public bool IsValid => (Supplier != null || Filter != null) && Aliases != null;

    /// <summary>
    /// Creates a new <see cref="FilterEntry"/> with the given <see cref="HubFilter"/> and aliases.
    /// </summary>
    /// <param name="filter">The filter to use.</param>
    /// <param name="aliases">The aliases for the filter.</param>
    /// <exception cref="ArgumentException">Thrown if no aliases are specified.</exception>
    /// <exception cref="ArgumentNullException">Thrown if the filter is null.</exception>
    public FilterEntry(HubFilter filter, params string[] aliases)
    {
        Aliases = aliases is {Length: not 0} ? aliases : throw new ArgumentException("At least one alias must be specified", nameof(aliases));
        Filter = filter ?? throw new ArgumentNullException(nameof(filter));
        Supplier = null;
    }

    /// <summary>
    /// Creates a new <see cref="FilterEntry"/> with the given <see cref="FilterSupplier"/> and aliases.
    /// </summary>
    /// <param name="supplier">The method to create a filter.</param>
    /// <param name="aliases">The aliases for the filter.</param>
    /// <exception cref="ArgumentException">Thrown if no aliases are specified.</exception>
    /// <exception cref="ArgumentNullException">Thrown if the supplier is null.</exception>
    public FilterEntry(FilterSupplier supplier, params string[] aliases)
    {
        Aliases = aliases is {Length: not 0} ? aliases : throw new ArgumentException("At least one alias must be specified", nameof(aliases));
        Supplier = supplier ?? throw new ArgumentNullException(nameof(supplier));
        Filter = null;
    }

    /// <summary>
    /// Creates a <see cref="HubFilter"/> based on the value or returns the already defined filter.
    /// </summary>
    /// <param name="value">The value to use.</param>
    /// <returns>The created filter.</returns>
    public HubFilter CreateFilter(string value) => Supplier?.Invoke(value) ?? Filter;

}
