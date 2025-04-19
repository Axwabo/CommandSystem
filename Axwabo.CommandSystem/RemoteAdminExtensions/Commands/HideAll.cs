using Axwabo.CommandSystem.Attributes;
using Axwabo.CommandSystem.Attributes.Containers;
using Axwabo.CommandSystem.Commands;

namespace Axwabo.CommandSystem.RemoteAdminExtensions.Commands;

[CommandProperties("hideAll", "Hides all RA options available to you.")]
[SubcommandOfContainer(typeof(OptionPreferencesContainer))]
internal sealed class HideAll : CommandBase
{

    protected override CommandResult Execute(ArraySegment<string> arguments, CommandSender sender)
    {
        var nowHidden = 0;
        foreach (var option in RemoteAdminOptionManager.AllOptions)
            if (option.Permissions.CheckSafe(sender) && OptionPreferencesContainer.SetVisibility(sender.SenderId, option, false))
                nowHidden++;
        return nowHidden == 0
            ? "!All options are already hidden."
            : $"{nowHidden} {(nowHidden == 1 ? "option was" : "options were")} made invisible to you.";
    }

}
