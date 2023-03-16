using System;

namespace Axwabo.CommandSystem.Permissions;

/// <summary>
/// An attribute specifying that at least one of the given permissions is required.
/// </summary>
/// <seealso cref="VanillaPermissionsAttribute"/>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
public sealed class OneOfVanillaPermissionsAttribute : Attribute
{

    /// <summary>The list of permissions.</summary>
    public readonly PlayerPermissions[] Permissions;

    /// <summary>
    /// Initializes a new instance of the <see cref="OneOfVanillaPermissionsAttribute"/> class.
    /// </summary>
    /// <param name="permissions">A list of permissions.</param>
    public OneOfVanillaPermissionsAttribute(params PlayerPermissions[] permissions) => Permissions = permissions;

}
