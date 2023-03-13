namespace Axwabo.CommandSystem.Commands.MessageOverrides;

/// <summary>
/// A custom message generator that creates a message when a command affects all players.
/// </summary>
public interface IAffectedAllPlayersMessageGenerator
{

    /// <summary>
    /// Generates a message when a command affects all players.
    /// </summary>
    /// <param name="players">The number of players affected.</param>
    /// <returns>The message.</returns>
    string OnEveryoneAffected(int players);

}
