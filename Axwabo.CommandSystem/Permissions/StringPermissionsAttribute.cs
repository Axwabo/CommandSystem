namespace Axwabo.CommandSystem.Permissions;

/// <summary>
/// A permission attribute that uses a string as the permission.
/// </summary>
/// <remarks>
/// The NWAPI version requires the <a href="https://github.com/CedModV2/NWAPIPermissionSystem">Permission System plugin</a> which is not needed if you're using the EXILED version.
/// </remarks>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
public sealed class StringPermissionsAttribute : Attribute
{

    /// <summary>The required permission string.</summary>
    public readonly string Permission;

    /// <summary>
    /// Initializes a new instance of the <see cref="StringPermissionsAttribute"/> class.
    /// </summary>
    /// <param name="permission">The required permission.</param>
    public StringPermissionsAttribute(string permission) => Permission = permission;

}
