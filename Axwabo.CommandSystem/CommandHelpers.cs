using System;
using System.Text;
using Axwabo.CommandSystem.Commands;
using Axwabo.CommandSystem.Permissions;
using Axwabo.CommandSystem.Structs;
using CommandSystem;
using NorthwoodLib.Pools;
using RemoteAdmin;
using IHiddenCommand = Axwabo.CommandSystem.Commands.Interfaces.IHiddenCommand;

namespace Axwabo.CommandSystem;

/// <summary>
/// Helper methods for commands.
/// </summary>
public static class CommandHelpers
{

    /// <summary>
    /// Gets the full path of the class implementing the command.
    /// </summary>
    /// <param name="command">The command to get the implementation location of.</param>
    /// <returns>The full implementation path.</returns>
    public static string GetImplementationLocation(ICommand command)
        => GetTypeInfo(command is CommandWrapper wrapper ? wrapper.BackingCommand.GetType() : command.GetType());

    /// <summary>
    /// Gets the assembly name combined with the type's full name.
    /// </summary>
    /// <param name="type">The type to get the info from.</param>
    /// <returns>The assembly name and type's full name.</returns>
    public static string GetTypeInfo(Type type) => type.Assembly.GetName().Name + ":" + type.FullName;

    /// <summary>
    /// Gets the usage of the command.
    /// </summary>
    /// <param name="command">The command to get the usage of.</param>
    /// <returns>The usage of the command.</returns>
    public static string GetUsage(ICommand command) => command switch
    {
        CommandWrapper {BackingCommand: { } bc} => bc.Usage is {Length: not 0} ? "\n" + bc.CombinedUsage : "",
        IUsageProvider {Usage.Length: not 0} usage => $"\nUsage: {command.Command} {usage.DisplayCommandUsage()}",
        _ => ""
    };

    /// <summary>
    /// Determines whether the command is hidden.
    /// </summary>
    /// <param name="command">The command to check.</param>
    /// <returns><see langword="true"/> if the command is hidden; otherwise, <see langword="false"/>.</returns>
    public static bool IsHidden(ICommand command) => command is CommandWrapper wrapper
        ? wrapper.BackingCommand is IHiddenCommand {IsHidden: true}
        : command is global::CommandSystem.IHiddenCommand;

    /// <summary>
    /// Casts the command sender to a <see cref="PlayerCommandSender"/> and returns its <see cref="ReferenceHub"/>.
    /// </summary>
    /// <param name="sender">The sender to get the <see cref="ReferenceHub"/> of.</param>
    /// <returns>The <see cref="ReferenceHub"/> of the sender.</returns>
    /// <exception cref="InvalidCastException">Thrown when the sender is not a <see cref="PlayerCommandSender"/>.</exception>
    public static ReferenceHub Hub(this CommandSender sender) => ((PlayerCommandSender) sender).ReferenceHub;

    /// <summary>
    /// Safely checks for permissions, given an <see cref="IPermissionChecker"/> and the <see cref="CommandSender"/>.
    /// </summary>
    /// <param name="checker">The permission checker to use.</param>
    /// <param name="sender">The sender to check the permissions of.</param>
    /// <returns>A <see cref="CommandResult"/> indicating whether the sender has the permissions. If the <see cref="CommandResult.Success"/> field is true, the check has passed.</returns>
    /// <seealso cref="IPermissionChecker.CheckPermission"/>
    public static CommandResult CheckSafe(this IPermissionChecker checker, CommandSender sender)
    {
        if (sender == null)
            return "!No sender was provided.";
        if (sender.FullPermissions || checker == null)
            return true;
        var result = checker.CheckPermission(sender);
        return !result && result.IsEmpty ? "!Insufficient permissions." : result;
    }

    /// <summary>
    /// Gets the full help string for the given command.
    /// </summary>
    /// <param name="command">The command to get help for.</param>
    /// <param name="arguments">The arguments that may determine subcommands.</param>
    /// <returns>The full help string.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="command"/> is <see langword="null"/>.</exception>
    public static string GetHelpForCommand(ICommand command, ArraySegment<string> arguments)
    {
        if (command == null)
            throw new ArgumentNullException(nameof(command));
        var args = arguments.Count < 2 ? new ArraySegment<string>(Array.Empty<string>()) : arguments.Segment(1);
        return command is CommandWrapper {BackingCommand: { } backingCommand}
            ? GetHelpForCustomCommand(backingCommand, args)
            : GetHelpForVanillaCommand(command, args);
    }

    /// <summary>
    /// Gets the help string for a vanilla <see cref="ICommand"/>.
    /// </summary>
    /// <param name="command">The command to get help for.</param>
    /// <param name="args">The arguments that may determine subcommands.</param>
    /// <returns>The help string.</returns>
    public static string GetHelpForVanillaCommand(ICommand command, ArraySegment<string> args)
    {
        var sb = StringBuilderPool.Shared.Rent();
        var str = command.Command;
        while (args.Count != 0 && command is ICommandHandler commandHandler && commandHandler.TryGetCommand(args.At(0), out var subcommand))
        {
            command = subcommand;
            str += " " + subcommand.Command;
            if (args.Count <= 1)
                break;
            args = args.Segment(1);
        }

        sb.Append(str + " - " + (command is IHelpProvider helpProvider ? helpProvider.GetHelp(args) : command.Description));
        if (command.Aliases is {Length: not 0})
            sb.AppendLine().Append("Aliases: " + string.Join(", ", command.Aliases));
        GetCommandList(command, "\nSubcommand list:", sb);
        sb.AppendLine(GetUsage(command));
        sb.Append("Implemented in: " + GetTypeInfo(command.GetType()));
        return StringBuilderPool.Shared.ToStringReturn(sb);
    }

    /// <summary>
    /// Gets the help string for a custom <see cref="CommandBase"/>.
    /// </summary>
    /// <param name="command">The command to get help for.</param>
    /// <param name="arguments">The arguments that may determine subcommands.</param>
    /// <returns>The help string.</returns>
    public static string GetHelpForCustomCommand(CommandBase command, ArraySegment<string> arguments)
    {
        var sb = StringBuilderPool.Shared.Rent();
        var commandName = command.Name;
        while (arguments.Count != 0 && command is ContainerCommand container && container.TryGetSubcommand(arguments.At(0), out var subcommand))
        {
            command = subcommand;
            commandName = commandName + " " + subcommand.Name;
            if (arguments.Count <= 1)
                break;
            arguments = arguments.Segment(1);
        }

        sb.Append(commandName + " - " + command.Description);
        if (command.Aliases is {Length: not 0})
            sb.AppendLine().Append("Aliases: " + string.Join(", ", command.Aliases));
        if (command is ContainerCommand containerCommand)
        {
            sb.AppendLine().Append("Subcommand list:");
            GetCustomCommandList(containerCommand, sb);
        }

        if (command.Usage is {Length: not 0})
            sb.AppendLine().Append(command.CombinedUsage);
        sb.AppendLine().Append("Implemented in: " + GetTypeInfo(command.GetType()));
        return StringBuilderPool.Shared.ToStringReturn(sb);
    }

    /// <summary>
    /// Gets the subcommand list of a command.
    /// </summary>
    /// <param name="command">The command to get the subcommands of.</param>
    /// <param name="header">The header to prepend the list with.</param>
    /// <param name="builder">The <see cref="StringBuilder"/> to append the list to.</param>
    public static void GetCommandList(ICommand command, string header, StringBuilder builder)
    {
        if (command is CommandWrapper {BackingCommand: ContainerCommand container})
        {
            builder.Append(header);
            GetCustomCommandList(container, builder);
            return;
        }

        if (command is not ICommandHandler handler)
            return;

        builder.Append(header);
        GetVanillaCommandList(handler, builder);
    }

    /// <summary>
    /// Gets the subcommand list of a vanilla <see cref="ICommandHandler"/>.
    /// </summary>
    /// <param name="handler">The command handler to get the subcommands of.</param>
    /// <param name="builder">The <see cref="StringBuilder"/> to append the list to.</param>
    public static void GetVanillaCommandList(ICommandHandler handler, StringBuilder builder)
    {
        foreach (var subcommand in handler.AllCommands)
        {
            if (IsHidden(subcommand))
                continue;
            builder.AppendLine();
            builder.Append("> ").Append(subcommand.Command).Append(" - ").Append(subcommand.Description);
            if (subcommand.Aliases is {Length: not 0})
                builder.Append(" - Aliases: ").Append(string.Join(", ", subcommand.Aliases));
        }
    }

    /// <summary>
    /// Gets the subcommand list of a custom <see cref="ContainerCommand"/>.
    /// </summary>
    /// <param name="container">The container command to get the subcommands of.</param>
    /// <param name="builder">The <see cref="StringBuilder"/> to append the list to.</param>
    public static void GetCustomCommandList(ContainerCommand container, StringBuilder builder)
    {
        foreach (var subcommand in container.AllSubcommands)
        {
            if (subcommand is IHiddenCommand {IsHidden: true})
                continue;
            builder.AppendLine();
            builder.Append("> ").Append(subcommand.Name).Append(" - ").Append(subcommand.Description);
            if (subcommand.Aliases is {Length: not 0})
                builder.Append(" - Aliases: ").Append(string.Join(", ", subcommand.Aliases));
        }
    }

}
