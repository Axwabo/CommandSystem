using System;
using Axwabo.CommandSystem.Permissions;
using Axwabo.CommandSystem.Structs;

namespace Axwabo.CommandSystem {

    public abstract class CommandBase {

        protected virtual IPermissionChecker Permissions => null;

        public virtual string Name => NameInternal;

        public virtual string Description => DescriptionInternal;

        public virtual string[] Aliases => AliasesInternal;

        public virtual string[] Usage => UsageInternal;

        protected string CombinedUsage => $"Usage:\n{string.Join("\n", Usage)}";

        protected virtual int MinArguments => MinArgsInternal;

        internal string NameInternal;

        internal string DescriptionInternal;

        internal string[] AliasesInternal;

        internal string[] UsageInternal;

        internal int MinArgsInternal;

        protected CommandBase() {
            
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
