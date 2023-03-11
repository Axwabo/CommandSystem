#if EXILED
using Exiled.API.Features;
#else
using PluginAPI.Core;
#endif
using System;
using System.Collections.Generic;
using System.Text;
using Axwabo.CommandSystem.RemoteAdminExtensions.Interfaces;
using Axwabo.Helpers;
using RemoteAdmin;

namespace Axwabo.CommandSystem.RemoteAdminExtensions;

public static class RemoteAdminOptionManager {

    private static readonly List<RemoteAdminOptionBase> Options = new();

    public static void RegisterOption(RemoteAdminOptionBase option) => Options.Add(option);

    public static IEnumerable<RemoteAdminOptionBase> AllOptions => Options.AsReadOnly();

    public static bool HandleCustomRequest(string name, RequestDataButton button, PlayerCommandSender sender, out string response) {
        name = name.Trim().TrimEnd('.');
        try {
            foreach (var option in Options) {
                if (!option.OptionIdentifier.Equals(name, StringComparison.OrdinalIgnoreCase))
                    continue;
                response = option.OnClick(button, sender);
                if (response == null)
                    return false;
                response = response.Color("white");
                return true;
            }
        } catch (Exception e) {
            Log.Error($"Error while processing Remote Admin data button request!\nButton: {button}, query: {name}\n" + e);
            response = "Failed to process request!".Color("red");
            return true;
        }

        response = null;
        return false;
    }

    public static void AppendAllOptions(CommandSender sender, StringBuilder builder, bool hideIdentifier = true) {
        var textToFormat = hideIdentifier ? "<size=0>({0})</size>{1}" : "({0}) {1}";
        foreach (var option in Options)
            if (option is not IOptionVisibilityController controller || controller.IsVisibleTo(sender))
                builder.AppendLine(string.Format(textToFormat, option.OptionIdentifier, option.GetText(sender)));
    }

}
