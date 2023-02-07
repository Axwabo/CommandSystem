namespace Axwabo.CommandSystem.Commands.MessageOverrides;

internal sealed class DefaultTargetingMessageGenerator : IAffectedMultiplePlayersMessageGenerator, IAffectedAllPlayersGenerator, IAffectedOnePlayerMessageGenerator {

    private readonly bool _affectedMultipleIsCustom;
    private readonly string _affectedMultipleMessage = "Done! The request affected {0}.";

    public string AffectedMultipleMessage {
        get => _affectedMultipleMessage;
        init {
            _affectedMultipleIsCustom = value == _affectedMultipleMessage;
            _affectedMultipleMessage = value;
        }
    }

    public string AffectedOneMessage { get; init; } = "Done! The request affected {0}.";

    public string OnAffected(int players) => string.Format(AffectedMultipleMessage, _affectedMultipleIsCustom ? "players".Pluralize(players) : players.ToString());

    public string OnEveryoneAffected(int players) => OnAffected(players);

    public string OnAffected(ReferenceHub target) => string.Format(AffectedOneMessage, target.nicknameSync.MyNick);

}
