using System;
using Axwabo.CommandSystem.Permissions;
using Axwabo.CommandSystem.PropertyManager.Resolvers;

namespace Axwabo.CommandSystem.PropertyManager {

    public sealed class SimpleCommandPermissionCreator<T> : ICommandPermissionCreator<T> where T : Attribute {

        public static readonly ICommandPermissionCreator<T> Instance = new SimpleCommandPermissionCreator<T>();

        public IPermissionChecker CreatePermissionCheckerInstance(T attribute) => throw new NotImplementedException();

    }

}
