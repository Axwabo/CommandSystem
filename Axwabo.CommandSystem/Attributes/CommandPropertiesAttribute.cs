using Axwabo.CommandSystem.Attributes.Interfaces;

namespace Axwabo.CommandSystem.Attributes;

/// <summary>
/// Specifies multiple properties of a command.
/// </summary>
[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public sealed class CommandPropertiesAttribute : CommandTargetAttribute, ICommandName, IDescription, IAliases, IMinArguments
{

    /// <inheritdoc />
    public string Name { get; }

    /// <inheritdoc />
    public string Description { get; init; }

    /// <inheritdoc />
    public string[] Aliases { get; init; }

    /// <inheritdoc />
    public int MinArguments { get; init; }

    /// <summary>
    /// Initializes a new instance of the <see cref="CommandPropertiesAttribute"/> with all properties except <see cref="MinArguments"/>.
    /// </summary>
    /// <param name="targets">The command handler types.</param>
    /// <param name="name">The name of the command.</param>
    /// <param name="description">The description of the command.</param>
    /// <param name="aliases">Aliases for the command.</param>
    public CommandPropertiesAttribute(CommandHandlerType targets, string name, string description = null, params string[] aliases) : base(targets)
    {
        Name = name;
        Description = description;
        Aliases = aliases;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CommandPropertiesAttribute"/> with all properties.
    /// </summary>
    /// <param name="targets">The command handler types.</param>
    /// <param name="name">The name of the command.</param>
    /// <param name="minArguments">The minimum number of arguments required to execute the command.</param>
    /// <param name="description">The description of the command.</param>
    /// <param name="aliases">Aliases for the command.</param>
    public CommandPropertiesAttribute(CommandHandlerType targets, string name, int minArguments, string description = null, params string[] aliases) : base(targets)
    {
        Name = name;
        Description = description;
        Aliases = aliases;
        MinArguments = minArguments;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CommandPropertiesAttribute"/> with properties <see cref="Name"/>, <see cref="Description"/> and <see cref="Aliases"/>.
    /// </summary>
    /// <param name="name">The name of the command.</param>
    /// <param name="description">The description of the command.</param>
    /// <param name="aliases">Aliases for the command.</param>
    public CommandPropertiesAttribute(string name, string description = null, params string[] aliases) : this(CommandHandlerType.None, name, description, aliases)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CommandPropertiesAttribute"/> with all properties except <see cref="CommandTargetAttribute.Targets"/>.
    /// </summary>
    /// <param name="name">The name of the command.</param>
    /// <param name="minArguments">The minimum number of arguments required to execute the command.</param>
    /// <param name="description">The description of the command.</param>
    /// <param name="aliases">Aliases for the command.</param>
    public CommandPropertiesAttribute(string name, int minArguments, string description = null, params string[] aliases) : this(CommandHandlerType.None, name, minArguments, description, aliases)
    {
    }

}
