using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Axwabo.CommandSystem.Attributes;
using Axwabo.CommandSystem.Attributes.Parenting;
using Axwabo.CommandSystem.Commands.MessageOverrides;
using Axwabo.CommandSystem.Permissions;
using Axwabo.CommandSystem.PropertyManager;
using Axwabo.CommandSystem.PropertyManager.Resolvers;
using Axwabo.CommandSystem.RemoteAdminExtensions;
using CommandSystem;
using RemoteAdmin;
using Utils.NonAllocLINQ;

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

    #region Add to Handler

    /// <summary>Registers a command to the <see cref="RemoteAdminCommandHandler"/>.</summary>
    /// <param name="command">The command to register.</param>
    public static void RegisterRemoteAdminCommand(ICommand command) => CommandProcessor.RemoteAdminCommandHandler.RegisterCommand(command);

    /// <summary>Registers a command to the <see cref="GameConsoleCommandHandler"/>.</summary>
    /// <param name="command">The command to register.</param>
    public static void RegisterServerConsoleCommand(ICommand command) => GameCore.Console.singleton.ConsoleCommandHandler.RegisterCommand(command);

    /// <summary>Registers a command to the <see cref="ClientCommandHandler"/>.</summary>
    /// <param name="command">The command to register.</param>
    public static void RegisterClientCommand(ICommand command) => QueryProcessor.DotCommandHandler.RegisterCommand(command);

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

    internal readonly List<ResolverContainer<IRemoteAdminOptionIdResolver, string>> RemoteAdminOptionIdResolvers = new();

    internal readonly List<ResolverContainer<IStaticOptionTextResolver, string>> StaticOptionTextResolvers = new();

    internal readonly List<ResolverContainer<IOptionIconResolver, BlinkingIcon>> OptionIconResolvers = new();

    #endregion

    #region Exec

    private readonly Dictionary<Type, List<Type>> _subcommandsToRegister = new();

    private readonly Dictionary<Type, Axwabo.CommandSystem.Commands.ParentCommand> _registeredParentCommands = new();

    private readonly Dictionary<Type, CommandHandlerType> _standaloneCommands = new();

    private readonly HashSet<Type> _skippedCommands = new();

    /// <summary>Executes the processor, registering all commands and Remote Admin extensions in the assembly.</summary>
    public void Execute()
    {
        BaseCommandPropertyManager.CurrentProcessor = this;
        try
        {
            foreach (var type in TargetAssembly.GetTypes())
                if (!type.IsAbstract)
                    ProcessType(type);
            foreach (var pair in _standaloneCommands)
                CreateCommandWrapperAndRegister(pair.Key, pair.Value);
            RegisterSubcommands();
            _skippedCommands.ForEach(LogSkippedCommand);
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

    private void ProcessType(Type type)
    {
        if (typeof(RemoteAdminOptionBase).IsAssignableFrom(type))
            RegisterOption(type);
        if (!typeof(CommandBase).IsAssignableFrom(type))
            return;
        var targets = CommandHandlerType.None;
        var isSubcommand = false;
        foreach (var attr in type.GetCustomAttributes())
            if (ProcessCommandAttribute(type, attr, ref isSubcommand, ref targets))
                return;
        if (isSubcommand)
            return;
        if (targets != CommandHandlerType.None)
            _standaloneCommands.Add(type, targets);
        else
            _skippedCommands.Add(type);
    }

    private bool ProcessCommandAttribute(Type type, Attribute attr, ref bool isSubcommand, ref CommandHandlerType targets)
    {
        switch (attr)
        {
            case IRegistrationFilter {AllowRegistration: false}:
                return true;
            case SubcommandOfParentAttribute subOf when typeof(Axwabo.CommandSystem.Commands.ParentCommand).IsAssignableFrom(subOf.ParentType):
                isSubcommand = true;
                _subcommandsToRegister.GetOrAdd(subOf.ParentType, () => new List<Type>()).Add(type);
                return false;
            case UsesSubcommandAttribute usesSub when typeof(Axwabo.CommandSystem.Commands.ParentCommand).IsAssignableFrom(type):
                _subcommandsToRegister.GetOrAdd(type, () => new List<Type>()).Add(usesSub.SubcommandType);
                return false;
            case CommandTargetAttribute targetAttribute:
                targets = CommandTargetAttribute.Combine(targets, targetAttribute);
                return false;
            default:
                return false;
        }
    }

    private static void RegisterOption(Type type)
    {
        foreach (var attr in type.GetCustomAttributes())
            if (attr is IRegistrationFilter {AllowRegistration: false})
                return;
        RemoteAdminOptionManager.RegisterOption((RemoteAdminOptionBase) Activator.CreateInstance(type));
    }

    private static void LogSkippedCommand(Type type) => Log.Warn($"Type \"{type.FullName}\" extends CommandBase but does not specify the command handler types in its attributes.");

    private void CreateCommandWrapperAndRegister(Type commandType, CommandHandlerType targets)
    {
        var commandBase = (CommandBase) Activator.CreateInstance(commandType);
        if (commandBase is IRegistrationFilter {AllowRegistration: false})
            return;
        if (commandBase is Axwabo.CommandSystem.Commands.ParentCommand parent)
            _registeredParentCommands.Add(commandType, parent);
        var wrapper = new CommandWrapper(commandBase);
        if (targets.HasFlagFast(CommandHandlerType.RemoteAdmin))
            RegisterRemoteAdminCommand(wrapper);
        if (targets.HasFlagFast(CommandHandlerType.ServerConsole))
            RegisterServerConsoleCommand(wrapper);
        if (targets.HasFlagFast(CommandHandlerType.Client))
            RegisterClientCommand(wrapper);
    }

    private void RegisterSubcommands()
    {
        foreach (var pair in _subcommandsToRegister)
        {
            foreach (var type in pair.Value)
                _skippedCommands.Remove(type);
            if (!_registeredParentCommands.TryGetValue(pair.Key, out var parent))
            {
                Log.Debug($"Parent command of type \"{pair.Key.FullName}\" was not registered.\nDependent subcommands: {string.Join(", ", pair.Value.Select(t => t.FullName))}");
                continue;
            }

            _skippedCommands.Remove(pair.Key);
            foreach (var type in pair.Value)
            {
                var commandBase = (CommandBase) Activator.CreateInstance(type);
                if (commandBase is not IRegistrationFilter {AllowRegistration: false})
                    parent.RegisterSubcommand(commandBase);
            }
        }
    }

    #endregion

}
