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

}
