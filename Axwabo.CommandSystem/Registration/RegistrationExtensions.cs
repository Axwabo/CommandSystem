using System;
using System.Collections.Generic;
using System.Reflection;
using Axwabo.CommandSystem.Attributes;
using Axwabo.CommandSystem.Exceptions;
using Axwabo.CommandSystem.PropertyManager.Resolvers;

namespace Axwabo.CommandSystem.Registration;

/// <summary>
/// Extensions for the <see cref="CommandRegistrationProcessor"/>.
/// </summary>
public static class RegistrationExtensions {

    #region Helpers

    /// <summary>
    /// Determines whether a <see cref="CommandHandlerType"/> is flagged with a specific value.
    /// </summary>
    /// <param name="target">The flags to check.</param>
    /// <param name="flag">The flag to find.</param>
    /// <returns>Whether the flag is present.</returns>
    public static bool HasFlagFast(this CommandHandlerType target, CommandHandlerType flag) => (target & flag) == flag;

    private static MethodInfo GetGenericResolverMethod(Type parameterType, Type interfaceType, object resolver, out Type param) {
        if (parameterType == null)
            throw new ArgumentNullException(nameof(parameterType));
        if (interfaceType == null)
            throw new ArgumentNullException(nameof(interfaceType));
        if (resolver == null)
            throw new ArgumentNullException(nameof(resolver));
        var implementedInterface = GenericTypeExtensions.GetGenericInterface(resolver.GetType(), interfaceType);
        if (implementedInterface == null)
            throw new TypeMismatchException($"Expected an implementation of {interfaceType.Name} with generic type {parameterType.FullName}, got: {resolver.GetType().FullName}");
        var method = implementedInterface.GetMethods()[0];
        var parameters = method.GetParameters();
        if (parameters is not {Length: 1})
            throw new TypeMismatchException($"Expected an implementation {interfaceType.Name} with a single parameter of type {parameterType.FullName}");
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

    /// <summary>
    /// Adds all registration attributes to the <see cref="CommandRegistrationProcessor"/> from the given <see cref="Type"/>.
    /// </summary>
    /// <param name="processor">The processor to add the attributes to.</param>
    /// <param name="type">The type to get the attributes from.</param>
    /// <returns>The processor itself.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="processor"/> or <paramref name="type"/> is null.</exception>
    public static CommandRegistrationProcessor WithRegistrationAttributesFrom(this CommandRegistrationProcessor processor, Type type) {
        if (processor == null)
            throw new ArgumentNullException(nameof(processor));
        if (type == null)
            throw new ArgumentNullException(nameof(type));
        foreach (var attribute in type.GetCustomAttributes())
            AddRegistrationAttribute(processor, attribute);
        return processor;
    }

    private static void AddRegistrationAttribute(CommandRegistrationProcessor processor, Attribute attribute) {
        processor.ConsumeAttribute<ICommandNameResolver>(attribute, typeof(ICommandNameResolver<>), WithNameResolver);
        processor.ConsumeAttribute<ICommandDescriptionResolver>(attribute, typeof(ICommandDescriptionResolver<>), WithDescriptionResolver);
        processor.ConsumeAttribute<ICommandAliasResolver>(attribute, typeof(ICommandAliasResolver<>), WithAliasResolver);
        processor.ConsumeAttribute<ICommandUsageResolver>(attribute, typeof(ICommandUsageResolver<>), WithUsageResolver);
        processor.ConsumeAttribute<ICommandPermissionCreator>(attribute, typeof(ICommandPermissionCreator<>), WithPermissionCreator);
    }

    private static void ConsumeAttribute<TBaseResolver>(this CommandRegistrationProcessor processor, Attribute attribute, Type genericType, Func<CommandRegistrationProcessor, Type, TBaseResolver, CommandRegistrationProcessor> addMethod) {
        if (attribute is TBaseResolver resolver)
            addMethod(processor, GenericTypeExtensions.ImplementedGenericType(resolver, genericType), resolver);
    }

    private static void Add<TResolver, TResult>(this List<ResolverContainer<TResolver, TResult>> list, Type suppliedGenericType, Type requiredGenericType, TResolver resolver) {
        var method = GetGenericResolverMethod(suppliedGenericType, requiredGenericType, resolver, out var param);
        list.Add(new ResolverContainer<TResolver, TResult>(method, param, resolver));
    }

    #endregion

    #region With Specific

    #region Base Properties

    /// <summary>
    /// Adds a name resolver to the <see cref="CommandRegistrationProcessor"/>.
    /// </summary>
    /// <param name="processor">The processor to add the resolver to.</param>
    /// <param name="nameResolver">The resolver to add.</param>
    /// <typeparam name="T">The type of the attribute to resolve the name from.</typeparam>
    /// <returns>The processor itself.</returns>
    public static CommandRegistrationProcessor WithNameResolver<T>(this CommandRegistrationProcessor processor, ICommandNameResolver<T> nameResolver) where T : Attribute
        => WithNameResolver(processor, typeof(T), nameResolver);

    /// <summary>
    /// Adds a name resolver to the <see cref="CommandRegistrationProcessor"/>.
    /// </summary>
    /// <param name="processor">The processor to add the resolver to.</param>
    /// <param name="type">The type of the attribute to resolve the name from.</param>
    /// <param name="nameResolver">The resolver to add.</param>
    /// <returns>The processor itself.</returns>
    /// <exception cref="TypeMismatchException">Thrown when the resolver does not implement <see cref="ICommandNameResolver{T}"/> with the correct generic type.</exception>
    public static CommandRegistrationProcessor WithNameResolver(this CommandRegistrationProcessor processor, Type type, ICommandNameResolver nameResolver) {
        processor.NameResolvers.Add(type, typeof(ICommandNameResolver<>), nameResolver);
        return processor;
    }

    /// <summary>
    /// Adds a description resolver to the <see cref="CommandRegistrationProcessor"/>.
    /// </summary>
    /// <param name="processor">The processor to add the resolver to.</param>
    /// <param name="descriptionResolver">The resolver to add.</param>
    /// <typeparam name="T">The type of the attribute to resolve the description from.</typeparam>
    /// <returns>The processor itself.</returns>
    public static CommandRegistrationProcessor WithDescriptionResolver<T>(this CommandRegistrationProcessor processor, ICommandDescriptionResolver<T> descriptionResolver) where T : Attribute
        => WithDescriptionResolver(processor, typeof(T), descriptionResolver);

    /// <summary>
    /// Adds a description resolver to the <see cref="CommandRegistrationProcessor"/>.
    /// </summary>
    /// <param name="processor">The processor to add the resolver to.</param>
    /// <param name="type">The type of the attribute to resolve the description from.</param>
    /// <param name="descriptionResolver">The resolver to add.</param>
    /// <returns>The processor itself.</returns>
    /// <exception cref="TypeMismatchException">Thrown when the resolver does not implement <see cref="ICommandDescriptionResolver{T}"/> with the correct generic type.</exception>
    public static CommandRegistrationProcessor WithDescriptionResolver(this CommandRegistrationProcessor processor, Type type, ICommandDescriptionResolver descriptionResolver) {
        processor.DescriptionResolvers.Add(type, typeof(ICommandDescriptionResolver<>), descriptionResolver);
        return processor;
    }

    /// <summary>
    /// Adds an alias resolver to the <see cref="CommandRegistrationProcessor"/>.
    /// </summary>
    /// <param name="processor">The processor to add the resolver to.</param>
    /// <param name="aliasResolver">The resolver to add.</param>
    /// <typeparam name="T">The type of the attribute to resolve the aliases from.</typeparam>
    /// <returns>The processor itself.</returns>
    public static CommandRegistrationProcessor WithAliasResolver<T>(this CommandRegistrationProcessor processor, ICommandAliasResolver<T> aliasResolver) where T : Attribute
        => WithAliasResolver(processor, typeof(T), aliasResolver);

    /// <summary>
    /// Adds an alias resolver to the <see cref="CommandRegistrationProcessor"/>.
    /// </summary>
    /// <param name="processor">The processor to add the resolver to.</param>
    /// <param name="type">The type of the attribute to resolve the aliases from.</param>
    /// <param name="aliasResolver">The resolver to add.</param>
    /// <returns>The processor itself.</returns>
    /// <exception cref="TypeMismatchException">Thrown when the resolver does not implement <see cref="ICommandAliasResolver{T}"/> with the correct generic type.</exception>
    public static CommandRegistrationProcessor WithAliasResolver(this CommandRegistrationProcessor processor, Type type, ICommandAliasResolver aliasResolver) {
        processor.AliasResolvers.Add(type, typeof(ICommandAliasResolver<>), aliasResolver);
        return processor;
    }

    /// <summary>
    /// Adds a usage resolver to the <see cref="CommandRegistrationProcessor"/>.
    /// </summary>
    /// <param name="processor">The processor to add the resolver to.</param>
    /// <param name="usageResolver">The resolver to add.</param>
    /// <typeparam name="T">The type of the attribute to resolve the usage from.</typeparam>
    /// <returns>The processor itself.</returns>
    public static CommandRegistrationProcessor WithUsageResolver<T>(this CommandRegistrationProcessor processor, ICommandUsageResolver<T> usageResolver) where T : Attribute
        => WithUsageResolver(processor, typeof(T), usageResolver);

    /// <summary>
    /// Adds a usage resolver to the <see cref="CommandRegistrationProcessor"/>.
    /// </summary>
    /// <param name="processor">The processor to add the resolver to.</param>
    /// <param name="type">The type of the attribute to resolve the usage from.</param>
    /// <param name="usageResolver">The resolver to add.</param>
    /// <returns>The processor itself.</returns>
    /// <exception cref="TypeMismatchException">Thrown when the resolver does not implement <see cref="ICommandUsageResolver{T}"/> with the correct generic type.</exception>
    public static CommandRegistrationProcessor WithUsageResolver(this CommandRegistrationProcessor processor, Type type, ICommandUsageResolver usageResolver) {
        processor.UsageResolvers.Add(type, typeof(ICommandUsageResolver<>), usageResolver);
        return processor;
    }

    /// <summary>
    /// Adds a permission creator to the <see cref="CommandRegistrationProcessor"/>.
    /// </summary>
    /// <param name="processor">The processor to add the resolver to.</param>
    /// <param name="permissionCreator">The resolver to add.</param>
    /// <typeparam name="T">The type of the attribute to resolve the permission from.</typeparam>
    /// <returns>The processor itself.</returns>
    public static CommandRegistrationProcessor WithPermissionCreator<T>(this CommandRegistrationProcessor processor, ICommandPermissionCreator<T> permissionCreator) where T : Attribute
        => WithPermissionCreator(processor, typeof(T), permissionCreator);

    /// <summary>
    /// Adds a permission creator to the <see cref="CommandRegistrationProcessor"/>.
    /// </summary>
    /// <param name="processor">The processor to add the resolver to.</param>
    /// <param name="type">The type of the attribute to resolve the permission from.</param>
    /// <param name="permissionCreator">The resolver to add.</param>
    /// <returns>The processor itself.</returns>
    /// <exception cref="TypeMismatchException">Thrown when the resolver does not implement <see cref="ICommandPermissionCreator{T}"/> with the correct generic type.</exception>
    public static CommandRegistrationProcessor WithPermissionCreator(this CommandRegistrationProcessor processor, Type type, ICommandPermissionCreator permissionCreator) {
        processor.PermissionCreators.Add(type, typeof(ICommandPermissionCreator<>), permissionCreator);
        return processor;
    }

    #endregion

    #region Targeting Properties

    /// <summary>
    /// Adds a targeting affected multiple players message generator resolver to the <see cref="CommandRegistrationProcessor"/>.
    /// </summary>
    /// <param name="processor">The processor to add the resolver to.</param>
    /// <param name="generatorResolver">The resolver to add.</param>
    /// <typeparam name="T">The type of the attribute to resolve the generator from.</typeparam>
    /// <returns>The processor itself.</returns>
    public static CommandRegistrationProcessor WithTargetingAffectedMultipleResolver<T>(this CommandRegistrationProcessor processor, IAffectedMultiplePlayersResolver<T> generatorResolver) where T : Attribute
        => WithTargetingAffectedMultipleResolver(processor, typeof(T), generatorResolver);

    /// <summary>
    /// Adds a targeting affected multiple players message generator resolver to the <see cref="CommandRegistrationProcessor"/>.
    /// </summary>
    /// <param name="processor">The processor to add the resolver to.</param>
    /// <param name="type">The type of the attribute to resolve the generator from.</param>
    /// <param name="generatorResolver">The resolver to add.</param>
    /// <returns>The processor itself.</returns>
    /// <exception cref="TypeMismatchException">Thrown when the resolver does not implement <see cref="IAffectedMultiplePlayersResolver{T}"/> with the correct generic type.</exception>
    public static CommandRegistrationProcessor WithTargetingAffectedMultipleResolver(this CommandRegistrationProcessor processor, Type type, IAffectedMultiplePlayersResolver generatorResolver) {
        processor.TargetingMultipleMessageResolvers.Add(type, typeof(IAffectedMultiplePlayersResolver<>), generatorResolver);
        return processor;
    }

    /// <summary>
    /// Adds a targeting affected one player message generator resolver to the <see cref="CommandRegistrationProcessor"/>.
    /// </summary>
    /// <param name="processor">The processor to add the resolver to.</param>
    /// <param name="generatorResolver">The resolver to add.</param>
    /// <typeparam name="T">The type of the attribute to resolve the generator from.</typeparam>
    /// <returns>The processor itself.</returns>
    public static CommandRegistrationProcessor WithTargetingAffectedOneResolver<T>(this CommandRegistrationProcessor processor, IAffectedOnePlayerResolver<T> generatorResolver) where T : Attribute
        => WithTargetingAffectedOneResolver(processor, typeof(T), generatorResolver);

    /// <summary>
    /// Adds a targeting affected one player message generator resolver to the <see cref="CommandRegistrationProcessor"/>.
    /// </summary>
    /// <param name="processor">The processor to add the resolver to.</param>
    /// <param name="type">The type of the attribute to resolve the generator from.</param>
    /// <param name="generatorResolver">The resolver to add.</param>
    /// <returns>The processor itself.</returns>
    /// <exception cref="TypeMismatchException">Thrown when the resolver does not implement <see cref="IAffectedOnePlayerResolver{T}"/> with the correct generic type.</exception>
    public static CommandRegistrationProcessor WithTargetingAffectedOneResolver(this CommandRegistrationProcessor processor, Type type, IAffectedOnePlayerResolver generatorResolver) {
        processor.TargetingSingleMessageResolvers.Add(type, typeof(IAffectedOnePlayerResolver<>), generatorResolver);
        return processor;
    }

    /// <summary>
    /// Adds a targeting affected all players message generator resolver to the <see cref="CommandRegistrationProcessor"/>.
    /// </summary>
    /// <param name="processor">The processor to add the resolver to.</param>
    /// <param name="generatorResolver">The resolver to add.</param>
    /// <typeparam name="T">The type of the attribute to resolve the generator from.</typeparam>
    /// <returns>The processor itself.</returns>
    public static CommandRegistrationProcessor WithTargetingAffectedAllResolver<T>(this CommandRegistrationProcessor processor, IAffectedAllPlayersResolver<T> generatorResolver) where T : Attribute
        => WithTargetingAffectedAllResolver(processor, typeof(T), generatorResolver);

    /// <summary>
    /// Adds a targeting affected all players message generator resolver to the <see cref="CommandRegistrationProcessor"/>.
    /// </summary>
    /// <param name="processor">The processor to add the resolver to.</param>
    /// <param name="type">The type of the attribute to resolve the generator from.</param>
    /// <param name="generatorResolver">The resolver to add.</param>
    /// <returns>The processor itself.</returns>
    /// <exception cref="TypeMismatchException">Thrown if the resolver does not implement the generic <see cref="IAffectedAllPlayersResolver{TAttribute}"/>.</exception>
    public static CommandRegistrationProcessor WithTargetingAffectedAllResolver(this CommandRegistrationProcessor processor, Type type, IAffectedAllPlayersResolver generatorResolver) {
        processor.TargetingAllMessageResolvers.Add(type, typeof(IAffectedAllPlayersResolver<>), generatorResolver);
        return processor;
    }

    /// <summary>
    /// Adds a targeting selection manager resolver to the <see cref="CommandRegistrationProcessor"/>.
    /// </summary>
    /// <param name="processor">The processor to add the resolver to.</param>
    /// <param name="selectionResolver">The resolver to add.</param>
    /// <typeparam name="T">The type of the attribute to resolve the selection manager from.</typeparam>
    /// <returns>The processor itself.</returns>
    public static CommandRegistrationProcessor WithTargetingSelectionResolver<T>(this CommandRegistrationProcessor processor, ITargetSelectionResolver<T> selectionResolver) where T : Attribute
        => WithTargetingSelectionResolver(processor, typeof(T), selectionResolver);

    /// <summary>
    /// Adds a targeting selection manager resolver to the <see cref="CommandRegistrationProcessor"/>.
    /// </summary>
    /// <param name="processor">The processor to add the resolver to.</param>
    /// <param name="type">The type of the attribute to resolve the selection manager from.</param>
    /// <param name="selectionResolver">The resolver to add.</param>
    /// <returns>The processor itself.</returns>
    /// <exception cref="TypeMismatchException">Thrown if the resolver does not implement the generic <see cref="ITargetSelectionResolver{TAttribute}"/>.</exception>
    public static CommandRegistrationProcessor WithTargetingSelectionResolver(this CommandRegistrationProcessor processor, Type type, ITargetSelectionResolver selectionResolver) {
        processor.TargetSelectionManagerResolvers.Add(type, typeof(ITargetSelectionResolver<>), selectionResolver);
        return processor;
    }

    #endregion

    #endregion

}
