using System;
using System.Collections.Generic;
using System.Linq;
using Axwabo.CommandSystem.Attributes;
using Axwabo.CommandSystem.Commands;
using Axwabo.CommandSystem.Permissions;
using Axwabo.Helpers;

namespace Axwabo.CommandSystem.RemoteAdminExtensions.Commands;

[CommandProperties(CommandHandlerType.RemoteAdmin, "remoteAdminOption", "Controls Remote Admin extensions.")]
[Aliases("raOption", "raOpt")]
[PlayerOnlyCommand]
[VanillaPermissions(PlayerPermissions.GameplayData)]
public sealed class OptionPreferencesContainer : ContainerCommand
{

    private static readonly Dictionary<string, HashSet<string>> HiddenCommandsByType = new();

    public static bool SetVisibility(string userId, RemoteAdminOptionBase option, bool visible)
    {
        if (!HiddenCommandsByType.TryGetValue(userId, out var set))
        {
            HiddenCommandsByType[userId] = set = new HashSet<string>();
            set.AddRange(RemoteAdminOptionManager.AllOptions
                .Where(e => !e.IsVisibleByDefault)
                .Select(e => e.GetType().FullName));
            return visible && set.Remove(option.GetType().FullName);
        }

        var method = visible ? (Func<string, bool>) set.Remove : set.Add;
        return method(option.GetType().FullName);
    }

    public static bool IsHidden(string userId, RemoteAdminOptionBase option)
        => HiddenCommandsByType.TryGetValue(userId, out var set)
            ? set.Contains(option.GetType().FullName)
            : !option.IsVisibleByDefault;

}
