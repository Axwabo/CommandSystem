using System;
using Axwabo.CommandSystem.Attributes;
using Axwabo.CommandSystem.PropertyManager;

namespace Axwabo.CommandSystem.Registration {

    public static class RegistrationExtensions {

        public static bool HasFlagFast(this CommandTarget target, CommandTarget flag) => (target & flag) == flag;

        public static CommandRegistrationProcessor WithRegistrationAttributesFrom(this CommandRegistrationProcessor processor, Type type) {
            // TODO: handle attributes
            return processor;
        }

        public static CommandRegistrationProcessor WithNameResolver<T>(this CommandRegistrationProcessor processor, CommandNameResolver<T> nameResolver) where T : Attribute
            => WithNameResolver(processor, typeof(T), (CommandNameResolver<Attribute>) nameResolver);

        public static CommandRegistrationProcessor WithNameResolver(this CommandRegistrationProcessor processor, Type type, CommandNameResolver<Attribute> nameResolver) {
            processor.NameResolvers[type] = nameResolver;
            return processor;
        }

    }

}
