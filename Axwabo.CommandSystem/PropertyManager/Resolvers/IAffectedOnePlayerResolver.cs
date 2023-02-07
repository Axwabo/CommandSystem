using System;
using Axwabo.CommandSystem.Commands.MessageOverrides;

namespace Axwabo.CommandSystem.PropertyManager.Resolvers;

public interface IAffectedOnePlayerResolver {

}

public interface IAffectedOnePlayerResolver<in TAttribute> : IAffectedOnePlayerResolver where TAttribute : Attribute {

    IAffectedOnePlayerMessageGenerator ResolveGenerator(TAttribute attribute);

}
