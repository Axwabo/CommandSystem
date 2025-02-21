using Axwabo.CommandSystem.Commands.MessageOverrides;

namespace Axwabo.CommandSystem.PropertyManager.Resolvers;

/// <summary>Base interface to resolve an <see cref="IAffectedMultiplePlayersMessageGenerator"/>. You must implement <see cref="IAffectedMultiplePlayersResolver{TAttribute}"/> due to reflection magic.</summary>
public interface IAffectedMultiplePlayersResolver;

/// <summary>
/// An interface to resolve an <see cref="IAffectedMultiplePlayersMessageGenerator"/> based on an attribute.
/// </summary>
/// <typeparam name="TAttribute">The type of the attribute to resolve the message generator from.</typeparam>
public interface IAffectedMultiplePlayersResolver<in TAttribute> : IAffectedMultiplePlayersResolver where TAttribute : Attribute
{

    /// <summary>
    /// Resolves the <see cref="IAffectedMultiplePlayersMessageGenerator"/> based on the attribute.
    /// </summary>
    /// <param name="attribute">The attribute to resolve the message generator from.</param>
    /// <returns>The resolved <see cref="IAffectedMultiplePlayersMessageGenerator"/>.</returns>
    IAffectedMultiplePlayersMessageGenerator ResolveGenerator(TAttribute attribute);

}
