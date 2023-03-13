using System;
using Axwabo.CommandSystem.Commands.MessageOverrides;

namespace Axwabo.CommandSystem.PropertyManager.Resolvers;

/// <summary>Base interface to resolve an <see cref="IAffectedOnePlayerMessageGenerator"/>. You must implement <see cref="IAffectedOnePlayerResolver{TAttribute}"/> due to reflection magic.</summary>
public interface IAffectedOnePlayerResolver
{

}

/// <summary>
/// An interface to resolve an <see cref="IAffectedOnePlayerMessageGenerator"/> based on an attribute.
/// </summary>
/// <typeparam name="TAttribute">The type of the attribute to resolve the message generator from.</typeparam>
public interface IAffectedOnePlayerResolver<in TAttribute> : IAffectedOnePlayerResolver where TAttribute : Attribute
{

    /// <summary>
    /// Resolves the <see cref="IAffectedOnePlayerMessageGenerator"/> based on the attribute.
    /// </summary>
    /// <param name="attribute">The attribute to resolve the message generator from.</param>
    /// <returns>The resolved <see cref="IAffectedOnePlayerMessageGenerator"/>.</returns>
    IAffectedOnePlayerMessageGenerator ResolveGenerator(TAttribute attribute);

}
