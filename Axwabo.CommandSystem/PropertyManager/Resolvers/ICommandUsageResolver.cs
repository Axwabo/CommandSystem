using System;

namespace Axwabo.CommandSystem.PropertyManager.Resolvers;

public interface ICommandUsageResolver {

}

public interface ICommandUsageResolver<in TAttribute> : ICommandUsageResolver where TAttribute : Attribute {

    string[] ResolveUsage(TAttribute attribute);

}
