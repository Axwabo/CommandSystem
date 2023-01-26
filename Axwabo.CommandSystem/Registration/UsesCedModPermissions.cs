using System;
using Axwabo.CommandSystem.Permissions;
using Axwabo.CommandSystem.PropertyManager.Resolvers;

namespace Axwabo.CommandSystem.Registration;

[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public sealed class UsesCedModPermissions : Attribute, ICommandPermissionCreator<CedModPermissionsAttribute> {

    public IPermissionChecker CreatePermissionCheckerInstance(CedModPermissionsAttribute attribute) => new CedModPermissionChecker(attribute.Permission);

}
