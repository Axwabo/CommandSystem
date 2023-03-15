using System;
using Axwabo.CommandSystem.Attributes;
using Axwabo.CommandSystem.Structs;

namespace Axwabo.CommandSystem.Selectors.StackCommands;

[CommandProperties(CommandHandlerType.RaAndServer, "stackReverse", "Reverses the order of players on the selection stack.")]
[Aliases("sReverse", "sRev")]
internal sealed class StackReverse : CommandBase
{

    protected override CommandResult Execute(ArraySegment<string> arguments, CommandSender sender)
    {
        if (!PlayerSelectionStack.PreprocessCommand(sender, out var selection, out var result))
            return result;
        selection.Reverse();
        return "The selection stack has been reversed.";
    }

}
