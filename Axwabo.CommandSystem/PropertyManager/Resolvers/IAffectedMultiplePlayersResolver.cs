using System;
using Axwabo.CommandSystem.Commands.MessageOverrides;

namespace Axwabo.CommandSystem.PropertyManager.Resolvers;

public interface IAffectedMultiplePlayersResolver {

}

public interface IAffectedMultiplePlayersResolver<in TAttribute> : IAffectedMultiplePlayersResolver where TAttribute : Attribute {

    IAffectedMultiplePlayersMessageGenerator ResolveGenerator(TAttribute attribute);

}
