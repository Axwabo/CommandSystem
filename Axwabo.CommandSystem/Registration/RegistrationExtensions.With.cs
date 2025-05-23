﻿using Axwabo.CommandSystem.PropertyManager.Resolvers;

namespace Axwabo.CommandSystem.Registration;

public static partial class RegistrationExtensions
{

    #region Types

    /// <summary>Adds the given type to the <see cref="CommandRegistrationProcessor"/>.</summary>
    /// <param name="processor">The processor to add the type to.</param>
    /// <param name="type">The type to add.</param>
    /// <returns>The processor itself.</returns>
    /// <exception cref="ArgumentException">If the type is not from the same assembly as the <see cref="CommandRegistrationProcessor.TargetAssembly"/>, is abstract or is not a class.</exception>
    public static CommandRegistrationProcessor WithType(this CommandRegistrationProcessor processor, Type type)
    {
        processor.AddType(type);
        return processor;
    }

    /// <summary>Adds the given types to the <see cref="CommandRegistrationProcessor"/>.</summary>
    /// <param name="processor">The processor to add the types to.</param>
    /// <param name="types">The types to add.</param>
    /// <returns>The processor itself.</returns>
    /// <exception cref="ArgumentException">If a type in the enumerable is not from the same assembly as the <see cref="CommandRegistrationProcessor.TargetAssembly"/>, is abstract or is not a class.</exception>
    public static CommandRegistrationProcessor WithTypes(this CommandRegistrationProcessor processor, IEnumerable<Type> types)
    {
        foreach (var type in types)
            processor.AddType(type);
        return processor;
    }

    /// <summary>
    /// Adds all types to the <see cref="CommandRegistrationProcessor"/> from its <see cref="CommandRegistrationProcessor.TargetAssembly"/>.
    /// </summary>
    /// <param name="processor">The processor to add the types to.</param>
    /// <returns>The processor itself.</returns>
    public static CommandRegistrationProcessor WithTypesFromOriginalAssembly(this CommandRegistrationProcessor processor)
        => processor.WithTypes(processor.TargetAssembly.GetTypes().Where(e => e.IsClass && !e.IsAbstract));

    #endregion

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
    /// Adds a player-only status resolver to the <see cref="CommandRegistrationProcessor"/>.
    /// </summary>
    /// <param name="processor">The processor to add the resolver to.</param>
    /// <param name="playerOnlyResolver">The resolver to add.</param>
    /// <typeparam name="T">The type of the attribute to resolve the usage from.</typeparam>
    /// <returns>The processor itself.</returns>
    public static CommandRegistrationProcessor WithPlayerOnlyResolver<T>(this CommandRegistrationProcessor processor, IPlayerOnlyResolver<T> playerOnlyResolver) where T : Attribute
        => WithPlayerOnlyResolver(processor, typeof(T), playerOnlyResolver);

    /// <summary>
    /// Adds a permission resolve to the <see cref="CommandRegistrationProcessor"/>.
    /// </summary>
    /// <param name="processor">The processor to add the resolver to.</param>
    /// <param name="permissionResolver">The resolver to add.</param>
    /// <typeparam name="T">The type of the attribute to resolve the permission from.</typeparam>
    /// <returns>The processor itself.</returns>
    public static CommandRegistrationProcessor WithPermissionResolver<T>(this CommandRegistrationProcessor processor, IAttributeBasedPermissionResolver<T> permissionResolver) where T : Attribute
        => WithPermissionResolver(processor, typeof(T), permissionResolver);

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

    /// <summary>
    /// Adds a targeting custom result compiler resolver to the <see cref="CommandRegistrationProcessor"/>.
    /// </summary>
    /// <param name="processor">The processor to add the resolver to.</param>
    /// <param name="compilerResolver">The resolver to add.</param>
    /// <typeparam name="T">The type of the attribute to resolve the compiler from.</typeparam>
    /// <returns>The processor itself.</returns>
    public static CommandRegistrationProcessor WithTargetingCustomResultCompilerResolver<T>(this CommandRegistrationProcessor processor, IResultCompilerResolver<T> compilerResolver) where T : Attribute
        => WithResultCompilerResolver(processor, typeof(T), compilerResolver);

    #endregion

    #region Remote Admin Option Properties

    /// <summary>
    /// Adds a Remote Admin option id resolver to the <see cref="CommandRegistrationProcessor"/>.
    /// </summary>
    /// <param name="processor">The processor to add the resolver to.</param>
    /// <param name="idResolver">The resolver to add.</param>
    /// <typeparam name="T">The type of the attribute to resolve the id from.</typeparam>
    /// <returns>The processor itself.</returns>
    public static CommandRegistrationProcessor WithRemoteAdminOptionIdResolver<T>(this CommandRegistrationProcessor processor, IRemoteAdminOptionIdResolver<T> idResolver) where T : Attribute
        => WithRemoteAdminOptionIdResolver(processor, typeof(T), idResolver);

    /// <summary>
    /// Adds a static Remote Admin option text resolver to the <see cref="CommandRegistrationProcessor"/>.
    /// </summary>
    /// <param name="processor">The processor to add the resolver to.</param>
    /// <param name="textResolver">The resolver to add.</param>
    /// <typeparam name="T">The type of the attribute to resolve the text from.</typeparam>
    /// <returns>The processor itself.</returns>
    public static CommandRegistrationProcessor WithRemoteAdminOptionTextResolver<T>(this CommandRegistrationProcessor processor, IStaticOptionTextResolver<T> textResolver) where T : Attribute
        => WithRemoteAdminOptionTextResolver(processor, typeof(T), textResolver);

    /// <summary>
    /// Adds a Remote Admin option icon resolver to the <see cref="CommandRegistrationProcessor"/>.
    /// </summary>
    /// <param name="processor">The processor to add the resolver to.</param>
    /// <param name="iconResolver">The resolver to add.</param>
    /// <typeparam name="T">The type of the attribute to resolve the icon from.</typeparam>
    /// <returns>The processor itself.</returns>
    public static CommandRegistrationProcessor WithRemoteAdminOptionIconResolver<T>(this CommandRegistrationProcessor processor, IOptionIconResolver<T> iconResolver) where T : Attribute
        => WithRemoteAdminOptionIconResolver(processor, typeof(T), iconResolver);

    #endregion

}
