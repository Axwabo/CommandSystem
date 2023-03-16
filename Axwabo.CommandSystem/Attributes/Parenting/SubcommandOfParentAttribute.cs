using System;

namespace Axwabo.CommandSystem.Attributes.Parenting;

/// <summary>
/// An attribute that marks a command as a subcommand of a parent command.
/// </summary>
/// <seealso cref="UsesSubcommandAttribute"/>
[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
public sealed class SubcommandOfParentAttribute : Attribute
{

    /// <summary>The parent command type to register the subcommand to.</summary>
    public Type ParentType { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="SubcommandOfParentAttribute"/> class.
    /// </summary>
    /// <param name="parentType">The type of the parent command.</param>
    public SubcommandOfParentAttribute(Type parentType) => ParentType = parentType;

}
