using System;

namespace Axwabo.CommandSystem.Permissions {

    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public sealed class CedModPermissionsAttribute : Attribute {

        public readonly string Permission;

        public CedModPermissionsAttribute(string permission) => Permission = permission;

    }

}
