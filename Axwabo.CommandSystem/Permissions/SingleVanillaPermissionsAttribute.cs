using System;

namespace Axwabo.CommandSystem.Permissions;

[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public sealed class SingleVanillaPermissionsAttribute : Attribute {

    public readonly PlayerPermissions Permissions;

    public SingleVanillaPermissionsAttribute(PlayerPermissions permissions) => Permissions = permissions;

}
