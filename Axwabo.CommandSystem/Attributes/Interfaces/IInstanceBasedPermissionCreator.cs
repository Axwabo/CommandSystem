using Axwabo.CommandSystem.Permissions;
using Axwabo.CommandSystem.RemoteAdminExtensions;

namespace Axwabo.CommandSystem.Attributes.Interfaces;

/// <summary>
/// An interface for attributes that create a permission checker based on the <see cref="CommandBase">command</see> or <see cref="RemoteAdminOptionBase">RA option</see> instance.
/// </summary>
public interface IInstanceBasedPermissionCreator
{

    /// <summary>
    /// Creates a permission checker based on the command instance.
    /// </summary>
    /// <param name="command">The command instance.</param>
    /// <returns>The permission checker.</returns>
    IPermissionChecker Create(CommandBase command);

    /// <summary>
    /// Creates a permission checker based on the RA option instance.
    /// </summary>
    /// <param name="option">The RA option instance.</param>
    /// <returns>The permission checker.</returns>
    IPermissionChecker Create(RemoteAdminOptionBase option);

}
