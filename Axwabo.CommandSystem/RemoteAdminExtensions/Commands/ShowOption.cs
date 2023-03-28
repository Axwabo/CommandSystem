using System;
using Axwabo.CommandSystem.Attributes;
using Axwabo.CommandSystem.Attributes.Containers;

namespace Axwabo.CommandSystem.RemoteAdminExtensions.Commands;

[CommandProperties("show", 1, "Makes the given option visible to you.")]
[SubcommandOfContainer(typeof(OptionPreferencesContainer))]
internal sealed class ShowOption : CommandBase
{

    protected override CommandResult Execute(ArraySegment<string> arguments, CommandSender sender)
        => !RemoteAdminOptionManager.TryGetOption(arguments.At(0), out var option)
            ? "!No option found with that id."
            : OptionPreferencesContainer.SetVisibility(sender.SenderId, option, true)
                ? "Option is now visible to you."
                : "!Option is already visible to you.";

}
