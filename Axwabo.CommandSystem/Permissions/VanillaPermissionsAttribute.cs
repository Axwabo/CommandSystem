using System;

namespace Axwabo.CommandSystem.Permissions;

/// <summary>
/// An attribute specifying that a given permission is required.
/// </summary>
/// <seealso cref="OneOfVanillaPermissionsAttribute"/>
[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public sealed class VanillaPermissionsAttribute : Attribute
{

    /// <summary>The required permission.</summary>
    public readonly PlayerPermissions Permission;

    /// <summary>
    /// Initializes a new instance of the <see cref="VanillaPermissionsAttribute"/> class.
    /// </summary>
    /// <param name="permission">The required permission.</param>
    public VanillaPermissionsAttribute(PlayerPermissions permission) => Permission = permission;

}
