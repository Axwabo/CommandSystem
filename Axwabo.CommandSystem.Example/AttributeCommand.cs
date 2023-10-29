using System;
using Axwabo.CommandSystem.Attributes;
using Axwabo.CommandSystem.Commands.Interfaces;
using Axwabo.Helpers;

namespace Axwabo.CommandSystem.Example;

[RemoteAdminCommand]
[ServerCommand]
[ClientCommand]
[CommandName("attributeCommand")]
[Description("Command with attributes")]
[Aliases("attributeAlias", "attributeAlias2")]
[MinArguments(1)] // at least one argument is required
[Usage("number")] // do not include the command name
public sealed class AttributeCommand : CommandBase, IHiddenCommand
{

    public bool IsHidden => true; // this command will not be visible in the help list or tab completion

    protected override CommandResult Execute(ArraySegment<string> arguments, CommandSender sender)
    {
        if (!arguments.ParseInt(out var number)) // you can also use int.TryParse
            return "!Invalid number!"; // prepend ! to mark the result as failed
        if (number == 69420)
            return CommandResult.Succeeded("Nice");
        return "The result is " + number * 69; // this will be marked as succeeded
    }

}
