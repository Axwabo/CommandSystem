using Axwabo.CommandSystem.Permissions;

namespace Axwabo.CommandSystem.Attributes.Interfaces;

public interface IGenericInstanceBasedPermissionCreator<in TCommand> where TCommand : CommandBase {

    IPermissionChecker Create(TCommand command);

}
