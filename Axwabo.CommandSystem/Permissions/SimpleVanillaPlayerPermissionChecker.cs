namespace Axwabo.CommandSystem.Permissions;

public sealed class SimpleVanillaPlayerPermissionChecker : IPermissionChecker {

    public readonly PlayerPermissions Permissions;

    public SimpleVanillaPlayerPermissionChecker(PlayerPermissions permissions) => Permissions = permissions;

    public CommandResult CheckPermission(CommandSender sender)
        => sender.FullPermissions
            ? true
            : new CommandResult(sender.CheckPermission(Permissions, out var response), response);

}
