using System;
using System.Collections.Generic;
using Axwabo.CommandSystem.Attributes;
using Axwabo.CommandSystem.Commands;
using Axwabo.CommandSystem.Commands.Interfaces;
using Axwabo.Helpers;
using Hints;

namespace Axwabo.CommandSystem.Example.Resolvers;

[EnumCommand(CustomCommandType.SendHint)]
[MinArguments(2)]
[Usage("duration ...message")]
public sealed class SendHintCommand : SeparatedTargetingCommand, ITargetingPreExecutionFilter
{

    private TextHint _hint;

    public CommandResult? OnBeforeExecuted(List<ReferenceHub> targets, ArraySegment<string> arguments, CommandSender sender)
    {
        // make sure the duration is 0 or higher
        if (!arguments.ParseFloat(ValueRange<float>.StartOnly(0), out var duration))
            return "!Invalid duration - must be 0 or greater.";
        var content = arguments.Join(1);
        _hint = new TextHint(content, new HintParameter[]
        {
            new StringHintParameter(content)
        }, durationScalar: duration);
        return CommandResult.Null;
    }

    protected override CommandResult ExecuteOn(ReferenceHub target, ArraySegment<string> arguments, CommandSender sender)
    {
        target.hints.Show(_hint);
        return true;
    }

}
