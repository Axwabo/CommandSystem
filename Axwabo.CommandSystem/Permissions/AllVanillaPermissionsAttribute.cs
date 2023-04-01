using System;

namespace Axwabo.CommandSystem.Permissions;

/// <summary>
/// An attribute specifying that all of the given permissions are required.
/// </summary>
/// <seealso cref="VanillaPermissionsAttribute"/>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
public sealed class AllVanillaPermissionsAttribute : Attribute
{

    /// <summary>The list of permissions.</summary>
    public readonly PlayerPermissions[] Permissions;

    /// <summary>
    /// Initializes a new instance of the <see cref="AllVanillaPermissionsAttribute"/> class.
    /// </summary>
    /// <param name="permissions">A list of permissions.</param>
    public AllVanillaPermissionsAttribute(params PlayerPermissions[] permissions) => Permissions = permissions;

}
