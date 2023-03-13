using System;
using Axwabo.CommandSystem.Attributes;
using Axwabo.CommandSystem.Structs;

namespace Axwabo.CommandSystem.Selectors.StackCommands;

/// <summary>A command to clear the selection stack.</summary>
[CommandProperties(CommandHandlerType.RaAndServer, "stackClear", "Clears the player selection stack.")]
[Aliases("sClear", "sClr", "scl")]
public sealed class StackClear : CommandBase
{

    /// <inheritdoc />
    protected override CommandResult Execute(ArraySegment<string> arguments, CommandSender sender)
    {
        if (!PlayerSelectionStack.PreprocessCommand(sender, out var selection, out var result, true))
            return result;
        if (selection.IsEmpty)
            return "The selection stack is already empty.";
        var count = selection.Count;
        selection.Clear();
        return $"Cleared the selection stack of {"selection".PluralizeWithCount(count)}.";
    }

}
