using Axwabo.CommandSystem.Permissions;

namespace Axwabo.CommandSystem.Attributes.Interfaces;

/// <summary>
/// An interface for attributes that create a permission checker based on the RA option instance.
/// </summary>
/// <typeparam name="TOption">The option type.</typeparam>
public interface IGenericRemoteAdminOptionInstanceBasedPermissionResolver<in TOption> where TOption : RemoteAdminOptionBase
{

    /// <summary>
    /// Creates a permission checker based on the RA option instance.
    /// </summary>
    /// <param name="option">The option instance.</param>
    /// <returns>The permission checker.</returns>
    IPermissionChecker Create(TOption option);

}
