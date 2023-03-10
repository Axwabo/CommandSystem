using Axwabo.CommandSystem.Permissions;

namespace Axwabo.CommandSystem.Attributes.Interfaces;

/// <summary>
/// An interface for attributes that create a permission checker based on the command instance.
/// </summary>
/// <typeparam name="TCommand">The command type.</typeparam>
public interface IGenericInstanceBasedPermissionCreator<in TCommand> where TCommand : CommandBase {

    /// <summary>
    /// Creates a permission checker based on the command instance.
    /// </summary>
    /// <param name="command">The command instance.</param>
    /// <returns>The permission checker.</returns>
    IPermissionChecker Create(TCommand command);

}
