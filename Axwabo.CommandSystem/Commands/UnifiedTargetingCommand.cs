using System;
using System.Collections.Generic;
using Axwabo.CommandSystem.Structs;

namespace Axwabo.CommandSystem.Commands;

public abstract class UnifiedTargetingCommand : CommandBase {

    protected override int MinArguments => base.MinArguments + 1;

    protected virtual string NoTargetsFound => "No targets were found.";

    protected sealed override CommandResult Execute(ArraySegment<string> arguments, CommandSender sender) {
        var targets = arguments.GetTargets(out var newArgs);
        if (targets is not {Count: not 0})
            return CommandResult.Failed(NoTargetsFound);
        var args = (newArgs ?? Array.Empty<string>()).Segment(0);
        return targets.Count == 1 ? ExecuteOnSingleTarget(targets[0], arguments, sender) : ExecuteOnTargets(targets, args, sender);
    }

    protected abstract CommandResult ExecuteOnTargets(List<ReferenceHub> targets, ArraySegment<string> arguments, CommandSender sender);

    protected virtual CommandResult ExecuteOnSingleTarget(ReferenceHub target, ArraySegment<string> arguments, CommandSender sender)
        => ExecuteOnTargets(new List<ReferenceHub> {target}, arguments, sender);

}
