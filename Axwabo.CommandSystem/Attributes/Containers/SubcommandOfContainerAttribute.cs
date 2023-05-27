namespace Axwabo.CommandSystem.Attributes.Containers;

/// <summary>
/// An attribute that marks a command as a subcommand of a container command. This attribute is inherited.
/// </summary>
/// <seealso cref="UsesSubcommandsAttribute"/>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public sealed class SubcommandOfContainerAttribute : Attribute
{

    /// <summary>The container command type to register the subcommand to.</summary>
    public Type ContainerType { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="SubcommandOfContainerAttribute"/> class.
    /// </summary>
    /// <param name="containerType">The type of the container command.</param>
    public SubcommandOfContainerAttribute(Type containerType) => ContainerType = containerType;

}
