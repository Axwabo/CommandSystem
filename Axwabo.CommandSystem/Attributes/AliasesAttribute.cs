using Axwabo.CommandSystem.Attributes.Interfaces;

namespace Axwabo.CommandSystem.Attributes;

/// <summary>
/// Attribute for defining aliases for a command.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = false)]
public sealed class AliasesAttribute : Attribute, IAliases
{

    /// <inheritdoc />
    public string[] Aliases { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="AliasesAttribute"/> class.
    /// </summary>
    /// <param name="aliases">The aliases for the command.</param>
    public AliasesAttribute(params string[] aliases) => Aliases = aliases;

}
