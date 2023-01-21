using System;

namespace Axwabo.CommandSystem.PropertyManager.Resolvers {

    public interface ICommandNameResolver<in TAttribute> where TAttribute : Attribute {

        string ResolveName(TAttribute attribute);

    }

}
