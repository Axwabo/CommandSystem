namespace Axwabo.CommandSystem.Permissions;

/// <summary>
/// A multiplex permission checker ensuring all permissions are sufficient.
/// </summary>
public sealed class CombinedPermissionChecker : IPermissionChecker
{

    /// <summary>The permission checker instances.</summary>
    public IPermissionChecker[] Instances { get; }

    /// <summary>
    /// Initializes an instance <see cref="CombinedPermissionChecker"/> instance.
    /// </summary>
    /// <param name="instances">The permission checker instances.</param>
    public CombinedPermissionChecker(params IPermissionChecker[] instances) => Instances = instances;

    /// <summary>
    /// Determines whether the specified sender has the permissions determined by all instances.
    /// </summary>
    /// <param name="sender">The sender to check.</param>
    /// <returns>A <see cref="CommandResult"/> indicating whether the sender has the permissions. If the <see cref="CommandResult.Success"/> field is true, the check has passed.</returns>
    /// <remarks>If one permission checker returns a result indicating insufficient permissions, the loop exits and returns that result as failing.</remarks>
    /// <seealso cref="IPermissionChecker.CheckPermission"/>
    public CommandResult CheckPermission(CommandSender sender)
    {
        foreach (var instance in Instances)
        {
            var result = instance.CheckPermission(sender);
            if (!result)
                return result;
        }

        return true;
    }

}
