using System;
using Axwabo.CommandSystem.Commands.Interfaces;
using Axwabo.CommandSystem.Exceptions;
using Axwabo.CommandSystem.Permissions;
using Axwabo.CommandSystem.PropertyManager;
using Axwabo.CommandSystem.Structs;
using RemoteAdmin;

namespace Axwabo.CommandSystem;

public abstract class CommandBase {

    private const string MustBePlayer = "You must be a player to use this command!";

    public virtual string Name => _name;

    public virtual string Description => _desc;

    public virtual string[] Aliases => _aliases;

    public virtual string[] Usage => _usage;

    protected virtual int MinArguments => _minArgs;

    public string CombinedUsage {
        get {
            var name = Name;
            var count = Usage is {Length: var l} ? l : 0;
            return count == 0 ? "" : $"{"Usage".Pluralize(count)}:{(count == 1 ? " " : "\n")}{name} {string.Join($"\n{name} ", Usage)}".TrimEnd();
        }
    }

    protected virtual IPermissionChecker Permissions { get; }

    private readonly string _name;

    private readonly string _desc;

    private readonly string[] _aliases;

    private readonly string[] _usage;

    private readonly int _minArgs;

    private readonly bool _playerOnly;

    // ReSharper disable VirtualMemberCallInConstructor
    protected CommandBase() {
        if (string.IsNullOrWhiteSpace(Name) && !BaseCommandPropertyManager.TryResolveProperties(this, out _name, out _desc, out _aliases, out _usage, out _minArgs, out _playerOnly))
            throw new NameNotSetException($"Command name on type {GetType().FullName} is not set. Are you missing an attribute or custom name resolver?");
        Permissions ??= BaseCommandPropertyManager.ResolvePermissionChecker(this);
    }

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
                ? OnNotEnoughArgumentsProvided(arguments, sender)
                : Execute(arguments, sender)
        );
    }

    private CommandResult? CheckIfPlayerOnly(ArraySegment<string> arguments, CommandSender sender) {
        if (sender is PlayerCommandSender)
            return null;

        return this is IPlayerOnlyCommand playerOnly
            ? playerOnly.OnNotPlayer(arguments, sender) ?? CommandResult.Failed(MustBePlayer)
            : _playerOnly
                ? CommandResult.Failed(MustBePlayer)
                : null;
    }

    private CommandResult OnNotEnoughArgumentsProvided(ArraySegment<string> arguments, CommandSender sender) =>
        this is INotEnoughArguments notEnough
            ? notEnough.OnNotEnoughArgumentsProvided(arguments, sender)
            : $"!You need to provide at least {"argument".PluralizeWithCount(MinArguments)}! {CombinedUsage}";

    protected abstract CommandResult Execute(ArraySegment<string> arguments, CommandSender sender);

}
