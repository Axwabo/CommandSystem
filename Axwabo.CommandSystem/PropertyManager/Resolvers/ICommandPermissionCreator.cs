using System;
using Axwabo.CommandSystem.Permissions;

namespace Axwabo.CommandSystem.PropertyManager.Resolvers;

public interface ICommandPermissionCreator {

}

public interface ICommandPermissionCreator<in TAttribute> : ICommandPermissionCreator where TAttribute : Attribute {

    IPermissionChecker CreatePermissionCheckerInstance(TAttribute attribute);

}
