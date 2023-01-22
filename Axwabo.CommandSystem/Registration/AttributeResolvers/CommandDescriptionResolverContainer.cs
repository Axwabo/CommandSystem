using System;
using System.Reflection;
using Axwabo.CommandSystem.PropertyManager.Resolvers;

namespace Axwabo.CommandSystem.Registration.AttributeResolvers {

    internal readonly struct CommandDescriptionResolverContainer : IResolverContainer<string> {

        private readonly MethodInfo _method;

        private readonly Type _parameter;

        private readonly ICommandDescriptionResolver _instance;

        public CommandDescriptionResolverContainer(MethodInfo method, Type type, ICommandDescriptionResolver instance) {
            _method = method;
            _parameter = type;
            _instance = instance;
        }

        public bool Takes(Type type) => _parameter.IsAssignableFrom(type);

        public string Invoke(Attribute attribute) => (string) _method.Invoke(_instance, new object[] {attribute});

    }

}
