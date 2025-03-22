using Axwabo.CommandSystem.Commands.Interfaces;
using Axwabo.CommandSystem.Exceptions;
using Axwabo.CommandSystem.Extensions;
using Axwabo.CommandSystem.Permissions;
using Axwabo.CommandSystem.PropertyManager;

namespace Axwabo.CommandSystem.Commands;

/// <summary>
/// Base class for creating commands.
/// </summary>
public abstract class CommandBase
{

    private readonly BaseCommandProperties _properties;

    /// <summary>The name of the command.</summary>
    public virtual string Name => _properties.Name;

    /// <summary>A string that describes the command.</summary>
    public virtual string Description => _properties.Description;

    /// <summary>An array of aliases for the command.</summary>
    public virtual string[] Aliases => _properties.Aliases;

    /// <summary>An array of usage examples for the command.</summary>
    public virtual string[] Usage => _properties.Usage;

    /// <summary>The minimum amount of arguments required to execute the command.</summary>
    protected virtual int MinArguments => _properties.MinArguments;

    /// <summary>Whether this command is restricted to <see cref="PlayerCommandSender"/>s.</summary>
    protected virtual bool PlayerOnly => _properties.PlayerOnly;

    /// <summary>Shows all possible usages if any with a "Usage(s):" prefix.</summary>
    /// <example>
    /// <code>
    /// Usage: myCommand [arg]
    /// </code>
    /// <code>
    /// Usages:
    /// myCommand [arg1]
    /// myCommand [arg2]
    /// </code>
    /// </example>
    public string CombinedUsage
    {
        get
        {
            var name = Name;
            var count = Usage is {Length: var l} ? l : 0;
            return count == 0 ? "" : $"{"Usage".Pluralize(count)}:{(count == 1 ? " " : "\n")}{name} {string.Join($"\n{name} ", Usage)}".TrimEnd();
        }
    }

    /// <summary>A permission checker for the command.</summary>
    protected virtual IPermissionChecker Permissions { get; }

    // ReSharper disable VirtualMemberCallInConstructor
    /// <summary>
    /// Creates a new <see cref="CommandBase"/> instance.
    /// Properties are resolved based on the type.
    /// </summary>
    /// <exception cref="InvalidNameException">If the <see cref="Name">command name property</see> is not overriden and is not specified by attributes on the class.</exception>
    protected CommandBase() : this(null)
    {
    }

    /// <summary>
    /// Creates a new <see cref="CommandBase"/> instance based on the supplied <see cref="BaseCommandProperties">properties</see>.
    /// If <paramref name="properties"/> is null, <see cref="BaseCommandPropertyManager.ResolveProperties"/> will be invoked to get properties.
    /// </summary>
    /// <exception cref="InvalidNameException">If the <see cref="Name">command name property</see> is not overriden and is not specified in <paramref name="properties"/>.</exception>
    protected CommandBase(BaseCommandProperties properties)
    {
        _properties = properties?.Clone() ?? BaseCommandPropertyManager.ResolveProperties(GetType());
        if (string.IsNullOrWhiteSpace(Name))
            throw new InvalidNameException($"Command name on type {GetType().FullName} is not set. Are you missing an attribute or custom name resolver?");
        Permissions = BaseCommandPropertyManager.ResolvePermissionChecker(this);
    }

    /// <summary>
    /// Executes the command with all checks.
    /// </summary>
    /// <param name="arguments">The arguments passed to the command.</param>
    /// <param name="sender">The sender of the command.</param>
    /// <returns>The result of execution.</returns>
    public CommandResult ExecuteBase(ArraySegment<string> arguments, CommandSender sender)
    {
        var playerOnly = CheckIfPlayerOnly(arguments, sender);
        if (playerOnly.HasValue)
            return playerOnly.Value;

        var permissions = Permissions;
        var permissionCheck = permissions.CheckSafe(sender);
        if (!permissionCheck)
            return permissionCheck;
        var pre = this is IPreExecutionFilter filter ? filter.OnBeforeExecuted(arguments, sender) : null;
        return pre ?? (
            arguments.Count < MinArguments
                ? OnNotEnoughArguments(arguments, sender, MinArguments)
                : Execute(arguments, sender)
        );
    }

    private CommandResult? CheckIfPlayerOnly(ArraySegment<string> arguments, CommandSender sender)
        => sender is PlayerCommandSender
            ? CommandResult.Null
            : this is IPlayerOnlyCommand playerOnly
                ? playerOnly.OnNotPlayer(arguments, sender)
                : PlayerOnly
                    ? CommandResult.Failed(DefaultCommandMessages.MustBePlayer)
                    : CommandResult.Null;

    /// <summary>
    /// Generates a response when not enough arguments are provided.
    /// </summary>
    /// <param name="arguments">The arguments passed to the command.</param>
    /// <param name="sender">The sender of the command.</param>
    /// <param name="required">The minimum amount of arguments required to execute the command.</param>
    /// <returns>The result of failure.</returns>
    protected CommandResult OnNotEnoughArguments(ArraySegment<string> arguments, CommandSender sender, int required)
    {
        var custom = this is INotEnoughArgumentsHandler notEnough
            ? notEnough.OnNotEnoughArgumentsProvided(arguments, sender, required)
            : CommandResult.Null;
        return custom ?? $"!You need to provide at least {"argument".PluralizeWithCount(required)}! {CombinedUsage}".TrimEnd();
    }

    /// <summary>
    /// The main body of the command.
    /// </summary>
    /// <param name="arguments">The arguments passed to the command.</param>
    /// <param name="sender">The sender of the command.</param>
    /// <returns>The result of execution.</returns>
    protected abstract CommandResult Execute(ArraySegment<string> arguments, CommandSender sender);

}
