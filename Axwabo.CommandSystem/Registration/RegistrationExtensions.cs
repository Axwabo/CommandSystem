using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Axwabo.CommandSystem.Attributes;
using Axwabo.CommandSystem.Exceptions;
using Axwabo.CommandSystem.PropertyManager.Resolvers;

namespace Axwabo.CommandSystem.Registration;

public static class RegistrationExtensions {

    public static bool HasFlagFast(this CommandHandlerType target, CommandHandlerType flag) => (target & flag) == flag;

    #region Helpers

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

    #endregion

    #region Auto-Register

    public static CommandRegistrationProcessor WithRegistrationAttributesFrom(this CommandRegistrationProcessor processor, Type type) {
        if (type == null)
            throw new ArgumentNullException(nameof(type));
        foreach (var attribute in type.GetCustomAttributes())
            AddRegistrationAttribute(processor, attribute);
        return processor;
    }

    private static void AddRegistrationAttribute(CommandRegistrationProcessor processor, Attribute attribute) {
        processor.AddRegistrationAttribute<ICommandNameResolver>(attribute, typeof(ICommandNameResolver<>), WithNameResolver);
        processor.AddRegistrationAttribute<ICommandDescriptionResolver>(attribute, typeof(ICommandDescriptionResolver<>), WithDescriptionResolver);
        processor.AddRegistrationAttribute<ICommandAliasResolver>(attribute, typeof(ICommandAliasResolver<>), WithAliasResolver);
        processor.AddRegistrationAttribute<ICommandUsageResolver>(attribute, typeof(ICommandUsageResolver<>), WithUsageResolver);
        processor.AddRegistrationAttribute<ICommandPermissionCreator>(attribute, typeof(ICommandPermissionCreator<>), WithPermissionCreator);
    }

    private static void AddRegistrationAttribute<TBaseResolver>(this CommandRegistrationProcessor processor, Attribute attribute, Type genericType, Func<CommandRegistrationProcessor, Type, TBaseResolver, CommandRegistrationProcessor> addMethod) {
        if (attribute is TBaseResolver resolver)
            addMethod(processor, ImplementedGenericType(resolver, genericType), resolver);
    }

    private static void Add<TResolver, TResult>(this List<ResolverContainer<TResolver, TResult>> list, Type suppliedGenericType, Type requiredGenericType, TResolver resolver) {
        var method = GetGenericResolverMethod(suppliedGenericType, requiredGenericType, resolver, out var param);
        list.Add(new ResolverContainer<TResolver, TResult>(method, param, resolver));
    }

    #endregion

    #region With Specific

    #region Base Properties

    public static CommandRegistrationProcessor WithNameResolver<T>(this CommandRegistrationProcessor processor, ICommandNameResolver<T> nameResolver) where T : Attribute
        => WithNameResolver(processor, typeof(T), nameResolver);

    public static CommandRegistrationProcessor WithNameResolver(this CommandRegistrationProcessor processor, Type type, ICommandNameResolver nameResolver) {
        processor.NameResolvers.Add(type, typeof(ICommandNameResolver<>), nameResolver);
        return processor;
    }

    public static CommandRegistrationProcessor WithDescriptionResolver<T>(this CommandRegistrationProcessor processor, ICommandDescriptionResolver<T> descriptionResolver) where T : Attribute
        => WithDescriptionResolver(processor, typeof(T), descriptionResolver);

    public static CommandRegistrationProcessor WithDescriptionResolver(this CommandRegistrationProcessor processor, Type type, ICommandDescriptionResolver descriptionResolver) {
        processor.DescriptionResolvers.Add(type, typeof(ICommandDescriptionResolver<>), descriptionResolver);
        return processor;
    }

    public static CommandRegistrationProcessor WithAliasResolver<T>(this CommandRegistrationProcessor processor, ICommandAliasResolver<T> aliasResolver) where T : Attribute
        => WithAliasResolver(processor, typeof(T), aliasResolver);

    public static CommandRegistrationProcessor WithAliasResolver(this CommandRegistrationProcessor processor, Type type, ICommandAliasResolver aliasResolver) {
        processor.AliasResolvers.Add(type, typeof(ICommandAliasResolver<>), aliasResolver);
        return processor;
    }

    public static CommandRegistrationProcessor WithUsageResolver<T>(this CommandRegistrationProcessor processor, ICommandUsageResolver<T> usageResolver) where T : Attribute
        => WithUsageResolver(processor, typeof(T), usageResolver);

    public static CommandRegistrationProcessor WithUsageResolver(this CommandRegistrationProcessor processor, Type type, ICommandUsageResolver usageResolver) {
        processor.UsageResolvers.Add(type, typeof(ICommandUsageResolver<>), usageResolver);
        return processor;
    }

    public static CommandRegistrationProcessor WithPermissionCreator<T>(this CommandRegistrationProcessor processor, ICommandPermissionCreator<T> permissionCreator) where T : Attribute
        => WithPermissionCreator(processor, typeof(T), permissionCreator);

    public static CommandRegistrationProcessor WithPermissionCreator(this CommandRegistrationProcessor processor, Type type, ICommandPermissionCreator permissionCreator) {
        processor.PermissionCreators.Add(type, typeof(ICommandPermissionCreator<>), permissionCreator);
        return processor;
    }

    #endregion

    #region Targeting Properties

    public static CommandRegistrationProcessor WithTargetingAffectedMultipleResolver<T>(this CommandRegistrationProcessor processor, IAffectedMultiplePlayersResolver<T> generatorResolver) where T : Attribute
        => WithTargetingAffectedMultipleResolver(processor, typeof(T), generatorResolver);

    public static CommandRegistrationProcessor WithTargetingAffectedMultipleResolver(this CommandRegistrationProcessor processor, Type type, IAffectedMultiplePlayersResolver generatorResolver) {
        processor.TargetingMultipleMessageResolvers.Add(type, typeof(IAffectedMultiplePlayersResolver<>), generatorResolver);
        return processor;
    }

    public static CommandRegistrationProcessor WithTargetingAffectedOneResolver<T>(this CommandRegistrationProcessor processor, IAffectedOnePlayerResolver<T> generatorResolver) where T : Attribute
        => WithTargetingAffectedOneResolver(processor, typeof(T), generatorResolver);

    public static CommandRegistrationProcessor WithTargetingAffectedOneResolver(this CommandRegistrationProcessor processor, Type type, IAffectedOnePlayerResolver generatorResolver) {
        processor.TargetingSingleMessageResolvers.Add(type, typeof(IAffectedOnePlayerResolver<>), generatorResolver);
        return processor;
    }

    public static CommandRegistrationProcessor WithTargetingAffectedAllResolver<T>(this CommandRegistrationProcessor processor, IAffectedAllPlayersResolver<T> generatorResolver) where T : Attribute
        => WithTargetingAffectedAllResolver(processor, typeof(T), generatorResolver);

    public static CommandRegistrationProcessor WithTargetingAffectedAllResolver(this CommandRegistrationProcessor processor, Type type, IAffectedAllPlayersResolver generatorResolver) {
        processor.TargetingAllMessageResolvers.Add(type, typeof(IAffectedAllPlayersResolver<>), generatorResolver);
        return processor;
    }

    public static CommandRegistrationProcessor WithTargetingSelectionResolver<T>(this CommandRegistrationProcessor processor, ITargetSelectionResolver<T> selectionResolver) where T : Attribute
        => WithTargetingSelectionResolver(processor, typeof(T), selectionResolver);

    public static CommandRegistrationProcessor WithTargetingSelectionResolver(this CommandRegistrationProcessor processor, Type type, ITargetSelectionResolver selectionResolver) {
        processor.TargetSelectionManagerResolvers.Add(type, typeof(ITargetSelectionResolver<>), selectionResolver);
        return processor;
    }

    #endregion

    #endregion

}
