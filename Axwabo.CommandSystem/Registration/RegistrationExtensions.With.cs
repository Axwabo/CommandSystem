using System;
using Axwabo.CommandSystem.PropertyManager.Resolvers;

namespace Axwabo.CommandSystem.Registration;

public static partial class RegistrationExtensions
{

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
    /// Adds a description resolver to the <see cref="CommandRegistrationProcessor"/>.
    /// </summary>
    /// <param name="processor">The processor to add the resolver to.</param>
    /// <param name="descriptionResolver">The resolver to add.</param>
    /// <typeparam name="T">The type of the attribute to resolve the description from.</typeparam>
    /// <returns>The processor itself.</returns>
    public static CommandRegistrationProcessor WithDescriptionResolver<T>(this CommandRegistrationProcessor processor, ICommandDescriptionResolver<T> descriptionResolver) where T : Attribute
        => WithDescriptionResolver(processor, typeof(T), descriptionResolver);

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
    /// Adds a usage resolver to the <see cref="CommandRegistrationProcessor"/>.
    /// </summary>
    /// <param name="processor">The processor to add the resolver to.</param>
    /// <param name="usageResolver">The resolver to add.</param>
    /// <typeparam name="T">The type of the attribute to resolve the usage from.</typeparam>
    /// <returns>The processor itself.</returns>
    public static CommandRegistrationProcessor WithUsageResolver<T>(this CommandRegistrationProcessor processor, ICommandUsageResolver<T> usageResolver) where T : Attribute
        => WithUsageResolver(processor, typeof(T), usageResolver);

    /// <summary>
    /// Adds a permission creator to the <see cref="CommandRegistrationProcessor"/>.
    /// </summary>
    /// <param name="processor">The processor to add the resolver to.</param>
    /// <param name="permissionCreator">The resolver to add.</param>
    /// <typeparam name="T">The type of the attribute to resolve the permission from.</typeparam>
    /// <returns>The processor itself.</returns>
    public static CommandRegistrationProcessor WithPermissionCreator<T>(this CommandRegistrationProcessor processor, IAttributeBasedPermissionCreator<T> permissionCreator) where T : Attribute
        => WithPermissionCreator(processor, typeof(T), permissionCreator);

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
    /// Adds a targeting affected one player message generator resolver to the <see cref="CommandRegistrationProcessor"/>.
    /// </summary>
    /// <param name="processor">The processor to add the resolver to.</param>
    /// <param name="generatorResolver">The resolver to add.</param>
    /// <typeparam name="T">The type of the attribute to resolve the generator from.</typeparam>
    /// <returns>The processor itself.</returns>
    public static CommandRegistrationProcessor WithTargetingAffectedOneResolver<T>(this CommandRegistrationProcessor processor, IAffectedOnePlayerResolver<T> generatorResolver) where T : Attribute
        => WithTargetingAffectedOneResolver(processor, typeof(T), generatorResolver);

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
    /// Adds a targeting selection manager resolver to the <see cref="CommandRegistrationProcessor"/>.
    /// </summary>
    /// <param name="processor">The processor to add the resolver to.</param>
    /// <param name="selectionResolver">The resolver to add.</param>
    /// <typeparam name="T">The type of the attribute to resolve the selection manager from.</typeparam>
    /// <returns>The processor itself.</returns>
    public static CommandRegistrationProcessor WithTargetingSelectionResolver<T>(this CommandRegistrationProcessor processor, ITargetSelectionResolver<T> selectionResolver) where T : Attribute
        => WithTargetingSelectionResolver(processor, typeof(T), selectionResolver);

    #endregion

}
