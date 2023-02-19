using System;
using Axwabo.CommandSystem.Attributes;
using Axwabo.CommandSystem.Structs;

namespace Axwabo.CommandSystem.Selectors.StackCommands;

/// <summary>A command to show the selection stack.</summary>
[CommandProperties(CommandHandlerType.RaAndServer, "stackList", "Lists the players on the selection stack.", "slist")]
public sealed class StackList : CommandBase {

    /// <inheritdoc />
    protected override CommandResult Execute(ArraySegment<string> arguments, CommandSender sender) {
        var selection = PlayerSelectionStack.Get(sender);
        return selection == null
            ? $"!Cannot get a selection stack object from {sender.GetType().FullName}."
            : selection.IsEmpty
                ? "The selection stack is empty."
                : $"Your selection stack:\n{selection}";
    }

}
