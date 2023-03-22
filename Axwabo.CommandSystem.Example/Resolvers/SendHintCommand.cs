using System;
using System.Collections.Generic;
using Axwabo.CommandSystem.Attributes;
using Axwabo.CommandSystem.Commands;
using Axwabo.CommandSystem.Structs;
using Hints;

namespace Axwabo.CommandSystem.Example.Resolvers;

[EnumCommand(CustomCommandType.SendHint)]
[MinArguments(2)]
[Usage("duration ...message")]
public sealed class SendHintCommand : UnifiedTargetingCommand
{

    protected override CommandResult ExecuteOnTargets(List<ReferenceHub> targets, ArraySegment<string> arguments, CommandSender sender)
    {
        if (!float.TryParse(arguments.At(0), out var duration))
            return "!Invalid duration.";
        var content = string.Join(" ", arguments.Segment(1));
        var hint = new TextHint(content, new HintParameter[]
        {
            new StringHintParameter(content)
        }, durationScalar: duration);
        foreach (var hub in targets)
            hub.hints.Show(hint);
        return $"Hint sent to {"player".PluralizeWithCount(targets.Count)}";
    }

}
