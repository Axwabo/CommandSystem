using System;

namespace Axwabo.CommandSystem.Selectors.StackCommands {

    public sealed class StackReverse : CommandBase {

        protected override CommandResult Execute(ArraySegment<string> arguments, CommandSender sender) {
            var selection = PlayerSelectionStack.Get(sender);
            if (selection == null)
                return $"!Cannot get a selection stack object from {sender.GetType().FullName}.";
            if (selection.IsEmpty)
                return "!The selection stack is empty.";
            selection.Reverse();
            return $"Selection stack has been reversed:\n{selection}";
        }

    }

}
