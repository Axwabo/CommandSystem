using Axwabo.CommandSystem.Attributes;
using Axwabo.CommandSystem.Attributes.Containers;
using Axwabo.CommandSystem.Commands.Interfaces;
using Axwabo.CommandSystem.Commands.Wrappers;
using Axwabo.CommandSystem.PropertyManager;

namespace Axwabo.CommandSystem.Commands;

/// <summary>
/// A command encapsulating other subcommands.
/// </summary>
public abstract class ContainerCommand : CommandBase
{

    /// <summary>The list of subcommands.</summary>
    protected readonly List<CommandBase> Subcommands = new();

    /// <summary>The subcommands as a read-only list.</summary>
    public IReadOnlyList<CommandBase> AllSubcommands => Subcommands.AsReadOnly();

    /// <summary>Registers a subcommand by its type.</summary>
    /// <typeparam name="T">The type of the subcommand. Must have a default parameterless constructor.</typeparam>
    protected internal void RegisterSubcommand<T>() where T : CommandBase, new() => RegisterSubcommand(new T());

    /// <summary>
    /// Registers a subcommand instance.
    /// </summary>
    /// <param name="command">The command to register.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="command"/> is null.</exception>
    /// <exception cref="InvalidOperationException">Thrown if a subcommand with the same name already exists.</exception>
    protected internal void RegisterSubcommand(CommandBase command)
    {
        if (command == null)
            throw new ArgumentNullException(nameof(command));
        if (TryGetSubcommand(command.Name, out var existing, true))
            throw new InvalidOperationException($"Subcommand \"{command.Name}\" already exists in container command \"{GetType().FullName}\"; conflict with \"{existing.GetType().FullName}\"");
        Subcommands.Add(command);
    }

    /// <summary>
    /// Attempts to get a subcommand by name or alias.
    /// </summary>
    /// <param name="query">The name or alias to search for.</param>
    /// <param name="command">The found command, if any.</param>
    /// <param name="nameOnly">Whether to only search by name.</param>
    /// <returns>Whether a command was found.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="query"/> is null.</exception>
    /// <remarks>The method prioritizes commands by name.
    /// If no command by name is found, it will return the command by alias if any.
    /// If there is a command with an alias that matches another subcommand's name, the command by name will be returned.</remarks>
    public bool TryGetSubcommand(string query, out CommandBase command, bool nameOnly = false)
    {
        if (query == null)
            throw new ArgumentNullException(nameof(query));
        CommandBase byName = null;
        CommandBase byAlias = null;
        foreach (var cmd in Subcommands)
            if (cmd.Name.Equals(query, StringComparison.OrdinalIgnoreCase))
                byName = cmd;
            else if (cmd.Aliases.Contains(query, StringComparison.OrdinalIgnoreCase) && !nameOnly)
                byAlias = cmd;
        command = byName ?? byAlias;
        return command != null;
    }

    /// <inheritdoc />
    public override string[] Usage
    {
        get
        {
            var sub = UsagesOfSubcommands;
            var usage = base.Usage;
            if (usage is {Length: not 0})
                sub.InsertRange(0, usage);
            return sub.ToArray();
        }
    }

    /// <summary>Gets the usage list of all subcommands including command names.</summary>
    protected List<string> UsagesOfSubcommands
    {
        get
        {
            var list = new List<string>();
            foreach (var command in Subcommands)
            {
                if (command is IHiddenCommand {IsHidden: true})
                    continue;
                if (command.Usage is not {Length: not 0})
                    list.Add(command.Name);
                else
                    list.AddRange(command.Usage.Select(e => $"{command.Name} {e}"));
            }

            return list;
        }
    }

    private static bool ShouldRegisterSubcommand(MemberInfo methodInfo)
    {
        var isSubcommand = false;
        foreach (var attr in methodInfo.GetCustomAttributes())
        {
            if (attr is MethodBasedSubcommandAttribute)
                isSubcommand = true;
            if (attr is DoNotAutoRegister {AllowRegistration: false})
                return false;
        }

        return isSubcommand;
    }

    private static bool HasValidParameters(MethodInfo methodInfo, out bool isTargetingCommand)
    {
        var parameters = methodInfo.GetParameters();
        isTargetingCommand = false;
        if (methodInfo.ReturnType != typeof(CommandResult))
            return false;
        switch (parameters.Length)
        {
            case 2:
                return parameters[0].ParameterType == typeof(ArraySegment<string>) && parameters[1].ParameterType == typeof(CommandSender);
            case 3:
                isTargetingCommand = true;
                return parameters[0].ParameterType == typeof(List<ReferenceHub>)
                       && parameters[1].ParameterType == typeof(ArraySegment<string>)
                       && parameters[2].ParameterType == typeof(CommandSender);
            default:
                return false;
        }
    }

    /// <summary>
    /// Creates a new <see cref="ContainerCommand"/> instance.
    /// </summary>
    protected ContainerCommand()
    {
        var thisType = GetType();
        foreach (var methodInfo in thisType.GetMethods(BindingFlags.Public | BindingFlags.Instance))
        {
            if (!ShouldRegisterSubcommand(methodInfo))
                continue;
            if (HasValidParameters(methodInfo, out var isTargetingCommand))
                RegisterMethodBasedSubcommand(methodInfo, isTargetingCommand);
            else
                Log.Warn($"Method \"{methodInfo.Name}\" in type \"{thisType.FullName}\" is marked as a subcommand but does not match the signature of a command method. Ignoring registration.");
        }
    }

    private void RegisterMethodBasedSubcommand(MethodInfo methodInfo, bool isTargetingCommand)
    {
        if (!BaseCommandPropertyManager.TryResolveProperties(null, out var name, out var description, out var aliases, out var usage, out var minArguments, out var playerOnly, methodInfo))
            name = methodInfo.Name;
        var permissions = BaseCommandPropertyManager.ResolvePermissionChecker(this, methodInfo);
        if (isTargetingCommand)
            MethodBasedTargetingCommand.SetNextCommandName(name);
        else
            MethodBasedCommand.SetNextCommandName(name);
        RegisterSubcommand(
            isTargetingCommand
                ? new MethodBasedTargetingCommand(description, aliases, usage, minArguments, permissions, playerOnly, methodInfo, this)
                : new MethodBasedCommand(description, aliases, usage, minArguments, permissions, playerOnly, methodInfo, this)
        );
    }

    /// <summary>
    /// Attempts to execute a subcommand, or the container command if no subcommand is found.
    /// </summary>
    /// <param name="arguments">The arguments passed to the command.</param>
    /// <param name="sender">The sender of the command.</param>
    /// <returns>The result of execution.</returns>
    /// <seealso cref="ExecuteContainer"/>
    protected override CommandResult Execute(ArraySegment<string> arguments, CommandSender sender)
        => arguments.Count != 0 && TryGetSubcommand(arguments.At(0), out var subcommand)
            ? subcommand.ExecuteBase(arguments.Segment(1), sender)
            : ExecuteContainer(arguments, sender);

    /// <summary>
    /// Called when no subcommand is found. This method is NOT min-argument checked by default and may be invoked an empty argument array!
    /// </summary>
    /// <param name="arguments">The arguments passed to the command.</param>
    /// <param name="sender">The sender of the command.</param>
    /// <returns>The result of execution.</returns>
    protected virtual CommandResult ExecuteContainer(ArraySegment<string> arguments, CommandSender sender) => $"!Unknown subcommand! {CombinedUsage}";

}
