using System;
using Axwabo.CommandSystem.PropertyManager.Resolvers;
using Axwabo.CommandSystem.Registration.AttributeResolvers;

namespace Axwabo.CommandSystem.Registration {

    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public sealed class UsesCedModPermissions : Attribute, IPermissionResolverMarker<UsesCedModPermissions> {

        public ICommandPermissionCreator<UsesCedModPermissions> CreateResolver() {
            throw new NotImplementedException();
        }

    }

}
