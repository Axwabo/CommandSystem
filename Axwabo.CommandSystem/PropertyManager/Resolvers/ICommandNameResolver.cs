namespace Axwabo.CommandSystem.PropertyManager.Resolvers;

/// <summary>Base interface to resolve the command name. You must implement <see cref="ICommandNameResolver{TAttribute}"/> due to reflection magic.</summary>
public interface ICommandNameResolver;

/// <summary>
/// An interface to resolve the command name based on an attribute.
/// </summary>
/// <typeparam name="TAttribute">The type of the attribute to resolve the name from.</typeparam>
public interface ICommandNameResolver<in TAttribute> : ICommandNameResolver where TAttribute : Attribute
{

    /// <summary>
    /// Resolves command name based on the attribute.
    /// </summary>
    /// <param name="attribute">The attribute to resolve the name from.</param>
    /// <returns>The resolved name.</returns>
    string ResolveName(TAttribute attribute);

}
