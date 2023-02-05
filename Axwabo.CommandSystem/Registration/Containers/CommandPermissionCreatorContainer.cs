using System;
using System.Reflection;
using Axwabo.CommandSystem.Permissions;
using Axwabo.CommandSystem.PropertyManager.Resolvers;

namespace Axwabo.CommandSystem.Registration.Containers;

internal readonly struct CommandPermissionCreatorContainer : IResolverContainer<IPermissionChecker> {

    private readonly MethodInfo _method;

    private readonly Type _parameter;

    private readonly ICommandPermissionCreator _instance;

    public CommandPermissionCreatorContainer(MethodInfo method, Type type, ICommandPermissionCreator instance) {
        _method = method;
        _parameter = type;
        _instance = instance;
    }

    public bool Takes(Type type) => _parameter.IsAssignableFrom(type);

    public IPermissionChecker Resolve(Attribute attribute) => (IPermissionChecker) _method.Invoke(_instance, new object[] {attribute});

}
