#if EXILED
using Exiled.Permissions.Extensions;
#else
using NWAPIPermissionSystem;
#endif
using Axwabo.CommandSystem.Structs;

namespace Axwabo.CommandSystem.Permissions;

/// <summary>
/// Checks if the sender has a specific string permission. 
/// </summary>
/// <remarks>
/// The NWAPI version requires the <a href="https://github.com/CedModV2/NWAPIPermissionSystem">Permission System plugin</a> which is not needed if you're using the EXILED version.
/// </remarks>
public sealed class StringPermissionChecker : IPermissionChecker
{

    /// <summary>The required permission string.</summary>
    public readonly string Permission;

    /// <summary>
    /// Initializes a new instance of the <see cref="StringPermissionChecker"/> class.
    /// </summary>
    /// <param name="permission">The required permission.</param>
    public StringPermissionChecker(string permission) => Permission = permission;

    /// <inheritdoc />
    public CommandResult CheckPermission(CommandSender sender)
    {
        var hasPermission = Check(sender, Permission);
        return hasPermission ? true : "!You don't have permissions to use this feature. Required: " + Permission;
    }

    /// <summary>
    /// A static method to check if a sender has a specific permission.
    /// </summary>
    /// <param name="sender">The sender to check.</param>
    /// <param name="permission">The permission to check.</param>
    /// <returns>Whether the sender has the permission.</returns>
    public static bool Check(CommandSender sender, string permission) => sender.CheckPermission(permission);

}
