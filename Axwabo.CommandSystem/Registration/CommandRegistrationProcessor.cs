using System;
using System.Collections.Generic;
using System.Reflection;
using Axwabo.CommandSystem.Attributes;
using Axwabo.CommandSystem.Commands.MessageOverrides;
using Axwabo.CommandSystem.Permissions;
using Axwabo.CommandSystem.PropertyManager;
using Axwabo.CommandSystem.PropertyManager.Resolvers;
using CommandSystem;
using PluginAPI.Core;
using RemoteAdmin;

namespace Axwabo.CommandSystem.Registration;

public sealed class CommandRegistrationProcessor {

    #region Auto-Register

    public static void RegisterAll(object assemblyMemberInstance) => RegisterAll(assemblyMemberInstance.GetType());

    public static void RegisterAll(Type typeInAssembly)
        => new CommandRegistrationProcessor(typeInAssembly.Assembly)
            .WithRegistrationAttributesFrom(typeInAssembly)
            .Execute();

    public static void RegisterAll(Assembly assembly) => new CommandRegistrationProcessor(assembly).Execute();

    #endregion

    #region Unregister

    public static void UnregisterAll(object assemblyMemberInstance) => UnregisterAll(assemblyMemberInstance.GetType());

    public static void UnregisterAll(Type typeInAssembly) => UnregisterAll(typeInAssembly.Assembly);

    public static void UnregisterAll(Assembly assembly) {
        UnregisterFromHandler(assembly, CommandProcessor.RemoteAdminCommandHandler);
        UnregisterFromHandler(assembly, GameCore.Console.singleton.ConsoleCommandHandler);
        UnregisterFromHandler(assembly, QueryProcessor.DotCommandHandler);
    }

    private static void UnregisterFromHandler(Assembly assembly, ICommandHandler handler) {
        foreach (var cmd in handler.AllCommands)
            if (cmd is CommandWrapper wrapper && wrapper.BackingCommand.GetType().Assembly == assembly)
                handler.UnregisterCommand(cmd);
    }

    #endregion

    #region Create

    public static CommandRegistrationProcessor Create(object assemblyMemberInstance) => Create(assemblyMemberInstance.GetType().Assembly);

    public static CommandRegistrationProcessor Create(Type typeInAssembly) => Create(typeInAssembly.Assembly);

    public static CommandRegistrationProcessor Create(Assembly assembly) => new(assembly);

    #endregion

    #region Fields

    public Assembly TargetAssembly { get; }

    private CommandRegistrationProcessor(Assembly assembly) => TargetAssembly = assembly;

    internal readonly List<ResolverContainer<ICommandNameResolver, string>> NameResolvers = new();

    internal readonly List<ResolverContainer<ICommandDescriptionResolver, string>> DescriptionResolvers = new();

    internal readonly List<ResolverContainer<ICommandAliasResolver, string[]>> AliasResolvers = new();

    internal readonly List<ResolverContainer<ICommandUsageResolver, string[]>> UsageResolvers = new();

    internal readonly List<ResolverContainer<ICommandPermissionCreator, IPermissionChecker>> PermissionCreators = new();

    internal readonly List<ResolverContainer<IAffectedMultiplePlayersResolver, IAffectedMultiplePlayersMessageGenerator>> TargetingMultipleMessageResolvers = new();

    internal readonly List<ResolverContainer<IAffectedOnePlayerResolver, IAffectedOnePlayerMessageGenerator>> TargetingSingleMessageResolvers = new();

    internal readonly List<ResolverContainer<IAffectedAllPlayersResolver, IAffectedAllPlayersGenerator>> TargetingAllMessageResolvers = new();

    internal readonly List<ResolverContainer<ITargetSelectionResolver, ITargetSelectionManager>> TargetSelectionManagerResolvers = new();

    #endregion

    #region Exec

    public void Execute() {
        BaseCommandPropertyManager.CurrentProcessor = this;
        try {
            foreach (var type in TargetAssembly.GetTypes())
                if (!type.IsAbstract && typeof(CommandBase).IsAssignableFrom(type))
                    RegisterCommand(type);
        } catch (ReflectionTypeLoadException ex) {
            Log.Error("Failed to load types from assembly: \"" + TargetAssembly.FullName + "\"\nList of all exceptions:");
            foreach (var loaderException in ex.LoaderExceptions)
                Log.Error(loaderException.ToString());
        } finally {
            BaseCommandPropertyManager.CurrentProcessor = null;
        }
    }

    private static void RegisterCommand(Type type) {
        var targets = CommandHandlerType.None;
        foreach (var attr in type.GetCustomAttributes())
            if (attr is CommandTargetAttribute targetAttribute)
                targets = CommandTargetAttribute.Combine(targets, targetAttribute);
        if (targets is CommandHandlerType.None) {
            Log.Warning($"Type \"{type.FullName}\" extends CommandBase but does not specify the command handler types in its attributes.");
            return;
        }

        var wrapper = new CommandWrapper((CommandBase) Activator.CreateInstance(type));
        if (targets.HasFlagFast(CommandHandlerType.RemoteAdmin))
            CommandProcessor.RemoteAdminCommandHandler.RegisterCommand(wrapper);
        if (targets.HasFlagFast(CommandHandlerType.ServerConsole))
            GameCore.Console.singleton.ConsoleCommandHandler.RegisterCommand(wrapper);
        if (targets.HasFlagFast(CommandHandlerType.Client))
            QueryProcessor.DotCommandHandler.RegisterCommand(wrapper);
    }

    #endregion

}
