using Axwabo.CommandSystem.Attributes;
using Axwabo.CommandSystem.Commands;

namespace Axwabo.CommandSystem.Selectors.StackCommands;

[CommandProperties(CommandHandlerType.RaAndServer, "stackList", "Lists the players on the selection stack.", "slist")]
internal sealed class StackList : CommandBase
{

    protected override CommandResult Execute(ArraySegment<string> arguments, CommandSender sender)
    {
        var selection = PlayerSelectionStack.Get(sender);
        return selection == null
            ? $"!Cannot get a selection stack object from {sender.GetType().FullName}."
            : selection.IsEmpty
                ? "The selection stack is empty."
                : $"Your selection stack:\n{selection}";
    }

}
