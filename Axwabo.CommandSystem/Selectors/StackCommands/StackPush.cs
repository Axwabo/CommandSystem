using System;
using System.Collections.Generic;
using Axwabo.CommandSystem.Attributes;
using Axwabo.CommandSystem.Commands;

namespace Axwabo.CommandSystem.Selectors.StackCommands {

    [CommandProperties(CommandHandlerType.RaAndServer, "stackpush", "Pushes the given players onto the selection stack.")]
    public sealed class StackPush : UnifiedTargetingCommand {

        protected override CommandResult ExecuteOnTargets(List<ReferenceHub> targets, ArraySegment<string> arguments, CommandSender sender) {
            var selection = PlayerSelectionStack.Get(sender);
            if (selection == null)
                return $"!Cannot get a selection stack object from {sender.GetType().FullName}.";
            selection.Push(targets);
            return $"Pushed {"player".Pluralize(targets.Count)} onto the selection stack:\n{targets.CombineNicknames()}";
        }

    }

}
