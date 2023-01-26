namespace Axwabo.CommandSystem.Permissions;

public interface IPermissionChecker {

    CommandResult CheckPermission(CommandSender sender);

}
