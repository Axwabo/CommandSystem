using System;
using System.Collections.Generic;
using Axwabo.CommandSystem.Structs;

namespace Axwabo.CommandSystem.Commands;

public abstract class SeparatedTargetingCommand : UnifiedTargetingCommand {

    protected override CommandResult ExecuteOnTargets(List<ReferenceHub> targets, ArraySegment<string> arguments, CommandSender sender) {
        var succeeded = new List<CommandResultOnTarget>();
        var failed = new List<CommandResultOnTarget>();
        foreach (var target in targets) {
            var result = ExecuteOn(target, arguments, sender);
            if (result)
                succeeded.Add(new CommandResultOnTarget(target, result.Response));
            else
                failed.Add(new CommandResultOnTarget(target, result.Response, false));
        }

        if (succeeded.Count == 0 && failed.Count == 0)
            return CommandResult.Failed(NoPlayersAffected);
        return CompileResult(succeeded, failed);
    }

    protected abstract CommandResult ExecuteOn(ReferenceHub target, ArraySegment<string> arguments, CommandSender sender);

    protected virtual CommandResult CompileResult(List<CommandResultOnTarget> success, List<CommandResultOnTarget> failures) {
        var affected = success.Count;
        return affected == 0
            ? CommandResult.Failed(NoPlayersAffected)
            : CommandResult.Succeeded(
                affected == 1
                    ? GetAffectedMessageSingle(success[0].Target)
                    : IsEveryoneAffected(affected)
                        ? GetAffectedMessageAll(affected)
                        : GetAffectedMessage(affected)
            );
    }

}
