﻿using Axwabo.CommandSystem.Attributes;
using Axwabo.CommandSystem.Attributes.Containers;
using Axwabo.CommandSystem.Commands;
using Axwabo.CommandSystem.Commands.Interfaces;
using Axwabo.CommandSystem.Commands.MessageOverrides;
using Axwabo.CommandSystem.Commands.Wrappers;
using Axwabo.CommandSystem.Permissions;
using Axwabo.CommandSystem.PropertyManager.Resolvers;
using CommandSystem;
using Utils.NonAllocLINQ;
using Console = GameCore.Console;

namespace Axwabo.CommandSystem.Registration;

/// <summary>Handles the registration of commands.</summary>
public sealed class CommandRegistrationProcessor
{

    #region Auto-Register

    /// <summary>Registers all commands in the assembly of the given object instance with registration attributes on the type.</summary>
    /// <param name="assemblyMemberInstance">An object from the assembly.</param>
    public static void RegisterAll(object assemblyMemberInstance) => RegisterAll(assemblyMemberInstance.GetType());

    /// <summary>Registers all commands in the assembly of the given type with registration attributes on it.</summary>
    /// <param name="typeInAssembly">A type from the assembly.</param>
    public static void RegisterAll(Type typeInAssembly)
        => Create(typeInAssembly.Assembly)
            .WithTypesFromOriginalAssembly()
            .WithRegistrationAttributesFrom(typeInAssembly)
            .Execute();

    /// <summary>Registers all commands in the given assembly.</summary>
    /// <param name="assembly">The assembly to register commands from.</param>
    /// <remarks>Use <see cref="RegisterAll(object)"/> or <see cref="RegisterAll(Type)"/> to also pull the registration attributes from a type.</remarks>
    public static void RegisterAll(Assembly assembly) => Create(assembly)
        .WithTypesFromOriginalAssembly()
        .Execute();

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
        UnregisterFromHandler(assembly, Console.singleton.ConsoleCommandHandler);
        UnregisterFromHandler(assembly, QueryProcessor.DotCommandHandler);
        RemoteAdminOptionManager.UnregisterAll(assembly);
    }

    private static void UnregisterFromHandler(Assembly assembly, ICommandHandler handler)
    {
        foreach (var cmd in handler.AllCommands.ToList())
            if (cmd.TryGetUnderlyingCommand(out var underlyingCommand) && underlyingCommand.GetType().Assembly == assembly)
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
    public static void RegisterServerConsoleCommand(ICommand command) => Console.singleton.ConsoleCommandHandler.RegisterCommand(command);

    /// <summary>Registers a command to the <see cref="ClientCommandHandler"/>.</summary>
    /// <param name="command">The command to register.</param>
    public static void RegisterClientCommand(ICommand command) => QueryProcessor.DotCommandHandler.RegisterCommand(command);

    #endregion

    #region Fields

    /// <summary>The assembly to register commands from.</summary>
    public Assembly TargetAssembly { get; }

    /// <summary>The collection of types to be processed.</summary>
    public IReadOnlyCollection<Type> Types => _types;

    private CommandRegistrationProcessor(Assembly assembly) => TargetAssembly = assembly;

    private readonly HashSet<Type> _types = [];

    internal readonly List<ResolverContainer<ICommandNameResolver, string>> NameResolvers = [];

    internal readonly List<ResolverContainer<ICommandDescriptionResolver, string>> DescriptionResolvers = [];

    internal readonly List<ResolverContainer<ICommandAliasResolver, string[]>> AliasResolvers = [];

    internal readonly List<ResolverContainer<ICommandUsageResolver, string[]>> UsageResolvers = [];

    internal readonly List<ResolverContainer<IPlayerOnlyResolver, bool>> PlayerOnlyResolvers = [];

    internal readonly List<ResolverContainer<IAttributeBasedPermissionResolver, IPermissionChecker>> PermissionCreators = [];

    internal readonly List<ResolverContainer<IAffectedMultiplePlayersResolver, IAffectedMultiplePlayersMessageGenerator>> TargetingMultipleMessageResolvers = [];

    internal readonly List<ResolverContainer<IAffectedOnePlayerResolver, IAffectedOnePlayerMessageGenerator>> TargetingSingleMessageResolvers = [];

    internal readonly List<ResolverContainer<IAffectedAllPlayersResolver, IAffectedAllPlayersMessageGenerator>> TargetingAllMessageResolvers = [];

    internal readonly List<ResolverContainer<ITargetSelectionResolver, ITargetSelectionManager>> TargetSelectionManagerResolvers = [];

    internal readonly List<ResolverContainer<IResultCompilerResolver, ICustomResultCompiler>> TargetingResultCompilerResolvers = [];

    internal readonly List<ResolverContainer<IRemoteAdminOptionIdResolver, string>> RemoteAdminOptionIdResolvers = [];

    internal readonly List<ResolverContainer<IStaticOptionTextResolver, string>> StaticOptionTextResolvers = [];

    internal readonly List<ResolverContainer<IOptionIconResolver, BlinkingIcon>> OptionIconResolvers = [];

    #endregion

    #region Exec

    /// <summary>The current command registration processor.</summary>
    public static CommandRegistrationProcessor Current { get; private set; }

    private readonly Dictionary<Type, List<Type>> _subcommandsToRegister = new();

    private readonly Dictionary<Type, ContainerCommand> _registeredContainerCommands = new();

    private readonly Dictionary<Type, CommandHandlerType> _standaloneCommands = new();

    private readonly HashSet<Type> _skippedCommands = [];

    /// <summary>
    /// Adds the given type to be processed. The type must be from the same assembly as <see cref="TargetAssembly"/>, must be a class and must not be abstract. 
    /// </summary>
    /// <param name="type">The type to add.</param>
    /// <exception cref="ArgumentException">If the type is not from the same assembly, is abstract or is not a class.</exception>
    public void AddType(Type type)
    {
        if (type.Assembly != TargetAssembly)
            throw new ArgumentException($"Type must be from the same assembly as {nameof(TargetAssembly)}: {type}");
        if (type.IsAbstract)
            throw new ArgumentException($"Type must not be abstract: {type}");
        if (!type.IsClass)
            throw new ArgumentException($"Type must be a class: {type}");
        _types.Add(type);
    }

    /// <summary>The type of the command that is currently being registered.</summary>
    public Type RegisteringCommandType { get; private set; }

    /// <summary>Executes the processor, registering all commands and Remote Admin extensions in the assembly.</summary>
    public void Execute()
    {
        Current = this;
        try
        {
            foreach (var type in _types)
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
            Current = null;
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
            case SubcommandOfContainerAttribute subOf when !subOf.ContainerType.IsAbstract && typeof(ContainerCommand).IsAssignableFrom(subOf.ContainerType):
                isSubcommand = true;
                _subcommandsToRegister.GetOrAdd(subOf.ContainerType, () => []).Add(type);
                return false;
            case UsesSubcommandsAttribute {SubcommandTypes: {Length: not 0} types} when typeof(ContainerCommand).IsAssignableFrom(type):
                _subcommandsToRegister.GetOrAdd(type, () => [])
                    .AddRange(types.Where(t => !t.IsAbstract && typeof(CommandBase).IsAssignableFrom(t)));
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
        var option = Activator.CreateInstance(type);
        if (option is not IRegistrationFilter {AllowRegistration: false})
            RemoteAdminOptionManager.RegisterOption((RemoteAdminOptionBase) option);
    }

    private static void LogSkippedCommand(Type type)
    {
        if (type.Assembly != typeof(CommandRegistrationProcessor).Assembly)
            Log.Warn($"Type \"{type.FullName}\" extends CommandBase but does not specify the command handler types in its attributes.");
    }

    private void CreateCommandWrapperAndRegister(Type commandType, CommandHandlerType targets)
    {
        RegisteringCommandType = commandType;
        try
        {
            var commandBase = (CommandBase) Activator.CreateInstance(commandType);
            if (commandBase is IRegistrationFilter {AllowRegistration: false})
                return;
            if (commandBase is ContainerCommand container)
                _registeredContainerCommands.Add(commandType, container);
            var wrapper = new CommandWrapper(commandBase);
            if (targets.HasFlagFast(CommandHandlerType.RemoteAdmin))
                RegisterRemoteAdminCommand(wrapper);
            if (targets.HasFlagFast(CommandHandlerType.ServerConsole))
                RegisterServerConsoleCommand(wrapper);
            if (targets.HasFlagFast(CommandHandlerType.Client))
                RegisterClientCommand(wrapper);
        }
        finally
        {
            RegisteringCommandType = null;
        }
    }

    private void RegisterSubcommands()
    {
        foreach (var pair in _subcommandsToRegister)
        {
            foreach (var type in pair.Value)
                _skippedCommands.Remove(type);
            if (!_registeredContainerCommands.TryGetValue(pair.Key, out var container))
            {
                Log.Debug($"Container command of type \"{pair.Key.FullName}\" was not registered.\nDependent subcommands: {string.Join(", ", pair.Value.Select(t => t.FullName))}");
                continue;
            }

            _skippedCommands.Remove(pair.Key);
            foreach (var type in pair.Value)
                RegisterSubcommand(type, container);
        }
    }

    private void RegisterSubcommand(Type type, ContainerCommand container)
    {
        RegisteringCommandType = type;
        try
        {
            var commandBase = (CommandBase) Activator.CreateInstance(type);
            if (commandBase is not IRegistrationFilter {AllowRegistration: false})
                container.RegisterSubcommand(commandBase);
        }
        finally
        {
            RegisteringCommandType = null;
        }
    }

    #endregion

}
