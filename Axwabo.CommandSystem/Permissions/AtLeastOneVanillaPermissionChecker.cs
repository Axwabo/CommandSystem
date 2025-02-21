using Axwabo.CommandSystem.Commands;

namespace Axwabo.CommandSystem.Permissions;

/// <summary>
/// A permission checker that ensures that at least one of the given permissions is sufficient.
/// </summary>
public sealed class AtLeastOneVanillaPermissionChecker : IPermissionChecker
{

    /// <summary>The list of permissions.</summary>
    public readonly PlayerPermissions[] Permissions;

    /// <summary>
    /// Initializes a new instance of the <see cref="AtLeastOneVanillaPermissionChecker"/> class.
    /// </summary>
    /// <param name="permissions">A list of permissions.</param>
    public AtLeastOneVanillaPermissionChecker(PlayerPermissions[] permissions) => Permissions = permissions;

    /// <inheritdoc />
    public CommandResult CheckPermission(CommandSender sender)
        => sender.FullPermissions || sender.CheckPermission(Permissions)
            ? true
            : "!You don't have permissions to use this feature. At least one of the following permissions is required: " + string.Join(", ", Permissions);

}
