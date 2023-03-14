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
        var hasPermission = sender.CheckPermission(Permission);
        return hasPermission ? true : "!You don't have permissions to use this feature. Required: " + Permission;
    }

}
