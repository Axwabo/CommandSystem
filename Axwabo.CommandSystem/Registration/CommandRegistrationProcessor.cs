#if EXILED
using Exiled.API.Features;
#else
using PluginAPI.Core;
#endif
using System;
using System.Collections.Generic;
using System.Reflection;
using Axwabo.CommandSystem.Attributes;
using Axwabo.CommandSystem.Commands.MessageOverrides;
using Axwabo.CommandSystem.Permissions;
using Axwabo.CommandSystem.PropertyManager;
using Axwabo.CommandSystem.PropertyManager.Resolvers;
using Axwabo.CommandSystem.RemoteAdminExtensions;
using CommandSystem;
using RemoteAdmin;

namespace Axwabo.CommandSystem.Registration;

/// <summary>Handles the registration of commands.</summary>
public sealed class CommandRegistrationProcessor
{

    #region Auto-Register

    /// <summary>Registers all commands in the assembly of the given object instance with registration attributes on the type.</summary>
    /// <param name="assemblyMemberInstance">An object from the assembly.</param>
    public static void RegisterAll(object assemblyMemberInstance) => RegisterAll(assemblyMemberInstance.GetType());

    /// <summary>Registers all commands in the assembly of the given type with registration attributes on the it.</summary>
    /// <param name="typeInAssembly">A type from the assembly.</param>
    public static void RegisterAll(Type typeInAssembly)
        => Create(typeInAssembly.Assembly)
            .WithRegistrationAttributesFrom(typeInAssembly)
            .Execute();

    /// <summary>Registers all commands in the given assembly.</summary>
    /// <param name="assembly">The assembly to register commands from.</param>
    /// <remarks>Use <see cref="RegisterAll(object)"/> or <see cref="RegisterAll(Type)"/> to also pull the registration attributes from a type.</remarks>
    public static void RegisterAll(Assembly assembly) => Create(assembly).Execute();

    #endregion

    #region Unregister

    /// <summary>Removes all registered commands in the assembly of the given object instance.</summary>
    /// <param name="assemblyMemberInstance">An object from the assembly.</param>
    public static void UnregisterAll(object assemblyMemberInstance) => UnregisterAll(assemblyMemberInstance.GetType());

    /// <summary>Removes all registered commands in the assembly of the given type.</summary>
    /// <param name="typeInAssembly">A type from the assembly.</param>
    public static void UnregisterAll(Type typeInAssembly) => UnregisterAll(typeInAssembly.Assembly);

    /// <summary>Removes all registered commands in the given assembly.</summary>
    /// <param name="assembly">The assembly to remove commands from.</param>
    public static void UnregisterAll(Assembly assembly)
    {
        UnregisterFromHandler(assembly, CommandProcessor.RemoteAdminCommandHandler);
        UnregisterFromHandler(assembly, GameCore.Console.singleton.ConsoleCommandHandler);
        UnregisterFromHandler(assembly, QueryProcessor.DotCommandHandler);
    }

    private static void UnregisterFromHandler(Assembly assembly, ICommandHandler handler)
    {
        foreach (var cmd in handler.AllCommands)
            if (cmd is CommandWrapper wrapper && wrapper.BackingCommand.GetType().Assembly == assembly)
                handler.UnregisterCommand(cmd);
    }

    #endregion

    #region Create

    /// <summary>Creates a new <see cref="CommandRegistrationProcessor"/> with the assembly of the given object instance.</summary>
    /// <param name="assemblyMemberInstance">An object from the assembly.</param>
    public static CommandRegistrationProcessor Create(object assemblyMemberInstance) => Create(assemblyMemberInstance.GetType().Assembly);

    /// <summary>Creates a new <see cref="CommandRegistrationProcessor"/> with the assembly of the given type.</summary>
    /// <param name="typeInAssembly">A type from the assembly.</param>
    public static CommandRegistrationProcessor Create(Type typeInAssembly) => Create(typeInAssembly.Assembly);

    /// <summary>Creates a new <see cref="CommandRegistrationProcessor"/> with the given assembly.</summary>
    /// <param name="assembly">The assembly to register commands from.</param>
    public static CommandRegistrationProcessor Create(Assembly assembly) => new(assembly);

    #endregion

    #region Fields

    /// <summary>The assembly to register commands from.</summary>
    public Assembly TargetAssembly { get; }

    private CommandRegistrationProcessor(Assembly assembly) => TargetAssembly = assembly;

    internal readonly List<ResolverContainer<ICommandNameResolver, string>> NameResolvers = new();

    internal readonly List<ResolverContainer<ICommandDescriptionResolver, string>> DescriptionResolvers = new();

    internal readonly List<ResolverContainer<ICommandAliasResolver, string[]>> AliasResolvers = new();

    internal readonly List<ResolverContainer<ICommandUsageResolver, string[]>> UsageResolvers = new();

    internal readonly List<ResolverContainer<IAttributeBasedPermissionCreator, IPermissionChecker>> PermissionCreators = new();

    internal readonly List<ResolverContainer<IAffectedMultiplePlayersResolver, IAffectedMultiplePlayersMessageGenerator>> TargetingMultipleMessageResolvers = new();

    internal readonly List<ResolverContainer<IAffectedOnePlayerResolver, IAffectedOnePlayerMessageGenerator>> TargetingSingleMessageResolvers = new();

    internal readonly List<ResolverContainer<IAffectedAllPlayersResolver, IAffectedAllPlayersMessageGenerator>> TargetingAllMessageResolvers = new();

    internal readonly List<ResolverContainer<ITargetSelectionResolver, ITargetSelectionManager>> TargetSelectionManagerResolvers = new();

    #endregion

    #region Exec

    /// <summary>Executes the processor, registering all commands and Remote Admin extensions in the assembly.</summary>
    public void Execute()
    {
        BaseCommandPropertyManager.CurrentProcessor = this;
        try
        {
            foreach (var type in TargetAssembly.GetTypes())
                if (!type.IsAbstract)
                    ProcessType(type);
        }
        catch (ReflectionTypeLoadException ex)
        {
            Log.Error("Failed to load types from assembly: \"" + TargetAssembly.FullName + "\"\nList of all exceptions:");
            foreach (var loaderException in ex.LoaderExceptions)
                Log.Error(loaderException.ToString());
        }
        finally
        {
            BaseCommandPropertyManager.CurrentProcessor = null;
        }
    }

    private static void ProcessType(Type type)
    {
        if (typeof(CommandBase).IsAssignableFrom(type))
            RegisterCommand(type);
        else if (typeof(RemoteAdminOptionBase).IsAssignableFrom(type))
            RemoteAdminOptionManager.RegisterOption((RemoteAdminOptionBase) Activator.CreateInstance(type));
    }

    private static void RegisterCommand(Type type)
    {
        var targets = CommandHandlerType.None;
        foreach (var attr in type.GetCustomAttributes())
            if (attr is CommandTargetAttribute targetAttribute)
                targets = CommandTargetAttribute.Combine(targets, targetAttribute);
        if (targets is CommandHandlerType.None)
        {
#if EXILED
            Log.Warn
#else
            Log.Warning
#endif
                ($"Type \"{type.FullName}\" extends CommandBase but does not specify the command handler types in its attributes.");
            return;
        }

        CreateWrapperAndRegister(type, targets);
    }

    private static void CreateWrapperAndRegister(Type commandBaseType, CommandHandlerType targets)
    {
        var wrapper = new CommandWrapper((CommandBase) Activator.CreateInstance(commandBaseType));
        if (targets.HasFlagFast(CommandHandlerType.RemoteAdmin))
            CommandProcessor.RemoteAdminCommandHandler.RegisterCommand(wrapper);
        if (targets.HasFlagFast(CommandHandlerType.ServerConsole))
            GameCore.Console.singleton.ConsoleCommandHandler.RegisterCommand(wrapper);
        if (targets.HasFlagFast(CommandHandlerType.Client))
            QueryProcessor.DotCommandHandler.RegisterCommand(wrapper);
    }

    #endregion

}
