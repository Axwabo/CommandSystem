using System;
using Axwabo.CommandSystem.Attributes;
using Axwabo.Helpers.Pools;

namespace Axwabo.CommandSystem.Selectors.StackCommands;

[CommandProperties(CommandHandlerType.RaAndServer, "stackduplicate", "Duplicates the specified selections on the player stack.")]
[Aliases("stackdup", "sdup")]
[Usage("sdup", "sdup all/*", "sdup first/f/top/t", $"sdup [indexes separated by spaces or one of{Separators}]")]
public sealed class StackDuplicate : CommandBase {

    private const string Separators = " .,;_+-";

    protected override CommandResult Execute(ArraySegment<string> arguments, CommandSender sender) {
        var selection = PlayerSelectionStack.Get(sender);
        if (selection == null)
            return $"!Cannot get a selection stack object from {sender.GetType().FullName}.";
        if (selection.IsEmpty)
            return "!The selection stack is empty.";
        if (arguments.Count < 1)
            return DuplicateFirst(selection);
        return arguments.At(0).ToLower() switch {
            "all" or "*" => DuplicateAll(selection),
            "first" or "f" or "top" or "t" => DuplicateFirst(selection),
            _ => DuplicateSpecific(selection, arguments)
        };
    }

    private static CommandResult DuplicateFirst(PlayerSelectionStack selection) {
        var dup = selection.Peek();
        selection.Push(dup);
        return $"Duplicated the first selection on the selection stack:\n{dup.CombineNicknames()}";
    }

    private static CommandResult DuplicateAll(PlayerSelectionStack selection) {
        selection.DuplicateAll();
        return "Duplicated all selections on the selection stack.";
    }

    private static CommandResult DuplicateSpecific(PlayerSelectionStack selection, ArraySegment<string> arguments) {
        var split = string.Join(" ", arguments).Split(Separators.ToCharArray());
        var push = ListPool<HubCollection>.Shared.Rent(split.Length);
        try {
            foreach (var s in split)
                if (int.TryParse(s, out var index) && index >= 0 && index < selection.Count)
                    push.Add(selection.PeekAt(index));
            if (push.Count == 0)
                return "!No valid indices were specified.";
            foreach (var collection in push)
                selection.Push(collection);

            return $"Duplicated {"selection".Pluralize(push.Count)} on the selection stack.";
        } finally {
            ListPool<HubCollection>.Shared.Return(push);
        }
    }

}
