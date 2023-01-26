using System;
using System.Collections.Generic;
using Utils;

namespace Axwabo.CommandSystem.Commands;

public abstract class UnifiedTargetingCommand : CommandBase {

    protected override int MinArguments => base.MinArguments + 1;

    protected sealed override CommandResult Execute(ArraySegment<string> arguments, CommandSender sender) {
        var targets = RAUtils.ProcessPlayerIdOrNamesList(arguments, 0, out var newArgs);
        return targets is not {Count: not 0}
            ? "!No targets were found."
            : ExecuteOnTargets(targets, (newArgs ?? Array.Empty<string>()).Segment(0), sender);
    }

    protected abstract CommandResult ExecuteOnTargets(List<ReferenceHub> targets, ArraySegment<string> arguments, CommandSender sender);

}
