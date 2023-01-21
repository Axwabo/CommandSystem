using System;

namespace Axwabo.CommandSystem.PropertyManager.Resolvers {

    public interface ICommandDescriptionResolver<in TAttribute> where TAttribute : Attribute {

        string ResolveDescription(TAttribute attribute);

    }

}
