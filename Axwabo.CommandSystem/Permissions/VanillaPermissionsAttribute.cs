using System;

namespace Axwabo.CommandSystem.Permissions {

    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public sealed class VanillaPermissionsAttribute : Attribute {

        public readonly PlayerPermissions Permissions;

        public VanillaPermissionsAttribute(PlayerPermissions permissions) => Permissions = permissions;

    }

}
