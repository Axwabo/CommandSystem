namespace Axwabo.CommandSystem.PropertyManager.Resolvers;

/// <summary>Base interface to resolve whether a command can only be executed by players. You must implement <see cref="IPlayerOnlyResolver{TAttribute}"/> due to reflection magic.</summary>
public interface IPlayerOnlyResolver;

/// <summary>
/// An interface to resolve whether a command is player-only.
/// </summary>
/// <typeparam name="TAttribute">The type of the attribute to resolve the manager from.</typeparam>
public interface IPlayerOnlyResolver<in TAttribute> : IPlayerOnlyResolver where TAttribute : Attribute
{

    /// <summary>
    /// Resolves the player-only status based on the attribute.
    /// </summary>
    /// <param name="attribute">The attribute to resolve the status from.</param>
    /// <returns>The resolved status.</returns>
    bool ResolvePlayerOnly(TAttribute attribute);

}
