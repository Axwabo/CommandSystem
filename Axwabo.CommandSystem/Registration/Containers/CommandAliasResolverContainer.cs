using System;
using System.Reflection;
using Axwabo.CommandSystem.PropertyManager.Resolvers;

namespace Axwabo.CommandSystem.Registration.Containers;

internal readonly struct CommandAliasResolverContainer : IResolverContainer<string[]> {

    private readonly MethodInfo _method;

    private readonly Type _parameter;

    private readonly ICommandAliasResolver _instance;

    public CommandAliasResolverContainer(MethodInfo method, Type type, ICommandAliasResolver instance) {
        _method = method;
        _parameter = type;
        _instance = instance;
    }

    public bool Takes(Type type) => _parameter.IsAssignableFrom(type);

    public string[] Resolve(Attribute attribute) => (string[]) _method.Invoke(_instance, new object[] {attribute});

}
