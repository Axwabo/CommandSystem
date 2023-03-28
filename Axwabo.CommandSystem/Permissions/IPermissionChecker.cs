namespace Axwabo.CommandSystem.Permissions;

/// <summary>
/// An interface for checking permissions.
/// </summary>
public interface IPermissionChecker
{

    /// <summary>
    /// Determines whether the specified sender has the permissions determined by this instance.
    /// </summary>
    /// <param name="sender">The sender to check.</param>
    /// <returns>A <see cref="CommandResult"/> indicating whether the sender has the permissions. If the <see cref="CommandResult.Success"/> field is true, the check has passed.</returns>
    /// <seealso cref="CommandHelpers.CheckSafe"/>
    CommandResult CheckPermission(CommandSender sender);

}
