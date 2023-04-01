namespace Axwabo.CommandSystem.Permissions;

/// <summary>
/// A simple permission checker based on a single <see cref="PlayerPermissions"/> value.
/// </summary>
public sealed class SimpleVanillaPlayerPermissionChecker : IPermissionChecker
{

    /// <summary>The required permission.</summary>
    public readonly PlayerPermissions Permissions;

    /// <summary>
    /// Initializes a new instance of the <see cref="SimpleVanillaPlayerPermissionChecker"/> class.
    /// </summary>
    /// <param name="permissions">The required permission.</param>
    public SimpleVanillaPlayerPermissionChecker(PlayerPermissions permissions) => Permissions = permissions;

    /// <inheritdoc />
    public CommandResult CheckPermission(CommandSender sender)
        => sender.FullPermissions || sender.CheckPermission(Permissions)
            ? true
            : $"!You don't have permissions to use this feature. Required: {Permissions}";

}
