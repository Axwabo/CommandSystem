namespace Axwabo.CommandSystem.Selectors.Filtering;

public delegate bool HubFilter(ReferenceHub hub);

public delegate bool ParameterizedHubFilter<in T>(ReferenceHub hub, T parameter);
