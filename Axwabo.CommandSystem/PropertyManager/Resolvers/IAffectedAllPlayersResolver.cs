using System;
using Axwabo.CommandSystem.Commands.MessageOverrides;

namespace Axwabo.CommandSystem.PropertyManager.Resolvers;

/// <summary>Base interface to resolve an <see cref="IAffectedAllPlayersMessageGenerator"/>. You must implement <see cref="IAffectedAllPlayersResolver{TAttribute}"/> due to reflection magic.</summary>
public interface IAffectedAllPlayersResolver
{

}

/// <summary>
/// An interface to resolve an <see cref="IAffectedAllPlayersMessageGenerator"/> based on an attribute.
/// </summary>
/// <typeparam name="TAttribute">The type of the attribute to resolve the message generator from.</typeparam>
public interface IAffectedAllPlayersResolver<in TAttribute> : IAffectedAllPlayersResolver where TAttribute : Attribute
{

    /// <summary>
    /// Resolves the <see cref="IAffectedAllPlayersMessageGenerator"/> based on the attribute.
    /// </summary>
    /// <param name="attribute">The attribute to resolve the message generator from.</param>
    /// <returns>The resolved <see cref="IAffectedAllPlayersMessageGenerator"/>.</returns>
    IAffectedAllPlayersMessageGenerator ResolveGenerator(TAttribute attribute);

}
