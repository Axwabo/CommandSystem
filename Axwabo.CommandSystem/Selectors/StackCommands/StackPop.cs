using System;
using Axwabo.CommandSystem.Attributes;

namespace Axwabo.CommandSystem.Selectors.StackCommands;

[CommandProperties(CommandHandlerType.RaAndServer, "stackPop", "Pops the topmost players from the selection stack.", "spop")]
[Usage("", "<index>")]
internal sealed class StackPop : CommandBase
{

    protected override CommandResult Execute(ArraySegment<string> arguments, CommandSender sender)
    {
        if (!PlayerSelectionStack.PreprocessCommand(sender, out var selection, out var result))
            return result;
        var indexSet = false;
        HubCollection popped;
        var index = 0;
        if (arguments.Count > 0 && int.TryParse(arguments.At(0), out index))
        {
            popped = selection.PopAt(index);
            indexSet = true;
        }
        else
            popped = selection.Pop();

        return $"Popped {"player".PluralizeWithCount(popped.Count)} from the selection stack{(indexSet ? $" at index {index}" : "")}:\n{popped.CombineNicknames()}";
    }

}
