using System;
using Axwabo.CommandSystem.Attributes;
using Axwabo.CommandSystem.Structs;

namespace Axwabo.CommandSystem.Selectors.StackCommands;

[CommandProperties(CommandHandlerType.RaAndServer, "stackreverse", "Reverses the order of players on the selection stack.")]
[Aliases("sreverse", "srev")]
public sealed class StackReverse : CommandBase {

    protected override CommandResult Execute(ArraySegment<string> arguments, CommandSender sender) {
        var selection = PlayerSelectionStack.Get(sender);
        if (selection == null)
            return $"!Cannot get a selection stack object from {sender.GetType().FullName}.";
        if (selection.IsEmpty)
            return "!The selection stack is empty.";
        selection.Reverse();
        return "The selection stack has been reversed.";
    }

}
