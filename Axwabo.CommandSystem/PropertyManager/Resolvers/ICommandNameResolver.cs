using System;

namespace Axwabo.CommandSystem.PropertyManager.Resolvers;

public interface ICommandNameResolver {

}

public interface ICommandNameResolver<in TAttribute> : ICommandNameResolver where TAttribute : Attribute {

    string ResolveName(TAttribute attribute);

}
