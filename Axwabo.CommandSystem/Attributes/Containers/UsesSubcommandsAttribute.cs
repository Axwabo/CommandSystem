using System;

namespace Axwabo.CommandSystem.Attributes.Containers;

/// <summary>
/// Specifies that the given types should be subcommands of the container command.
/// </summary>
/// <seealso cref="SubcommandOfContainerAttribute"/>
[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
public sealed class UsesSubcommandsAttribute : Attribute
{

    /// <summary>The subcommands to register to the instance this attribute is on.</summary>
    public Type[] SubcommandTypes { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="UsesSubcommandsAttribute"/> class.
    /// </summary>
    /// <param name="subcommandTypes">The types of subcommands.</param>
    public UsesSubcommandsAttribute(params Type[] subcommandTypes) => SubcommandTypes = subcommandTypes;

}
