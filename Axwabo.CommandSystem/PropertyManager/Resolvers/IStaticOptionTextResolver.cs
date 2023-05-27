namespace Axwabo.CommandSystem.PropertyManager.Resolvers;

/// <summary>Base interface to resolve a static RA option text. You must implement <see cref="IStaticOptionTextResolver{TAttribute}"/> due to reflection magic.</summary>
public interface IStaticOptionTextResolver
{

}

/// <summary>
/// An interface to resolve a static Remote Admin option text based on an attribute.
/// </summary>
/// <typeparam name="TAttribute">The type of the attribute to resolve the id from.</typeparam>
public interface IStaticOptionTextResolver<in TAttribute> : IStaticOptionTextResolver where TAttribute : Attribute
{

    /// <summary>
    /// Resolves the static content based on the attribute.
    /// </summary>
    /// <param name="attribute">The attribute to resolve the text from.</param>
    /// <returns>The resolved static text.</returns>
    string ResolveContent(TAttribute attribute);

}
