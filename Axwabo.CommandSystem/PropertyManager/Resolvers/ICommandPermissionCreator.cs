using System;
using Axwabo.CommandSystem.Permissions;

namespace Axwabo.CommandSystem.PropertyManager.Resolvers {

    public interface ICommandPermissionCreator<in TAttribute> where TAttribute : Attribute {

        IPermissionChecker CreatePermissionCheckerInstance(TAttribute attribute);

    }

}
