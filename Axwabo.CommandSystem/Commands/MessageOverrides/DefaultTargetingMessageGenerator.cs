namespace Axwabo.CommandSystem.Commands.MessageOverrides;

internal sealed class DefaultTargetingMessageGenerator :
    IAffectedMultiplePlayersMessageGenerator,
    IAffectedAllPlayersMessageGenerator,
    IAffectedOnePlayerMessageGenerator
{

    public const string DefaultAffectedMessage = "Done! The request affected {0}.";

    private readonly bool _affectedMultipleIsCustom;
    private readonly string _affectedMultipleMessage = DefaultAffectedMessage;

    public string AffectedMultipleMessage
    {
        get => _affectedMultipleMessage;
        init
        {
            _affectedMultipleMessage = value;
            _affectedMultipleIsCustom |= value is not DefaultAffectedMessage;
        }
    }

    public string AffectedOneMessage { get; init; } = DefaultAffectedMessage;

    public string OnAffected(int players) => string.Format(AffectedMultipleMessage, !_affectedMultipleIsCustom
        ? "players".PluralizeWithCount(players)
        : players.ToString());

    public string OnEveryoneAffected(int players) => OnAffected(players);

    public string OnAffected(ReferenceHub target) => string.Format(AffectedOneMessage, target.nicknameSync.MyNick);

}
