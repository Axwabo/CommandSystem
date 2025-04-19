namespace Axwabo.CommandSystem.Permissions;

/// <summary>A permission checker encapsulating multiple other <see cref="IPermissionChecker"/> instances.</summary>
public interface IMultiplexPermissionChecker : IPermissionChecker
{

    /// <summary>The permission checker instances.</summary>
    IReadOnlyList<IPermissionChecker> Instances { get; }

}
