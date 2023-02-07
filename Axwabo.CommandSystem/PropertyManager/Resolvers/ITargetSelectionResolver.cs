using System;
using Axwabo.CommandSystem.Commands.MessageOverrides;

namespace Axwabo.CommandSystem.PropertyManager.Resolvers;

public interface ITargetSelectionResolver {

}

public interface ITargetSelectionResolver<in TAttribute> : ITargetSelectionResolver where TAttribute : Attribute {

    ITargetSelectionManager ResolveGenerator(TAttribute attribute);

}
