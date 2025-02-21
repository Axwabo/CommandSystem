namespace Axwabo.CommandSystem.Commands;

/// <summary>Stores basic information about a <see cref="CommandBase"/>.</summary>
public sealed class BaseCommandProperties
{

    /// <summary>The name of the command.</summary>
    public string Name;

    /// <summary>The description of the command.</summary>
    public string Description;

    /// <summary>The aliases of the command.</summary>
    public string[] Aliases;

    /// <summary>The usages of the command.</summary>
    public string[] Usage;

    /// <summary>Minimum amount of arguments that have to be passed to a command.</summary>
    public int MinArguments;

    /// <summary>Whether the command can only be used by players.</summary>
    public bool PlayerOnly;

    /// <summary>
    /// Creates a new <see cref="BaseCommandProperties"/> based on the current instance.
    /// </summary>
    /// <returns>The cloned <see cref="BaseCommandProperties"/>.</returns>
    public BaseCommandProperties Clone() => new()
    {
        Name = Name,
        Description = Description,
        Aliases = Aliases,
        Usage = Usage,
        MinArguments = MinArguments,
        PlayerOnly = PlayerOnly
    };

}
