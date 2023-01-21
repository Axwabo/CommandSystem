using System;
using Axwabo.CommandSystem.Attributes;
using Axwabo.CommandSystem.Exceptions;
using Axwabo.CommandSystem.PropertyManager.Resolvers;

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
            ValidateArguments(type, nameResolver, "name resolver");
            processor.NameResolvers[type] = nameResolver;
            return processor;
        }

        public static CommandRegistrationProcessor WithDescriptionResolver<T>(this CommandRegistrationProcessor processor, ICommandDescriptionResolver<T> descriptionResolver) where T : Attribute
            => WithDescriptionResolver(processor, typeof(T), (ICommandDescriptionResolver<Attribute>) descriptionResolver);

        public static CommandRegistrationProcessor WithDescriptionResolver(this CommandRegistrationProcessor processor, Type type, ICommandDescriptionResolver<Attribute> descriptionResolver) {
            ValidateArguments(type, descriptionResolver, "description resolver");
            processor.DescriptionResolvers[type] = descriptionResolver;
            return processor;
        }
        
        public static CommandRegistrationProcessor WithPermissionCreator<T>(this CommandRegistrationProcessor processor, ICommandPermissionCreator<T> permissionCreator) where T : Attribute
            => WithPermissionCreator(processor, typeof(T), (ICommandPermissionCreator<Attribute>) permissionCreator);
        
        public static CommandRegistrationProcessor WithPermissionCreator(this CommandRegistrationProcessor processor, Type type, ICommandPermissionCreator<Attribute> permissionCreator) {
            ValidateArguments(type, permissionCreator, "permission creator");
            processor.PermissionCreators[type] = permissionCreator;
            return processor;
        }

        private static void ValidateArguments(Type type, object o, string typeName) {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            if (o == null)
                throw new ArgumentNullException(nameof(o));

            if (!type.IsInstanceOfType(o))
                throw new TypeMismatchException(GenericType(o), type, $"the {typeName}");
        }

    }

}
