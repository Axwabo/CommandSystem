namespace Axwabo.CommandSystem.Commands.MessageOverrides;

/// <summary>
/// A custom message generator that creates a message when a command one player.
/// </summary>
public interface IAffectedOnePlayerMessageGenerator
{

    /// <summary>
    /// Generates a message when a command affects a single player.
    /// </summary>
    /// <param name="target">The affected player.</param>
    /// <returns>The message.</returns>
    string OnAffected(ReferenceHub target);

}
