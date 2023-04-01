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
public static partial class RegistrationExtensions
{

    #region Helpers

    /// <summary>
    /// Determines whether a <see cref="CommandHandlerType"/> is flagged with a specific value.
    /// </summary>
    /// <param name="target">The flags to check.</param>
    /// <param name="flag">The flag to find.</param>
    /// <returns>Whether the flag is present.</returns>
    public static bool HasFlagFast(this CommandHandlerType target, CommandHandlerType flag) => (target & flag) == flag;

    private static MethodInfo GetGenericResolverMethod(Type parameterType, Type interfaceType, object resolver, out Type param)
    {
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

    private static void Add<TResolver, TResult>(this List<ResolverContainer<TResolver, TResult>> list, Type suppliedGenericType, Type requiredGenericType, TResolver resolver)
    {
        var method = GetGenericResolverMethod(suppliedGenericType, requiredGenericType, resolver, out var param);
        list.Add(new ResolverContainer<TResolver, TResult>(method, param, resolver));
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
    public static CommandRegistrationProcessor WithRegistrationAttributesFrom(this CommandRegistrationProcessor processor, Type type)
    {
        if (processor == null)
            throw new ArgumentNullException(nameof(processor));
        if (type == null)
            throw new ArgumentNullException(nameof(type));
        foreach (var attribute in type.GetCustomAttributes())
            WithMultiplexResolverObject(processor, attribute);
        return processor;
    }

    /// <summary>
    /// Adds all registration resolvers to the <see cref="CommandRegistrationProcessor"/> from the given object instance.
    /// </summary>
    /// <param name="processor">The processor to add the resolvers to.</param>
    /// <param name="instance">The instance to get the resolvers from.</param>
    /// <returns>The processor itself.</returns>
    public static CommandRegistrationProcessor WithMultiplexResolverObject(this CommandRegistrationProcessor processor, object instance)
    {
        processor.ConsumeInstance<ICommandNameResolver>(instance, typeof(ICommandNameResolver<>), WithNameResolver);
        processor.ConsumeInstance<ICommandDescriptionResolver>(instance, typeof(ICommandDescriptionResolver<>), WithDescriptionResolver);
        processor.ConsumeInstance<ICommandAliasResolver>(instance, typeof(ICommandAliasResolver<>), WithAliasResolver);
        processor.ConsumeInstance<ICommandUsageResolver>(instance, typeof(ICommandUsageResolver<>), WithUsageResolver);
        processor.ConsumeInstance<IAttributeBasedPermissionResolver>(instance, typeof(IAttributeBasedPermissionResolver<>), WithPermissionResolver);

        processor.ConsumeInstance<IAffectedMultiplePlayersResolver>(instance, typeof(IAffectedMultiplePlayersResolver<>), WithTargetingAffectedMultipleResolver);
        processor.ConsumeInstance<IAffectedOnePlayerResolver>(instance, typeof(IAffectedOnePlayerResolver<>), WithTargetingAffectedOneResolver);
        processor.ConsumeInstance<IAffectedAllPlayersResolver>(instance, typeof(IAffectedAllPlayersResolver<>), WithTargetingAffectedAllResolver);
        processor.ConsumeInstance<ITargetSelectionResolver>(instance, typeof(ITargetSelectionResolver<>), WithTargetingSelectionResolver);

        processor.ConsumeInstance<IRemoteAdminOptionIdResolver>(instance, typeof(IRemoteAdminOptionIdResolver<>), WithRemoteAdminOptionIdResolver);
        processor.ConsumeInstance<IStaticOptionTextResolver>(instance, typeof(IStaticOptionTextResolver<>), WithRemoteAdminOptionTextResolver);
        processor.ConsumeInstance<IOptionIconResolver>(instance, typeof(IOptionIconResolver<>), WithRemoteAdminOptionIconResolver);

        return processor;
    }

    private static void ConsumeInstance<TBaseResolver>(this CommandRegistrationProcessor processor, object instance, Type genericType, Func<CommandRegistrationProcessor, Type, TBaseResolver, CommandRegistrationProcessor> addMethod)
    {
        if (instance is TBaseResolver resolver)
            addMethod(processor, GenericTypeExtensions.ImplementedGenericType(resolver, genericType), resolver);
    }

    #endregion

}
