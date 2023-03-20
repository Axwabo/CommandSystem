using System;
using Axwabo.CommandSystem.Attributes;
using Axwabo.CommandSystem.Attributes.Containers;
using Axwabo.CommandSystem.Structs;

namespace Axwabo.CommandSystem.RemoteAdminExtensions.Commands;

[CommandProperties("hide", 1, "Hides the given option from your RA GUI.")]
[SubcommandOfContainer(typeof(OptionSettingsContainer))]
internal sealed class HideOption : CommandBase
{

    protected override CommandResult Execute(ArraySegment<string> arguments, CommandSender sender)
        => !RemoteAdminOptionManager.TryGetOption(arguments.At(0), out var option)
            ? "!No option found with that id."
            : OptionSettingsContainer.SetVisibility(sender.SenderId, option, false)
                ? "Option is no longer visible to you."
                : "!Option already isn't visible to you.";

}
