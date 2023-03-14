using System;
using Axwabo.CommandSystem.Exceptions;
using Axwabo.CommandSystem.PropertyManager.Resolvers;

namespace Axwabo.CommandSystem.Registration;

public static partial class RegistrationExtensions
{

    #region Base Properties

    /// <summary>
    /// Adds a name resolver to the <see cref="CommandRegistrationProcessor"/>.
    /// </summary>
    /// <param name="processor">The processor to add the resolver to.</param>
    /// <param name="type">The type of the attribute to resolve the name from.</param>
    /// <param name="nameResolver">The resolver to add.</param>
    /// <returns>The processor itself.</returns>
    /// <exception cref="TypeMismatchException">Thrown when the resolver does not implement <see cref="ICommandNameResolver{T}"/> with the correct generic type.</exception>
    public static CommandRegistrationProcessor WithNameResolver(this CommandRegistrationProcessor processor, Type type, ICommandNameResolver nameResolver)
    {
        processor.NameResolvers.Add(type, typeof(ICommandNameResolver<>), nameResolver);
        return processor;
    }

    /// <summary>
    /// Adds a description resolver to the <see cref="CommandRegistrationProcessor"/>.
    /// </summary>
    /// <param name="processor">The processor to add the resolver to.</param>
    /// <param name="type">The type of the attribute to resolve the description from.</param>
    /// <param name="descriptionResolver">The resolver to add.</param>
    /// <returns>The processor itself.</returns>
    /// <exception cref="TypeMismatchException">Thrown when the resolver does not implement <see cref="ICommandDescriptionResolver{T}"/> with the correct generic type.</exception>
    public static CommandRegistrationProcessor WithDescriptionResolver(this CommandRegistrationProcessor processor, Type type, ICommandDescriptionResolver descriptionResolver)
    {
        processor.DescriptionResolvers.Add(type, typeof(ICommandDescriptionResolver<>), descriptionResolver);
        return processor;
    }

    /// <summary>
    /// Adds an alias resolver to the <see cref="CommandRegistrationProcessor"/>.
    /// </summary>
    /// <param name="processor">The processor to add the resolver to.</param>
    /// <param name="type">The type of the attribute to resolve the aliases from.</param>
    /// <param name="aliasResolver">The resolver to add.</param>
    /// <returns>The processor itself.</returns>
    /// <exception cref="TypeMismatchException">Thrown when the resolver does not implement <see cref="ICommandAliasResolver{T}"/> with the correct generic type.</exception>
    public static CommandRegistrationProcessor WithAliasResolver(this CommandRegistrationProcessor processor, Type type, ICommandAliasResolver aliasResolver)
    {
        processor.AliasResolvers.Add(type, typeof(ICommandAliasResolver<>), aliasResolver);
        return processor;
    }

    /// <summary>
    /// Adds a usage resolver to the <see cref="CommandRegistrationProcessor"/>.
    /// </summary>
    /// <param name="processor">The processor to add the resolver to.</param>
    /// <param name="type">The type of the attribute to resolve the usage from.</param>
    /// <param name="usageResolver">The resolver to add.</param>
    /// <returns>The processor itself.</returns>
    /// <exception cref="TypeMismatchException">Thrown when the resolver does not implement <see cref="ICommandUsageResolver{T}"/> with the correct generic type.</exception>
    public static CommandRegistrationProcessor WithUsageResolver(this CommandRegistrationProcessor processor, Type type, ICommandUsageResolver usageResolver)
    {
        processor.UsageResolvers.Add(type, typeof(ICommandUsageResolver<>), usageResolver);
        return processor;
    }

    /// <summary>
    /// Adds a permission creator to the <see cref="CommandRegistrationProcessor"/>.
    /// </summary>
    /// <param name="processor">The processor to add the resolver to.</param>
    /// <param name="type">The type of the attribute to resolve the permission from.</param>
    /// <param name="permissionCreator">The resolver to add.</param>
    /// <returns>The processor itself.</returns>
    /// <exception cref="TypeMismatchException">Thrown when the resolver does not implement <see cref="IAttributeBasedPermissionCreator{TAttribute}"/> with the correct generic type.</exception>
    public static CommandRegistrationProcessor WithPermissionCreator(this CommandRegistrationProcessor processor, Type type, IAttributeBasedPermissionCreator permissionCreator)
    {
        processor.PermissionCreators.Add(type, typeof(IAttributeBasedPermissionCreator<>), permissionCreator);
        return processor;
    }

    #endregion

    #region Targeting Properties

    /// <summary>
    /// Adds a targeting affected multiple players message generator resolver to the <see cref="CommandRegistrationProcessor"/>.
    /// </summary>
    /// <param name="processor">The processor to add the resolver to.</param>
    /// <param name="type">The type of the attribute to resolve the generator from.</param>
    /// <param name="generatorResolver">The resolver to add.</param>
    /// <returns>The processor itself.</returns>
    /// <exception cref="TypeMismatchException">Thrown when the resolver does not implement <see cref="IAffectedMultiplePlayersResolver{T}"/> with the correct generic type.</exception>
    public static CommandRegistrationProcessor WithTargetingAffectedMultipleResolver(this CommandRegistrationProcessor processor, Type type, IAffectedMultiplePlayersResolver generatorResolver)
    {
        processor.TargetingMultipleMessageResolvers.Add(type, typeof(IAffectedMultiplePlayersResolver<>), generatorResolver);
        return processor;
    }

    /// <summary>
    /// Adds a targeting affected one player message generator resolver to the <see cref="CommandRegistrationProcessor"/>.
    /// </summary>
    /// <param name="processor">The processor to add the resolver to.</param>
    /// <param name="type">The type of the attribute to resolve the generator from.</param>
    /// <param name="generatorResolver">The resolver to add.</param>
    /// <returns>The processor itself.</returns>
    /// <exception cref="TypeMismatchException">Thrown when the resolver does not implement <see cref="IAffectedOnePlayerResolver{T}"/> with the correct generic type.</exception>
    public static CommandRegistrationProcessor WithTargetingAffectedOneResolver(this CommandRegistrationProcessor processor, Type type, IAffectedOnePlayerResolver generatorResolver)
    {
        processor.TargetingSingleMessageResolvers.Add(type, typeof(IAffectedOnePlayerResolver<>), generatorResolver);
        return processor;
    }

    /// <summary>
    /// Adds a targeting affected all players message generator resolver to the <see cref="CommandRegistrationProcessor"/>.
    /// </summary>
    /// <param name="processor">The processor to add the resolver to.</param>
    /// <param name="type">The type of the attribute to resolve the generator from.</param>
    /// <param name="generatorResolver">The resolver to add.</param>
    /// <returns>The processor itself.</returns>
    /// <exception cref="TypeMismatchException">Thrown if the resolver does not implement the generic <see cref="IAffectedAllPlayersResolver{TAttribute}"/>.</exception>
    public static CommandRegistrationProcessor WithTargetingAffectedAllResolver(this CommandRegistrationProcessor processor, Type type, IAffectedAllPlayersResolver generatorResolver)
    {
        processor.TargetingAllMessageResolvers.Add(type, typeof(IAffectedAllPlayersResolver<>), generatorResolver);
        return processor;
    }

    /// <summary>
    /// Adds a targeting selection manager resolver to the <see cref="CommandRegistrationProcessor"/>.
    /// </summary>
    /// <param name="processor">The processor to add the resolver to.</param>
    /// <param name="type">The type of the attribute to resolve the selection manager from.</param>
    /// <param name="selectionResolver">The resolver to add.</param>
    /// <returns>The processor itself.</returns>
    /// <exception cref="TypeMismatchException">Thrown if the resolver does not implement the generic <see cref="ITargetSelectionResolver{TAttribute}"/>.</exception>
    public static CommandRegistrationProcessor WithTargetingSelectionResolver(this CommandRegistrationProcessor processor, Type type, ITargetSelectionResolver selectionResolver)
    {
        processor.TargetSelectionManagerResolvers.Add(type, typeof(ITargetSelectionResolver<>), selectionResolver);
        return processor;
    }

    #endregion

}
