using System;

namespace Axwabo.CommandSystem.Permissions;

[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public sealed class OneOfVanillaPermissionsAttribute : Attribute {

    public readonly PlayerPermissions[] Permissions;

    public OneOfVanillaPermissionsAttribute(params PlayerPermissions[] permissions) => Permissions = permissions;

}
