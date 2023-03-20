using System;
using System.Collections.Generic;
using Axwabo.CommandSystem.Attributes;
using Axwabo.CommandSystem.Commands;

namespace Axwabo.CommandSystem.RemoteAdminExtensions.Commands;

[CommandProperties(CommandHandlerType.RemoteAdmin, "remoteAdminOption", "Controls Remote Admin extensions.")]
[Aliases("raOption", "raOpt")]
[PlayerOnlyCommand]
internal sealed class OptionSettingsContainer : ContainerCommand
{

    private static readonly Dictionary<string, HashSet<Type>> HiddenCommands = new();

    public static bool SetVisibility(string userId, RemoteAdminOptionBase option, bool visible)
    {
        if (!HiddenCommands.TryGetValue(userId, out var set))
        {
            HiddenCommands[userId] = set = new HashSet<Type>();
            if (option.IsHiddenByDefault)
            {
                set.Add(option.GetType());
                return false;
            }
        }

        var method = visible ? (Func<Type, bool>) set.Remove : set.Add;
        return method(option.GetType());
    }

    public static bool IsHidden(string userId, RemoteAdminOptionBase option)
        => HiddenCommands.TryGetValue(userId, out var set)
            ? set.Contains(option.GetType())
            : option.IsHiddenByDefault;

}
