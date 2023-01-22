using System;

namespace Axwabo.CommandSystem.PropertyManager.Resolvers {

    public interface ICommandDescriptionResolver {

    }

    public interface ICommandDescriptionResolver<in TAttribute> : ICommandDescriptionResolver where TAttribute : Attribute {

        string ResolveDescription(TAttribute attribute);

    }

}
