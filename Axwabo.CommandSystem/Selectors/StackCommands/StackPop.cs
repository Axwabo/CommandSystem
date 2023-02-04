using System;
using Axwabo.CommandSystem.Attributes;

namespace Axwabo.CommandSystem.Selectors.StackCommands;

[CommandProperties(CommandHandlerType.RaAndServer, "stackpop", "Pops the topmost players from the selection stack.", "spop")]
[Usage("spop", "spop <index>")]
public sealed class StackPop : CommandBase {

    protected override CommandResult Execute(ArraySegment<string> arguments, CommandSender sender) {
        var selection = PlayerSelectionStack.Get(sender);
        if (selection == null)
            return $"!Cannot get a selection stack object from {sender.GetType().FullName}.";
        if (selection.IsEmpty)
            return "!The selection stack is empty.";
        var indexSet = false;
        HubCollection popped;
        var index = 0;
        if (arguments.Count > 0 && int.TryParse(arguments.At(0), out index)) {
            popped = selection.PopAt(index);
            indexSet = true;
        } else
            popped = selection.Pop();

        return $"Popped {"player".Pluralize(popped.Count)} from the selection stack{(indexSet ? $" at index {index}" : "")}:\n{popped.CombineNicknames()}";
    }

}
