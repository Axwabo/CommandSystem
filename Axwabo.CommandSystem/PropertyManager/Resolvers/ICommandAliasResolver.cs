using System;

namespace Axwabo.CommandSystem.PropertyManager.Resolvers;

public interface ICommandAliasResolver {

}

public interface ICommandAliasResolver<in TAttribute> : ICommandAliasResolver where TAttribute : Attribute {

    string[] ResolveAliases(TAttribute attribute);

}
