using System;
using Axwabo.CommandSystem.Commands.MessageOverrides;

namespace Axwabo.CommandSystem.PropertyManager.Resolvers;

public interface IAffectedAllPlayersResolver {

}

public interface IAffectedAllPlayersResolver<in TAttribute> : IAffectedAllPlayersResolver where TAttribute : Attribute {

    IAffectedAllPlayersGenerator ResolveGenerator(TAttribute attribute);

}
