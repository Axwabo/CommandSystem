using System;

namespace Axwabo.CommandSystem.PropertyManager {

    public interface ICommandNameResolver<in TAttribute> where TAttribute : Attribute {

        string ResolveName(TAttribute attribute);

    }

}
