using System;
using Axwabo.CommandSystem.Commands.Interfaces;
using Axwabo.CommandSystem.Exceptions;
using Axwabo.CommandSystem.Permissions;
using Axwabo.CommandSystem.PropertyManager;
using Axwabo.CommandSystem.Structs;

namespace Axwabo.CommandSystem;

public abstract class CommandBase {

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

    // ReSharper disable VirtualMemberCallInConstructor
    protected CommandBase() {
        if (string.IsNullOrWhiteSpace(Name) && !BaseCommandPropertyManager.TryResolveProperties(this, out _name, out _desc, out _aliases, out _usage, out _minArgs))
            throw new NameNotSetException($"Command name on type {GetType().FullName} is not set. Are you missing an attribute or custom name resolver?");
        Permissions ??= BaseCommandPropertyManager.ResolvePermissionChecker(this);
    }

    public CommandResult ExecuteBase(ArraySegment<string> arguments, CommandSender sender) {
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

    protected abstract CommandResult Execute(ArraySegment<string> arguments, CommandSender sender);

    protected virtual CommandResult OnNotEnoughArgumentsProvided(ArraySegment<string> arguments, CommandSender sender) {
        var minArguments = MinArguments;
        return $"!Need at least {"argument".PluralizeWithCount(minArguments)}! {CombinedUsage}";
    }

}
