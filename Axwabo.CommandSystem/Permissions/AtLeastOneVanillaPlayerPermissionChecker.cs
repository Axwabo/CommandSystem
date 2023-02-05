using Axwabo.CommandSystem.Structs;

namespace Axwabo.CommandSystem.Permissions;

public sealed class AtLeastOneVanillaPlayerPermissionChecker : IPermissionChecker {

    public readonly PlayerPermissions[] Permissions;

    public AtLeastOneVanillaPlayerPermissionChecker(PlayerPermissions[] permissions) => Permissions = permissions;

    public CommandResult CheckPermission(CommandSender sender)
        => sender.FullPermissions
            ? true
            : new CommandResult(sender.CheckPermission(Permissions, out var response), response);

}
