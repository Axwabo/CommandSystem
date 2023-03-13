namespace Axwabo.CommandSystem.Commands.MessageOverrides;

/// <summary>
/// Manages target selection for <see cref="UnifiedTargetingCommand"/>.
/// </summary>
/// <seealso cref="IAffectedMultiplePlayersMessageGenerator"/>
/// <seealso cref="IAffectedAllPlayersMessageGenerator"/>
/// <seealso cref="IAffectedOnePlayerMessageGenerator"/>
public interface ITargetSelectionManager
{

    /// <summary>
    /// Whether the command should affect spectators.
    /// </summary>
    bool AffectSpectators { get; }

    /// <summary>
    /// Determines whether everyone was affected based on the given player count.
    /// </summary>
    /// <param name="count">The number of players affected.</param>
    /// <returns>Whether everyone was affected.</returns>
    bool IsEveryoneAffected(int count);

}
