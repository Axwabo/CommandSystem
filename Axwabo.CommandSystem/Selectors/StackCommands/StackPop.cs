using System;
using Axwabo.CommandSystem.Attributes;

namespace Axwabo.CommandSystem.Selectors.StackCommands {

    [CommandProperties(CommandTarget.RaAndConsole, "stackpop", "Pops the topmost players from the selection stack.")]
    public sealed class StackPop : CommandBase {

        protected override CommandResult Execute(ArraySegment<string> arguments, CommandSender sender) {
            var selection = PlayerSelectionStack.Get(sender);
            if (selection == null)
                return $"!Cannot get a selection stack object from {sender.GetType().FullName}.";
            if (selection.IsEmpty)
                return "!The selection stack is empty.";
            var popped = selection.Pop();
            return $"Popped {"player".Pluralize(popped.Count)} from the selection stack:\n{popped.CombineNicknames()}";
        }

    }

}
