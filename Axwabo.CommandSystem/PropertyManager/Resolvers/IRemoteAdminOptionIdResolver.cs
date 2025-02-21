namespace Axwabo.CommandSystem.PropertyManager.Resolvers;

/// <summary>Base interface to resolve a RA option id. You must implement <see cref="IRemoteAdminOptionIdResolver{TAttribute}"/> due to reflection magic.</summary>
public interface IRemoteAdminOptionIdResolver;

/// <summary>
/// An interface to resolve a Remote Admin option identifier based on an attribute.
/// </summary>
/// <typeparam name="TAttribute">The type of the attribute to resolve the id from.</typeparam>
public interface IRemoteAdminOptionIdResolver<in TAttribute> : IRemoteAdminOptionIdResolver where TAttribute : Attribute
{

    /// <summary>
    /// Resolves the option id based on the attribute.
    /// </summary>
    /// <param name="attribute">The attribute to resolve the identifier from.</param>
    /// <returns>The resolved id.</returns>
    string ResolveIdentifier(TAttribute attribute);

}
