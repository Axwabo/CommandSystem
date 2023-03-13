using System;

namespace Axwabo.CommandSystem.PropertyManager.Resolvers;

/// <summary>Base interface to resolve command aliases. You must implement <see cref="ICommandAliasResolver{TAttribute}"/> due to reflection magic.</summary>
public interface ICommandAliasResolver
{

}

/// <summary>
/// An interface to resolve aliases for a command based on an attribute.
/// </summary>
/// <typeparam name="TAttribute">The type of the attribute to resolve the aliases from.</typeparam>
public interface ICommandAliasResolver<in TAttribute> : ICommandAliasResolver where TAttribute : Attribute
{

    /// <summary>
    /// Resolves aliases based on the attribute.
    /// </summary>
    /// <param name="attribute">The attribute to resolve aliases from.</param>
    /// <returns>The resolved aliases.</returns>
    string[] ResolveAliases(TAttribute attribute);

}
