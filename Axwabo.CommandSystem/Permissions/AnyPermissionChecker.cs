using Axwabo.CommandSystem.Commands;
using Axwabo.CommandSystem.Extensions;

namespace Axwabo.CommandSystem.Permissions;

/// <summary>
/// A permission checker allowing usage if any sub-checkers pass.
/// </summary>
public sealed class AnyPermissionChecker : IMultiplexPermissionChecker
{

    /// <inheritdoc />
    public IReadOnlyList<IPermissionChecker> Instances { get; }

    /// <summary>
    /// Initializes an <see cref="AnyPermissionChecker"/> instance.
    /// </summary>
    /// <param name="instances">The permission checker instances.</param>
    public AnyPermissionChecker(params IPermissionChecker[] instances) : this(instances.AsEnumerable())
    {
    }

    /// <summary>
    /// Initializes an <see cref="AnyPermissionChecker"/> instance.
    /// </summary>
    /// <param name="instances">The permission checker instances.</param>
    public AnyPermissionChecker(IEnumerable<IPermissionChecker> instances)
        => Instances = instances.Flatten<AnyPermissionChecker>().ToList().AsReadOnly();

    /// <summary>
    /// Determines whether the specified sender has permissions determined by any instance.
    /// </summary>
    /// <param name="sender">The sender to check.</param>
    /// <returns>A <see cref="CommandResult"/> indicating whether the sender has the permissions. If the <see cref="CommandResult.Success"/> field is true, the check has passed.</returns>
    /// <remarks>If a permission checker returns a result indicating sufficient permissions, the loop exits and returns that result as successful.</remarks>
    /// <seealso cref="IPermissionChecker.CheckPermission"/>
    public CommandResult CheckPermission(CommandSender sender)
    {
        foreach (var instance in Instances)
        {
            var result = instance.CheckPermission(sender);
            if (result)
                return result;
        }

        return false;
    }

}
