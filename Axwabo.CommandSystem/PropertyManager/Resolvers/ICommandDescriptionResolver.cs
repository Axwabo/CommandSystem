namespace Axwabo.CommandSystem.PropertyManager.Resolvers;

/// <summary>Base interface to resolve the command description You must implement <see cref="ICommandDescriptionResolver{TAttribute}"/> due to reflection magic.</summary>
public interface ICommandDescriptionResolver
{

}

/// <summary>
/// An interface to resolve the command description based on an attribute.
/// </summary>
/// <typeparam name="TAttribute">The type of the attribute to resolve the description from.</typeparam>
public interface ICommandDescriptionResolver<in TAttribute> : ICommandDescriptionResolver where TAttribute : Attribute
{

    /// <summary>
    /// Resolves command description based on the attribute.
    /// </summary>
    /// <param name="attribute">The attribute to resolve the description from.</param>
    /// <returns>The resolved description.</returns>
    string ResolveDescription(TAttribute attribute);

}
