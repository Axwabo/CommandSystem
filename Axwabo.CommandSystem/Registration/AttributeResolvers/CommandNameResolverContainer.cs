using System;
using System.Reflection;
using Axwabo.CommandSystem.PropertyManager.Resolvers;

namespace Axwabo.CommandSystem.Registration.AttributeResolvers {

    internal readonly struct CommandNameResolverContainer : IResolverContainer<string> {

        private readonly MethodInfo _method;

        private readonly Type _parameter;

        private readonly ICommandNameResolver _instance;

        public CommandNameResolverContainer(MethodInfo method, Type parameter, ICommandNameResolver instance) {
            _parameter = parameter;
            _method = method;
            _instance = instance;
        }

        public bool Takes(Type type) => _parameter.IsAssignableFrom(type);

        public string Invoke(Attribute attribute) => (string) _method.Invoke(_instance, new object[] {attribute});

    }

}
