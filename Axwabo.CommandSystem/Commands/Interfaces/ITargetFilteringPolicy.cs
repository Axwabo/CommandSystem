namespace Axwabo.CommandSystem.Commands.Interfaces;

/// <summary>
/// An interface used by <see cref="UnifiedTargetingCommand"/> to filter targets.
/// </summary>
public interface ITargetFilteringPolicy
{

    /// <summary>
    /// Returns whether the target should be allowed through the filter.
    /// </summary>
    /// <param name="hub">The target to check.</param>
    /// <returns>True if the player should count as a valid command target, false otherwise.</returns>
    bool FilterTarget(ReferenceHub hub);

}
