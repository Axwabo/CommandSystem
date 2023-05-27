namespace Axwabo.CommandSystem.PropertyManager.Resolvers;

/// <summary>Base interface to resolve command usages. You must implement <see cref="ICommandUsageResolver{TAttribute}"/> due to reflection magic.</summary>
public interface ICommandUsageResolver
{

}

/// <summary>
/// An interface to resolve usages for a command based on an attribute.
/// </summary>
/// <typeparam name="TAttribute">The type of the attribute to resolve the usages from.</typeparam>
public interface ICommandUsageResolver<in TAttribute> : ICommandUsageResolver where TAttribute : Attribute
{

    /// <summary>
    /// Resolves usages based on the attribute.
    /// </summary>
    /// <param name="attribute">The attribute to resolve usages from.</param>
    /// <returns>The resolved usages.</returns>
    string[] ResolveUsage(TAttribute attribute);

}
