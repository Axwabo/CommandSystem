using System;
using System.Reflection;
using Axwabo.CommandSystem.Commands.MessageOverrides;
using Axwabo.CommandSystem.PropertyManager.Resolvers;

namespace Axwabo.CommandSystem.Registration.Containers;

internal readonly struct TargetingMultipleMessageResolverContainer : IResolverContainer<IAffectedMultiplePlayersMessageGenerator> {

    private readonly MethodInfo _method;

    private readonly Type _parameter;

    private readonly IAffectedMultiplePlayersResolver _instance;

    public TargetingMultipleMessageResolverContainer(MethodInfo method, Type type, IAffectedMultiplePlayersResolver instance) {
        _method = method;
        _parameter = type;
        _instance = instance;
    }

    public bool Takes(Type type) => _parameter.IsAssignableFrom(type);

    public IAffectedMultiplePlayersMessageGenerator Resolve(Attribute attribute) => (IAffectedMultiplePlayersMessageGenerator) _method.Invoke(_instance, new object[] {attribute});

}
