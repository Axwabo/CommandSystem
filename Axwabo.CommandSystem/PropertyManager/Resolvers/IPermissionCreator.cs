using System;
using Axwabo.CommandSystem.Permissions;

namespace Axwabo.CommandSystem.PropertyManager.Resolvers {

    public interface IPermissionCreator<in TAttribute> where TAttribute : Attribute {

        IPermissionChecker CreatePermissionCheckerInstance(TAttribute attribute);

    }

}
