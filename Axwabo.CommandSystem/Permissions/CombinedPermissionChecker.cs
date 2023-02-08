using Axwabo.CommandSystem.Structs;

namespace Axwabo.CommandSystem.Permissions;

public sealed class CombinedPermissionChecker : IPermissionChecker {

    public IPermissionChecker[] Instances { get; }

    public CombinedPermissionChecker(params IPermissionChecker[] instances) => Instances = instances;

    public CommandResult CheckPermission(CommandSender sender) {
        foreach (var instance in Instances) {
            var result = instance.CheckPermission(sender);
            if (!result)
                return result;
        }

        return true;
    }

}
