using Axwabo.CommandSystem.Attributes.Targeting.Interfaces;

namespace Axwabo.CommandSystem.Commands.MessageOverrides;

/// <summary>
/// Manages target selection for <see cref="UnifiedTargetingCommand"/>.
/// </summary>
/// <seealso cref="IAffectedMultiplePlayersMessageGenerator"/>
/// <seealso cref="IAffectedAllPlayersMessageGenerator"/>
/// <seealso cref="IAffectedOnePlayerMessageGenerator"/>
public interface ITargetSelectionManager : IShouldAffectSpectators
{

    /// <summary>
    /// Determines whether everyone was affected based on the given player count.
    /// </summary>
    /// <param name="count">The number of players affected.</param>
    /// <returns>Whether everyone was affected.</returns>
    bool IsEveryoneAffected(int count);

}
