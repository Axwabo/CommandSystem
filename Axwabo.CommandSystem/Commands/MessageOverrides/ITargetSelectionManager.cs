namespace Axwabo.CommandSystem.Commands.MessageOverrides;

public interface ITargetSelectionManager {

    bool AffectSpectators { get; }

    bool IsEveryoneAffected(int count);

}
