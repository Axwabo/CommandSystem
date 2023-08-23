using Axwabo.CommandSystem.Attributes.Interfaces;

namespace Axwabo.CommandSystem.Attributes;

/// <summary>
/// An attribute that defines usages of a command.
/// </summary>
/// <remarks>Usage strings should not contain the command name.</remarks>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = false)]
public sealed class UsageAttribute : Attribute, IUsage
{

    /// <inheritdoc />
    public string[] Usage { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="UsageAttribute"/> class.
    /// </summary>
    /// <param name="usage">The usage list.</param>
    public UsageAttribute(params string[] usage) => Usage = usage;

}
