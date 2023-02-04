using System;
using Axwabo.CommandSystem.Attributes;
using Axwabo.CommandSystem.Structs;

namespace Axwabo.CommandSystem.Selectors.StackCommands;

[CommandProperties(CommandHandlerType.RaAndServer, "stackclear", "Clears the player selection stack.")]
[Aliases("sclear", "sclr", "scl")]
public sealed class StackClear : CommandBase {

    protected override CommandResult Execute(ArraySegment<string> arguments, CommandSender sender) {
        var selection = PlayerSelectionStack.Get(sender);
        if (selection == null)
            return $"!Cannot get a selection stack object from {sender.GetType().FullName}.";
        if (selection.IsEmpty)
            return "The selection stack is already empty.";
        var count = selection.Count;
        selection.Clear();
        return $"Cleared the selection stack of {"selection".Pluralize(count)}.";
    }

}
