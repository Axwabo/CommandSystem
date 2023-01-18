using System;
using Axwabo.CommandSystem.Attributes;
using Axwabo.CommandSystem.Exceptions;
using Axwabo.CommandSystem.PropertyManager;

namespace Axwabo.CommandSystem.Registration {

    public static class RegistrationExtensions {

        private static Type GenericType(object o) => o.GetType().GenericTypeArguments[0];

        public static bool HasFlagFast(this CommandTarget target, CommandTarget flag) => (target & flag) == flag;

        public static CommandRegistrationProcessor WithRegistrationAttributesFrom(this CommandRegistrationProcessor processor, Type type) {
            // TODO: handle attributes
            return processor;
        }

        public static CommandRegistrationProcessor WithNameResolver<T>(this CommandRegistrationProcessor processor, ICommandNameResolver<T> nameResolver) where T : Attribute
            => WithNameResolver(processor, typeof(T), (ICommandNameResolver<Attribute>) nameResolver);

        public static CommandRegistrationProcessor WithNameResolver(this CommandRegistrationProcessor processor, Type type, ICommandNameResolver<Attribute> nameResolver) {
            if (!TryValidateArguments(type, nameResolver, out var exception, "name"))
                throw exception;
            processor.NameResolvers[type] = nameResolver;
            return processor;
        }

        public static CommandRegistrationProcessor WithDescriptionResolver<T>(this CommandRegistrationProcessor processor, ICommandDescriptionResolver<T> descriptionResolver) where T : Attribute
            => WithDescriptionResolver(processor, typeof(T), (ICommandDescriptionResolver<Attribute>) descriptionResolver);

        public static CommandRegistrationProcessor WithDescriptionResolver(this CommandRegistrationProcessor processor, Type type, ICommandDescriptionResolver<Attribute> descriptionResolver) {
            if (!TryValidateArguments(type, descriptionResolver, out var exception, "description"))
                throw exception;
            processor.DescriptionResolvers[type] = descriptionResolver;
            return processor;
        }

        private static bool TryValidateArguments(Type type, object resolver, out Exception exception, string resolverType) {
            if (type == null) {
                exception = new ArgumentNullException(nameof(type));
                return false;
            }

            if (resolver == null) {
                exception = new ArgumentNullException(nameof(resolver));
                return false;
            }

            if (!type.IsInstanceOfType(resolver)) {
                exception = new TypeMismatchException(GenericType(resolver), type, $"the {resolverType} resolver");
                return false;
            }

            exception = null;
            return true;
        }

    }

}
