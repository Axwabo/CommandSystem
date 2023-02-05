using System;
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

    protected string CombinedUsage => Usage is not {Length: not 0} ? "" : $"Usage:\n{string.Join("\n", Usage)}";

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
        Permissions ??= this as IPermissionChecker ?? BaseCommandPropertyManager.ResolvePermissionChecker(this);
    }

    public CommandResult ExecuteBase(ArraySegment<string> arguments, CommandSender sender) {
        if (arguments.Count < MinArguments)
            return OnNotEnoughArgumentsProvided(arguments);
        var permissions = Permissions;
        var permissionCheck = sender.FullPermissions || permissions == null ? (CommandResult) true : permissions.CheckPermission(sender);
        return !permissionCheck ? permissionCheck : Execute(arguments, sender);
    }

    protected abstract CommandResult Execute(ArraySegment<string> arguments, CommandSender sender);

    protected virtual string OnNotEnoughArgumentsProvided(ArraySegment<string> arguments) {
        var minArguments = MinArguments;
        return $"Need at least {"argument".Pluralize(minArguments)}! {CombinedUsage}";
    }

}
