using System;
using Axwabo.CommandSystem.PropertyManager.Resolvers;

namespace Axwabo.CommandSystem.Registration.AttributeResolvers {

    public interface IPermissionResolverMarker<in TAttribute> where TAttribute : Attribute {

        ICommandPermissionCreator<TAttribute> CreateResolver();

    }

}
