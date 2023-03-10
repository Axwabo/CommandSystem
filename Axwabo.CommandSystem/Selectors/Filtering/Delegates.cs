namespace Axwabo.CommandSystem.Selectors.Filtering;

/// <summary>A <see cref="ReferenceHub"/> predicate.</summary>
public delegate bool HubFilter(ReferenceHub hub);

/// <summary>
/// A <see cref="ReferenceHub"/> predicate with a parameter.
/// </summary>
/// <typeparam name="T">The type of the parameter.</typeparam>
public delegate bool ParameterizedHubFilter<in T>(ReferenceHub hub, T parameter);

/// <summary>A method that creates a <see cref="HubFilter"/> based on a string value.</summary>
public delegate HubFilter FilterSupplier(string value);
