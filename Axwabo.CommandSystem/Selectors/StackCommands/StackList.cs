using System;
using Axwabo.CommandSystem.Attributes;

namespace Axwabo.CommandSystem.Selectors.StackCommands;

[CommandProperties(CommandHandlerType.RaAndServer, "stacklist", "Lists the players on the selection stack.", "slist")]
public sealed class StackList : CommandBase {

    protected override CommandResult Execute(ArraySegment<string> arguments, CommandSender sender) {
        var selection = PlayerSelectionStack.Get(sender);
        return selection == null
            ? $"!Cannot get a selection stack object from {sender.GetType().FullName}."
            : selection.IsEmpty
                ? "The selection stack is empty."
                : $"Your selection stack:\n{selection}";
    }

}
