using System;
using System.Linq;
using System.Reflection;
using Axwabo.CommandSystem.Attributes;
using Axwabo.CommandSystem.Exceptions;
using Axwabo.CommandSystem.PropertyManager.Resolvers;
using Axwabo.CommandSystem.Registration.AttributeResolvers;

namespace Axwabo.CommandSystem.Registration {

    public static class RegistrationExtensions {

        private static Type GenericType(object o) {
            var type = o as Type ?? o.GetType();
            var args = type.GenericTypeArguments;
            return args is not {Length: not 0} ? null : args[0];
        }

        private static Type GetInterface(Type targetType, Type genericInterfaceType) => targetType.GetInterfaces().FirstOrDefault(e => e.IsGenericType && e.GetGenericTypeDefinition() == genericInterfaceType);

        private static Type ImplementedGenericType(object resolver, Type interfaceType)
            => GenericType(
                GetInterface(resolver.GetType(), interfaceType)
                ?? throw new AttributeResolverException($"Interface does not implement the generic {interfaceType.Name}")
            );

        public static bool HasFlagFast(this CommandTarget target, CommandTarget flag) => (target & flag) == flag;

        public static CommandRegistrationProcessor WithRegistrationAttributesFrom(this CommandRegistrationProcessor processor, Type type) {
            if (type == null)
                throw new ArgumentNullException(nameof(type));
            foreach (var attribute in type.GetCustomAttributes())
                AddRegistrationAttribute(processor, attribute);

            return processor;
        }

        private static void AddRegistrationAttribute(CommandRegistrationProcessor processor, Attribute attribute) {
            if (attribute is ICommandNameResolver nameResolver)
                processor.WithNameResolver(ImplementedGenericType(nameResolver, typeof(ICommandNameResolver<>)), nameResolver);
            if (attribute is ICommandDescriptionResolver descriptionResolver)
                processor.WithDescriptionResolver(ImplementedGenericType(descriptionResolver, typeof(ICommandDescriptionResolver<>)), descriptionResolver);
            if (attribute is ICommandPermissionCreator permissionCreator)
                processor.WithPermissionCreator(ImplementedGenericType(permissionCreator, typeof(ICommandPermissionCreator<>)), permissionCreator);
        }

        public static CommandRegistrationProcessor WithNameResolver<T>(this CommandRegistrationProcessor processor, ICommandNameResolver<T> nameResolver) where T : Attribute
            => WithNameResolver(processor, typeof(T), nameResolver);

        public static CommandRegistrationProcessor WithNameResolver(this CommandRegistrationProcessor processor, Type type, ICommandNameResolver nameResolver) {
            var method = GetGenericResolverMethod(type, typeof(ICommandNameResolver<>), nameResolver, out var param);
            processor.NameResolvers.Add(new CommandNameResolverContainer(method, param, nameResolver));
            return processor;
        }

        public static CommandRegistrationProcessor WithDescriptionResolver<T>(this CommandRegistrationProcessor processor, ICommandDescriptionResolver<T> descriptionResolver) where T : Attribute
            => WithDescriptionResolver(processor, typeof(T), descriptionResolver);

        public static CommandRegistrationProcessor WithDescriptionResolver(this CommandRegistrationProcessor processor, Type type, ICommandDescriptionResolver descriptionResolver) {
            var method = GetGenericResolverMethod(type, typeof(ICommandDescriptionResolver<>), descriptionResolver, out var param);
            processor.DescriptionResolvers.Add(new CommandDescriptionResolverContainer(method, param, descriptionResolver));
            return processor;
        }

        public static CommandRegistrationProcessor WithPermissionCreator<T>(this CommandRegistrationProcessor processor, ICommandPermissionCreator<T> permissionCreator) where T : Attribute
            => WithPermissionCreator(processor, typeof(T), permissionCreator);

        public static CommandRegistrationProcessor WithPermissionCreator(this CommandRegistrationProcessor processor, Type type, ICommandPermissionCreator permissionCreator) {
            var method = GetGenericResolverMethod(type, typeof(ICommandPermissionCreator<>), permissionCreator, out var param);
            processor.PermissionCreators.Add(new CommandPermissionCreatorContainer(method, param, permissionCreator));
            return processor;
        }

        private static MethodInfo GetGenericResolverMethod(Type parameterType, Type interfaceType, object resolver, out Type param) {
            if (parameterType == null)
                throw new ArgumentNullException(nameof(parameterType));
            if (interfaceType == null)
                throw new ArgumentNullException(nameof(interfaceType));
            if (resolver == null)
                throw new ArgumentNullException(nameof(resolver));
            var implementedInterface = GetInterface(resolver.GetType(), interfaceType);
            if (implementedInterface == null)
                throw new AttributeResolverException($"Expected an implementation of {interfaceType.Name} with generic type {parameterType.FullName}, got: {resolver.GetType().FullName}");
            var method = implementedInterface.GetMethods()[0];
            var parameters = method.GetParameters();
            if (parameters is not {Length: 1})
                throw new AttributeResolverException($"Expected an implementation {interfaceType.Name} with a single parameter of type {parameterType.FullName}");
            param = parameters[0].ParameterType;
            if (!param.IsAssignableFrom(parameterType))
                throw new TypeMismatchException($"Expected an implementation {interfaceType.Name} with a single parameter of type {parameterType.FullName}, got: {param.FullName}");
            var returnType = method.ReturnType;
            var expectedReturnType = interfaceType.GetMethods()[0].ReturnType;
            if (!returnType.IsAssignableFrom(expectedReturnType))
                throw new TypeMismatchException($"Expected an implementation {interfaceType.Name} with a return type of {expectedReturnType.FullName}, got: {returnType.FullName}");
            return method;
        }

    }

}
