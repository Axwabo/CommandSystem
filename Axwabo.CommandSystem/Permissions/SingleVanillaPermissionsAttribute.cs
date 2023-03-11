using System;

namespace Axwabo.CommandSystem.Permissions;

[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public sealed class SingleVanillaPermissionsAttribute : Attribute {

    public readonly PlayerPermissions Permission;

    public SingleVanillaPermissionsAttribute(PlayerPermissions permission) => Permission = permission;

}
