using Axwabo.CommandSystem.Extensions;

namespace Axwabo.CommandSystem.Commands.MessageOverrides;

/// <summary>
/// Implements all three affected players message generators.
/// </summary>
public sealed class DefaultTargetingMessageGenerator :
    IAffectedOnePlayerMessageGenerator,
    IAffectedMultiplePlayersMessageGenerator,
    IAffectedAllPlayersMessageGenerator
{

    /// <summary>The default format used in all methods.</summary>
    public const string DefaultAffectedMessage = "Done! The request affected {0}.";

    private readonly string _affectedOneMessage;

    private readonly string _affectedMultipleMessage;

    /// <summary>
    /// Creates a new <see cref="DefaultTargetingMessageGenerator"/> instance.
    /// </summary>
    /// <param name="affectedOneMessage">The message to format when one player was affected.</param>
    /// <param name="affectedMultipleMessage">The message to format when multiple players were affected.</param>
    public DefaultTargetingMessageGenerator(string affectedOneMessage, string affectedMultipleMessage)
    {
        _affectedMultipleMessage = affectedMultipleMessage ?? DefaultAffectedMessage;
        _affectedOneMessage = affectedOneMessage ?? DefaultAffectedMessage;
    }

    /// <inheritdoc />
    /// <returns>The affected one message formatted with the target's nickname.</returns>
    public string OnAffected(ReferenceHub target) => string.Format(_affectedOneMessage, target.nicknameSync.MyNick);

    /// <inheritdoc />
    /// <returns>The affected multiple message formatted with the amount of players. The amount is prepended with "player" or "players" if the format is <see cref="DefaultAffectedMessage"/>.</returns>
    public string OnAffected(int players) => string.Format(_affectedMultipleMessage, _affectedMultipleMessage is DefaultAffectedMessage
        ? "player".PluralizeWithCount(players)
        : players.ToString());

    /// <inheritdoc cref="OnAffected(int)" />
    public string OnEveryoneAffected(int players) => OnAffected(players);

}
