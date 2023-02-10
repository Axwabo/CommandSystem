﻿using System;
using System.Collections.Generic;
using Axwabo.CommandSystem.Attributes;
using Axwabo.CommandSystem.Commands;
using Axwabo.CommandSystem.Structs;

namespace Axwabo.CommandSystem.Selectors.StackCommands;

[CommandProperties(CommandHandlerType.RaAndServer, "stackPush", "Pushes the given players onto the selection stack.")]
[Aliases("spush", "sp")]
[Usage("<players>")]
public sealed class StackPush : UnifiedTargetingCommand {

    protected override CommandResult ExecuteOnTargets(List<ReferenceHub> targets, ArraySegment<string> arguments, CommandSender sender) {
        if (!PlayerSelectionStack.PreprocessCommand(sender, out var selection, out var result, true))
            return result;
        selection.Push(targets);
        return $"Pushed {"player".PluralizeWithCount(targets.Count)} onto the selection stack:\n{targets.CombineNicknames()}";
    }

}
