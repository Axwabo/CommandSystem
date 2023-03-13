using System;
using System.Text;
using Axwabo.CommandSystem.Attributes;
using Axwabo.CommandSystem.Structs;

namespace Axwabo.CommandSystem.RemoteAdminExtensions;

[CommandProperties(CommandHandlerType.RemoteAdmin, "optionList", "Lists all available Remote Admin extensions to you.", "raOptions")]
internal sealed class OptionListCommand : CommandBase
{

    protected override CommandResult Execute(ArraySegment<string> arguments, CommandSender sender)
    {
        var sb = new StringBuilder("Available Remote Admin extensions:\n");
        RemoteAdminOptionManager.AppendAllOptions(sender, sb, false);
        return CommandResult.Succeeded(sb.ToString().TrimEnd());
    }

}
