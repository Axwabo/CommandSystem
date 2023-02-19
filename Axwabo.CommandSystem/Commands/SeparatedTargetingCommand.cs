using System;
using System.Collections.Generic;
using Axwabo.CommandSystem.Commands.Interfaces;
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

        return succeeded.Count == 0 && failed.Count == 0
            ? CommandResult.Failed(NoPlayersAffected)
            : this is ICustomResultCompiler custom
                ? custom.CompileCustomResult(succeeded, failed)
                : CompileResult(succeeded);
    }

    protected abstract CommandResult ExecuteOn(ReferenceHub target, ArraySegment<string> arguments, CommandSender sender);

    protected CommandResult CompileResult(List<CommandResultOnTarget> success) {
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
