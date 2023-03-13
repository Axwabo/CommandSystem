using Axwabo.CommandSystem.Structs;

namespace Axwabo.CommandSystem.Permissions;

/// <summary>
/// A permission checker that ensures that at least one of the given permissions is sufficient.
/// </summary>
public sealed class AtLeastOneVanillaPlayerPermissionChecker : IPermissionChecker
{

    /// <summary>The list of permissions.</summary>
    public readonly PlayerPermissions[] Permissions;

    /// <summary>
    /// Initializes a new instance of the <see cref="AtLeastOneVanillaPlayerPermissionChecker"/> class.
    /// </summary>
    /// <param name="permissions">A list of permissions.</param>
    public AtLeastOneVanillaPlayerPermissionChecker(PlayerPermissions[] permissions) => Permissions = permissions;

    /// <inheritdoc />
    public CommandResult CheckPermission(CommandSender sender)
        => sender.FullPermissions
            ? true
            : new CommandResult(sender.CheckPermission(Permissions, out var response), response);

}
