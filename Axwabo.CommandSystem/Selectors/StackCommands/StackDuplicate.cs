using System;
using Axwabo.CommandSystem.Attributes;
using Axwabo.CommandSystem.Commands.Interfaces;
using Axwabo.CommandSystem.Structs;
using Axwabo.Helpers.Pools;

namespace Axwabo.CommandSystem.Selectors.StackCommands;

[CommandProperties(CommandHandlerType.RaAndServer, "stackDuplicate", 1, "Duplicates the specified selections on the player stack.")]
[Aliases("stackDup", "sDup")]
[Usage("", "all/*", "first/f/top/t", $"indexes separated by spaces or one of{Separators}")]
internal sealed class StackDuplicate : CommandBase, INotEnoughArguments
{

    private const string Separators = " .,;_+-";

    protected override CommandResult Execute(ArraySegment<string> arguments, CommandSender sender)
        => !PlayerSelectionStack.PreprocessCommand(sender, out var selection, out var result)
            ? result
            : arguments.At(0).ToLower() switch
            {
                "all" or "*" => DuplicateAll(selection),
                "first" or "f" or "top" or "t" => DuplicateFirst(selection),
                _ => DuplicateSpecific(selection, arguments)
            };

    public CommandResult? OnNotEnoughArgumentsProvided(ArraySegment<string> arguments, CommandSender sender, int required)
        => !PlayerSelectionStack.PreprocessCommand(sender, out var selection, out var result) ? result : DuplicateFirst(selection);

    private static CommandResult DuplicateFirst(PlayerSelectionStack selection)
    {
        var dup = selection.Peek();
        selection.Push(dup);
        return $"Duplicated the first selection on the selection stack:\n{dup.CombineNicknames()}";
    }

    private static CommandResult DuplicateAll(PlayerSelectionStack selection)
    {
        selection.DuplicateAll();
        return "Duplicated all selections on the selection stack.";
    }

    private static CommandResult DuplicateSpecific(PlayerSelectionStack selection, ArraySegment<string> arguments)
    {
        var split = string.Join(" ", arguments).Split(Separators.ToCharArray());
        var push = ListPool<HubCollection>.Shared.Rent(split.Length);
        try
        {
            foreach (var s in split)
                if (int.TryParse(s, out var index) && index >= 0 && index < selection.Count)
                    push.Add(selection.PeekAt(index));
            if (push.Count == 0)
                return "!No valid indices were specified.";
            foreach (var collection in push)
                selection.Push(collection);

            return $"Duplicated {"selection".PluralizeWithCount(push.Count)} on the selection stack.";
        }
        finally
        {
            ListPool<HubCollection>.Shared.Return(push);
        }
    }

}
