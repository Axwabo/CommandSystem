using System;
using Axwabo.CommandSystem.Permissions;
using Axwabo.CommandSystem.Structs;
using CommandSystem;
using RemoteAdmin;

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
        CommandWrapper {BackingCommand: var bc} => bc.Usage is {Length: not 0} ? "\n" + bc.CombinedUsage : "",
        IUsageProvider {Usage.Length: not 0} usage => $"\nUsage: {command.Command} {usage.DisplayCommandUsage()}",
        _ => ""
    };

    /// <summary>
    /// Determines whether the command is hidden.
    /// </summary>
    /// <param name="command">The command to check.</param>
    /// <returns><see langword="true"/> if the command is hidden; otherwise, <see langword="false"/>.</returns>
    public static bool IsHidden(ICommand command) => command is CommandWrapper wrapper
        ? wrapper.BackingCommand is Commands.Interfaces.IHiddenCommand {IsHidden: true}
        : command is IHiddenCommand;

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

}
