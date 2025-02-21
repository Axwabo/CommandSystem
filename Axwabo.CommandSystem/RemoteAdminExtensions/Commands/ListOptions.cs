using Axwabo.CommandSystem.Attributes;
using Axwabo.CommandSystem.Attributes.Containers;
using Axwabo.CommandSystem.Commands;

namespace Axwabo.CommandSystem.RemoteAdminExtensions.Commands;

[CommandProperties("list", "Lists all RA options available to you.")]
[SubcommandOfContainer(typeof(OptionPreferencesContainer))]
internal sealed class ListOptions : CommandBase
{

    protected override CommandResult Execute(ArraySegment<string> arguments, CommandSender sender)
    {
        var sb = StringBuilderPool.Shared.Rent("Available Remote Admin options:").AppendLine();
        if (RemoteAdminOptionManager.AppendAllOptions(sender, sb, false, false))
            return CommandResult.Succeeded(StringBuilderPool.Shared.ToStringReturn(sb).TrimEnd());
        StringBuilderPool.Shared.Return(sb);
        return "!There are no options available to you.";
    }

}
