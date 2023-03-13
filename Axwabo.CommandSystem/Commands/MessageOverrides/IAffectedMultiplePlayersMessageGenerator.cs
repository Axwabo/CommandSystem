namespace Axwabo.CommandSystem.Commands.MessageOverrides;

/// <summary>
/// A custom message generator that creates a message when a command affects multiple players.
/// </summary>
/// <seealso cref="UnifiedTargetingCommand"/>
/// <seealso cref="IAffectedAllPlayersMessageGenerator"/>
/// <seealso cref="IAffectedOnePlayerMessageGenerator"/>
public interface IAffectedMultiplePlayersMessageGenerator
{

    /// <summary>
    /// Generates a message when a command affects multiple players.
    /// </summary>
    /// <param name="players">The number of players affected.</param>
    /// <returns>The message.</returns>
    string OnAffected(int players);

}
