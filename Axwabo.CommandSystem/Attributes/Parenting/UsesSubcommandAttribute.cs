using System;

namespace Axwabo.CommandSystem.Attributes.Parenting;

/// <summary>
/// Specifies that the given type should be a subcommand of the parent command.
/// </summary>
/// <seealso cref="SubcommandOfParentAttribute"/>
[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
public sealed class UsesSubcommandAttribute : Attribute
{

    /// <summary>The subcommand to register to the instance this attribute is on.</summary>
    public Type SubcommandType { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="UsesSubcommandAttribute"/> class.
    /// </summary>
    /// <param name="subcommandType">The type of the subcommand.</param>
    public UsesSubcommandAttribute(Type subcommandType) => SubcommandType = subcommandType;

}
