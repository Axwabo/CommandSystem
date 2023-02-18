using System;
using Axwabo.CommandSystem.Commands.Interfaces;
using Axwabo.CommandSystem.Exceptions;
using Axwabo.CommandSystem.Permissions;
using Axwabo.CommandSystem.PropertyManager;
using Axwabo.CommandSystem.Structs;
using RemoteAdmin;

namespace Axwabo.CommandSystem;

/// <summary>
/// A base class for creating commands.
/// </summary>
public abstract class CommandBase {

    private const string MustBePlayer = "You must be a player to use this command!";

    /// <summary>The name of the command.</summary>
    public virtual string Name => _name;

    /// <summary>A string that describes the command.</summary>
    public virtual string Description => _desc;

    /// <summary>An array of aliases for the command.</summary>
    public virtual string[] Aliases => _aliases;

    /// <summary>An array of usage examples for the command.</summary>
    public virtual string[] Usage => _usage;

    /// <summary>The minimum amount of arguments required to execute the command.</summary>
    protected virtual int MinArguments => _minArgs;

    /// <summary>Shows all possible usages if any with a "Usage(s):" prefix.</summary>
    /// <example>
    /// <code>
    /// Usage: mycommand [arg]
    /// </code>
    /// <code>
    /// Usages:
    /// mycommand [arg1]
    /// mycommand [arg2]
    /// </code>
    /// </example>
    public string CombinedUsage {
        get {
            var name = Name;
            var count = Usage is {Length: var l} ? l : 0;
            return count == 0 ? "" : $"{"Usage".Pluralize(count)}:{(count == 1 ? " " : "\n")}{name} {string.Join($"\n{name} ", Usage)}".TrimEnd();
        }
    }

    /// <summary>A permission checker for the command.</summary>
    protected virtual IPermissionChecker Permissions { get; }

    private readonly string _name;

    private readonly string _desc;

    private readonly string[] _aliases;

    private readonly string[] _usage;

    private readonly int _minArgs;

    private readonly bool _playerOnly;

    // ReSharper disable VirtualMemberCallInConstructor
    /// <summary>
    /// Creates a new instance of <see cref="CommandBase"/>.
    /// </summary>
    /// <exception cref="NameNotSetException">If the <see cref="Name">command name property</see> is not overriden and is not specified by attributes on the class.</exception>
    protected CommandBase() {
        if (string.IsNullOrWhiteSpace(Name) && !BaseCommandPropertyManager.TryResolveProperties(this, out _name, out _desc, out _aliases, out _usage, out _minArgs, out _playerOnly))
            throw new NameNotSetException($"Command name on type {GetType().FullName} is not set. Are you missing an attribute or custom name resolver?");
        Permissions ??= BaseCommandPropertyManager.ResolvePermissionChecker(this);
    }

    /// <summary>
    /// Executes the command with all checks.
    /// </summary>
    /// <param name="arguments">The arguments provided to the command.</param>
    /// <param name="sender">The sender of the command.</param>
    /// <returns>The result of execution.</returns>
    public CommandResult ExecuteBase(ArraySegment<string> arguments, CommandSender sender) {
        var playerOnly = CheckIfPlayerOnly(arguments, sender);
        if (playerOnly.HasValue)
            return playerOnly.Value;

        var permissions = Permissions;
        var permissionCheck = sender.FullPermissions || permissions == null ? (CommandResult) true : permissions.CheckPermission(sender);
        if (!permissionCheck)
            return permissionCheck;
        var pre = this is IPreExecutionFilter filter ? filter.OnBeforeExecuted(arguments, sender) : null;
        return pre ?? (
            arguments.Count < MinArguments
                ? OnNotEnoughArgumentsProvided(arguments, sender, MinArguments)
                : Execute(arguments, sender)
        );
    }

    private CommandResult? CheckIfPlayerOnly(ArraySegment<string> arguments, CommandSender sender)
        => sender is PlayerCommandSender
            ? null
            : this is IPlayerOnlyCommand playerOnly
                ? playerOnly.OnNotPlayer(arguments, sender) ?? CommandResult.Failed(MustBePlayer)
                : _playerOnly
                    ? CommandResult.Failed(MustBePlayer)
                    : null;

    /// <summary>
    /// Generates a response when not enough arguments are provided.
    /// </summary>
    /// <param name="arguments">The arguments provided to the command.</param>
    /// <param name="sender">The sender of the command.</param>
    /// <param name="required">The minimum amount of arguments required to execute the command.</param>
    /// <returns>The result of failure.</returns>
    protected CommandResult OnNotEnoughArgumentsProvided(ArraySegment<string> arguments, CommandSender sender, int required) =>
        this is INotEnoughArguments notEnough
            ? notEnough.OnNotEnoughArgumentsProvided(arguments, sender, required)
            : $"!You need to provide at least {"argument".PluralizeWithCount(required)}! {CombinedUsage}".TrimEnd();

    /// <summary>
    /// The main body of the command.
    /// </summary>
    /// <param name="arguments">The arguments provided to the command.</param>
    /// <param name="sender">The sender of the command.</param>
    /// <returns>The result of execution.</returns>
    protected abstract CommandResult Execute(ArraySegment<string> arguments, CommandSender sender);

}
