using Axwabo.CommandSystem.Permissions;

namespace Axwabo.CommandSystem.Attributes.Interfaces;

public interface IInstanceBasedPermissionCreator {

    IPermissionChecker Create(CommandBase command);

}
