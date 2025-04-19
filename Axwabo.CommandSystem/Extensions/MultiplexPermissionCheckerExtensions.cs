using Axwabo.CommandSystem.Permissions;

namespace Axwabo.CommandSystem.Extensions;

/// <summary>Extension methods for working with <see cref="IMultiplexPermissionChecker"/>s.</summary>
public static class MultiplexPermissionCheckerExtensions
{

    /// <summary>
    /// Flattens the enumerable to replace <typeparamref name="T"/> multiplex sub-checkers with its contained instances.
    /// </summary>
    /// <param name="instances">The instances to flatten.</param>
    /// <typeparam name="T">The type containing sub-checkers to flatten.</typeparam>
    /// <returns>An enumerable with <typeparamref name="T"/>'s sub-checkers extracted.</returns>
    public static IEnumerable<IPermissionChecker> Flatten<T>(this IEnumerable<IPermissionChecker> instances) where T : IMultiplexPermissionChecker
    {
        foreach (var checker in instances)
        {
            if (checker is not T t)
            {
                yield return checker;
                continue;
            }

            foreach (var instance in t.Instances)
                yield return instance;
        }
    }

    /// <summary>
    /// Aggregates the permission list to an <see cref="AllPermissionChecker"/> if applicable, ensuring that all permissions are sufficient.
    /// </summary>
    /// <param name="permissions">The permissions to combine.</param>
    /// <returns>
    /// <list type="bullet">
    /// <listheader><term>An <see cref="IPermissionChecker"/> based on the list's contents:</term></listheader>
    /// <item><description><see langword="null"/> if the list is empty.</description></item>
    /// <item><description>The sole <see cref="IPermissionChecker"/> in the list if it only contains 1 item.</description></item>
    /// <item><description>An <see cref="AllPermissionChecker"/> with all items of the list if its count is greater than 1.</description></item>
    /// </list>
    /// </returns>
    public static IPermissionChecker CheckAll(this IReadOnlyList<IPermissionChecker> permissions) => permissions.Count switch
    {
        0 => null,
        1 => permissions[0],
        _ => new AllPermissionChecker(permissions)
    };

    /// <summary>
    /// Aggregates the permission list to an <see cref="AnyPermissionChecker"/> if applicable, letting any checker allow the check to pass.
    /// </summary>
    /// <param name="permissions">The permissions to combine.</param>
    /// <returns>
    /// <list type="bullet">
    /// <listheader><term>An <see cref="IPermissionChecker"/> based on the list's contents:</term></listheader>
    /// <item><description><see langword="null"/> if the list is empty.</description></item>
    /// <item><description>The sole <see cref="IPermissionChecker"/> in the list if it only contains 1 item.</description></item>
    /// <item><description>An <see cref="AnyPermissionChecker"/> with all items of the list if its count is greater than 1.</description></item>
    /// </list>
    /// </returns>
    public static IPermissionChecker CheckAny(this IReadOnlyList<IPermissionChecker> permissions) => permissions.Count switch
    {
        0 => null,
        1 => permissions[0],
        _ => new AnyPermissionChecker(permissions)
    };

}
