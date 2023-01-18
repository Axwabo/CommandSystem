using System;
using Axwabo.CommandSystem.Exceptions;
using Axwabo.CommandSystem.Permissions;
using Axwabo.CommandSystem.PropertyManager;
using Axwabo.CommandSystem.Structs;

namespace Axwabo.CommandSystem {

    public abstract class CommandBase {

        protected virtual IPermissionChecker Permissions => null;

        public virtual string Name => _name;

        public virtual string Description => _desc;

        public virtual string[] Aliases => _aliases;

        public virtual string[] Usage => _usage;

        protected string CombinedUsage => $"Usage:\n{string.Join("\n", Usage)}";

        protected virtual int MinArguments => _minArgs;

        private readonly string _name;

        private readonly string _desc;

        private string[] _aliases;

        private string[] _usage;

        private int _minArgs;

        protected CommandBase() {
            if (string.IsNullOrEmpty(Name) && !CommandPropertiesManager.TryResolveProperties(this, out _name, out _desc))
                throw new NameNotSetException($"Command name on type {GetType().FullName} is not set. Are you missing an attribute or custom name resolver?");
        }

        public CommandResult ExecuteBase(ArraySegment<string> arguments, CommandSender sender) {
            var minArguments = MinArguments;
            if (arguments.Count < minArguments)
                return $"!Need at least {minArguments} arguments! {CombinedUsage}";
            var permissions = Permissions;
            var permissionCheck = !sender.FullPermissions && permissions != null ? permissions.CheckPermission(sender) : (CommandResult) true;
            return !permissionCheck ? permissionCheck : Execute(arguments, sender);
        }

        protected abstract CommandResult Execute(ArraySegment<string> arguments, CommandSender sender);

    }

}
