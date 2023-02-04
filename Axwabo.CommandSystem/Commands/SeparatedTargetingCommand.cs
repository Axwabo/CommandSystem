using System;
using System.Collections.Generic;
using Axwabo.CommandSystem.Selectors;
using Axwabo.CommandSystem.Structs;
using PlayerRoles;

namespace Axwabo.CommandSystem.Commands;

public abstract class SeparatedTargetingCommand : UnifiedTargetingCommand {

    protected sealed override CommandResult ExecuteOnTargets(List<ReferenceHub> targets, ArraySegment<string> arguments, CommandSender sender) {
        var succeeded = new List<CommandResultOnTarget>();
        var failed = new List<CommandResultOnTarget>();
        foreach (var target in targets) {
            if (DoNotAffectSpectators && !target.IsAlive())
                continue;
            var result = ExecuteOn(target, arguments, sender);
            if (result)
                succeeded.Add(new CommandResultOnTarget(target, result.Response));
            else
                failed.Add(new CommandResultOnTarget(target, result.Response, false));
        }

        return CompileResult(succeeded, failed);
    }

    protected sealed override CommandResult ExecuteOnSingleTarget(ReferenceHub target, ArraySegment<string> arguments, CommandSender sender)
        => base.ExecuteOnSingleTarget(target, arguments, sender);

    protected abstract CommandResult ExecuteOn(ReferenceHub target, ArraySegment<string> arguments, CommandSender sender);

    protected virtual CommandResult CompileResult(List<CommandResultOnTarget> success, List<CommandResultOnTarget> failures) {
        var affected = success.Count;
        return affected == 0
            ? CommandResult.Failed(NoPlayersAffected)
            : CommandResult.Succeeded(affected == 1
                ? GetAffectedMessageSingle(success[0].Target)
                : IsEveryoneAffected(affected)
                    ? GetAffectedMessageAll(affected)
                    : GetAffectedMessage(affected));
    }

    protected virtual string NoPlayersAffected => "No players were affected.";

    protected virtual bool DoNotAffectSpectators => false;

    protected virtual string GetAffectedMessage(int affected) => $"Done! The request affected {affected} {"player".Pluralize(affected)}.";

    protected virtual bool IsEveryoneAffected(int affected)
        => (DoNotAffectSpectators ? PlayerSelectionManager.NonSpectators : PlayerSelectionManager.AllPlayers).Count == affected;

    protected virtual string GetAffectedMessageAll(int affected) => GetAffectedMessage(affected);

    protected virtual string GetAffectedMessageSingle(ReferenceHub target) => $"Done! The request affected {target.nicknameSync.MyNick}.";

}
