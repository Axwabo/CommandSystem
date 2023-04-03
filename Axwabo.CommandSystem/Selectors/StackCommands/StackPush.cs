using System;
using System.Collections.Generic;
using Axwabo.CommandSystem.Attributes;
using Axwabo.CommandSystem.Commands;

namespace Axwabo.CommandSystem.Selectors.StackCommands;

[CommandProperties(CommandHandlerType.RaAndServer, "stackPush", "Pushes the given players onto the selection stack.")]
[Aliases("sPush", "sp")]
internal sealed class StackPush : UnifiedTargetingCommand
{

    protected override CommandResult ExecuteOnTargets(List<ReferenceHub> targets, ArraySegment<string> arguments, CommandSender sender)
    {
        if (!PlayerSelectionStack.PreprocessCommand(sender, out var selection, out var result, true))
            return result;
        if (selection.CheckOverflow(1))
            return $"!Already reached the limit of lists on the selection stack: {PlayerSelectionStack.MaxSize}.";
        selection.Push(targets);
        return $"Pushed {"player".PluralizeWithCount(targets.Count)} onto the selection stack:\n{targets.CombineNicknames()}";
    }

}
