﻿using Axwabo.CommandSystem.Attributes;
using Axwabo.CommandSystem.Commands;
using Axwabo.CommandSystem.Extensions;

namespace Axwabo.CommandSystem.Selectors.StackCommands;

[CommandProperties(CommandHandlerType.RaAndServer, "stackClear", "Clears the player selection stack.")]
[Aliases("sClear", "sClr", "scl")]
internal sealed class StackClear : CommandBase
{

    protected override CommandResult Execute(ArraySegment<string> arguments, CommandSender sender)
    {
        if (!PlayerSelectionStack.PreprocessCommand(sender, out var selection, out var result, true))
            return result;
        if (selection.IsEmpty)
            return "The selection stack is already empty.";
        var count = selection.Count;
        selection.Clear();
        return $"Cleared the selection stack of {"selection".PluralizeWithCount(count)}.";
    }

}
