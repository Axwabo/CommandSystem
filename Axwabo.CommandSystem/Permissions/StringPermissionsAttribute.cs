using System;

namespace Axwabo.CommandSystem.Permissions;

[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public sealed class StringPermissionsAttribute : Attribute {

    public readonly string Permission;

    public StringPermissionsAttribute(string permission) => Permission = permission;

}
