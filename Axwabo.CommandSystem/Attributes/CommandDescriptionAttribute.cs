using Axwabo.CommandSystem.Attributes.Interfaces;

namespace Axwabo.CommandSystem.Attributes;

/// <summary>
/// Sets a static description for the command.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = false)]
public sealed class CommandDescriptionAttribute : Attribute, IDescription
{

    /// <inheritdoc />
    public string Description { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="CommandDescriptionAttribute"/> class.
    /// </summary>
    /// <param name="description">The description of the command.</param>
    public CommandDescriptionAttribute(string description) => Description = description;

}
