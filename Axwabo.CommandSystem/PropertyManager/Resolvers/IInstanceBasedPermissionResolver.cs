using Axwabo.CommandSystem.Commands;
using Axwabo.CommandSystem.Permissions;

namespace Axwabo.CommandSystem.PropertyManager.Resolvers;

/// <summary>
/// An interface for attributes that create a permission checker based on the <see cref="CommandBase">command</see> or <see cref="RemoteAdminOptionBase">RA option</see> instance.
/// </summary>
public interface IInstanceBasedPermissionResolver
{

    /// <summary>
    /// Creates a permission checker based on the command instance.
    /// </summary>
    /// <param name="command">The command instance.</param>
    /// <returns>The permission checker.</returns>
    IPermissionChecker CreateFromCommand(CommandBase command);

    /// <summary>
    /// Creates a permission checker based on the RA option instance.
    /// </summary>
    /// <param name="option">The RA option instance.</param>
    /// <returns>The permission checker.</returns>
    IPermissionChecker CreateFromOption(RemoteAdminOptionBase option);

}
