using System;
using System.Reflection;
using Axwabo.CommandSystem.PropertyManager.Resolvers;

namespace Axwabo.CommandSystem.Registration.AttributeResolvers;

internal readonly struct CommandUsageResolverContainer : IResolverContainer<string[]> {

    private readonly MethodInfo _method;

    private readonly Type _parameter;

    private readonly ICommandUsageResolver _instance;

    public CommandUsageResolverContainer(MethodInfo method, Type type, ICommandUsageResolver instance) {
        _method = method;
        _parameter = type;
        _instance = instance;
    }

    public bool Takes(Type type) => _parameter.IsAssignableFrom(type);

    public string[] Resolve(Attribute attribute) => (string[]) _method.Invoke(_instance, new object[] {attribute});

}
