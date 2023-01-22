using System;
using Axwabo.CommandSystem.Exceptions;
using Axwabo.CommandSystem.Permissions;
using Axwabo.CommandSystem.PropertyManager;
using PluginAPI.Core;

namespace Axwabo.CommandSystem {

    public abstract class CommandBase {

        public virtual string Name => _name;

        public virtual string Description => _desc;

        public virtual string[] Aliases => _aliases;

        public virtual string[] Usage => _usage;

        protected string CombinedUsage => $"Usage:\n{string.Join("\n", Usage)}";

        protected virtual int MinArguments => _minArgs;

        protected virtual IPermissionChecker Permissions { get; }

        private readonly string _name;

        private readonly string _desc;

        private string[] _aliases;

        private string[] _usage;

        private int _minArgs;

        protected CommandBase() {
            if (string.IsNullOrEmpty(Name) && !CommandPropertyManager.TryResolveProperties(this, out _name, out _desc))
                throw new NameNotSetException($"Command name on type {GetType().FullName} is not set. Are you missing an attribute or custom name resolver?");
            Permissions ??= CommandPropertyManager.ResolvePermissionChecker(this);
        }

        public CommandResult ExecuteBase(ArraySegment<string> arguments, CommandSender sender) {
            var minArguments = MinArguments;
            if (arguments.Count < minArguments)
                return $"!Need at least {minArguments} argument{(minArguments == 1 ? "" : "s")}! {CombinedUsage}";
            var permissions = Permissions;
            var permissionCheck = sender.FullPermissions || permissions == null ? (CommandResult) true : permissions.CheckPermission(sender);
            return !permissionCheck ? permissionCheck : Execute(arguments, sender);
        }

        protected abstract CommandResult Execute(ArraySegment<string> arguments, CommandSender sender);

    }

}
