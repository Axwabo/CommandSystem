namespace Axwabo.CommandSystem.PropertyManager.Resolvers;

/// <summary>Base interface to resolve a RA option icon from an attribute. You must implement <see cref="IOptionIconResolver{TAttribute}"/> due to reflection magic.</summary>
public interface IOptionIconResolver;

/// <summary>
/// An interface to resolve a Remote Admin option icon based on an attribute.
/// </summary>
/// <typeparam name="TAttribute">The type of the attribute to resolve the usages from.</typeparam>
public interface IOptionIconResolver<in TAttribute> : IOptionIconResolver where TAttribute : Attribute
{

    /// <summary>
    /// Creates an icon from an attribute.
    /// </summary>
    /// <param name="attribute">The attribute to create an icon from.</param>
    /// <returns>The resolved icon.</returns>
    BlinkingIcon CreateIconFromAttribute(TAttribute attribute);

}
