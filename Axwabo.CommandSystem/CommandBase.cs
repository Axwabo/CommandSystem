using Axwabo.CommandSystem.Commands.Interfaces;
using Axwabo.CommandSystem.Exceptions;
using Axwabo.CommandSystem.Permissions;
using Axwabo.CommandSystem.PropertyManager;

namespace Axwabo.CommandSystem;

/// <summary>
/// Base class for creating commands.
/// </summary>
public abstract class CommandBase
{

    /// <summary>The message to display when only a player is allowed to use the command but the sender is not a player.</summary>
    public const string MustBePlayer = "You must be a player to use this command!";

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

    /// <summary>Whether response rich text tag chevrons ( &lt;&gt; ) should be replaced with a similar character. Defaults to the option specified in the config.</summary>
    public virtual bool SanitizeResponse => Plugin.Instance.Config.SanitizeResponses;

    private readonly string _name;

    private readonly string _desc;

    private readonly string[] _aliases;

    private readonly string[] _usage;

    private readonly int _minArgs;

    private readonly bool _playerOnly;

    // ReSharper disable VirtualMemberCallInConstructor
    /// <summary>
    /// Creates a new <see cref="CommandBase"/> instance.
    /// </summary>
    /// <exception cref="InvalidNameException">If the <see cref="Name">command name property</see> is not overriden and is not specified by attributes on the class.</exception>
    protected CommandBase()
    {
        var resolved = BaseCommandPropertyManager.TryResolveProperties(this, out _name, out _desc, out _aliases, out _usage, out _minArgs, out _playerOnly);
        if (string.IsNullOrWhiteSpace(Name) && !resolved)
            throw new InvalidNameException($"Command name on type {GetType().FullName} is not set. Are you missing an attribute or custom name resolver?");
        Permissions ??= BaseCommandPropertyManager.ResolvePermissionChecker(this);
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
                : _playerOnly
                    ? CommandResult.Failed(MustBePlayer)
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
