using Axwabo.CommandSystem.Structs;
using NWAPIPermissionSystem;

namespace Axwabo.CommandSystem.Permissions;

public sealed class StringPermissionChecker : IPermissionChecker {

    public readonly string Permission;

    public StringPermissionChecker(string permission) => Permission = permission;

    public CommandResult CheckPermission(CommandSender sender) {
        var hasPermission = sender.CheckPermission(Permission);
        return hasPermission ? true : "!You don't have permission to use this command. Required: " + Permission;
    }

}
