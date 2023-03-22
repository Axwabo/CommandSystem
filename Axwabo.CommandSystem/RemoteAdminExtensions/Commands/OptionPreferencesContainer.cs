using System;
using System.Collections.Generic;
using System.Linq;
using Axwabo.CommandSystem.Attributes;
using Axwabo.CommandSystem.Commands;
using Axwabo.CommandSystem.Permissions;
using Axwabo.Helpers;

namespace Axwabo.CommandSystem.RemoteAdminExtensions.Commands;

/// <summary>
/// A command for managing RA option visibility preferences.
/// </summary>
[CommandProperties(CommandHandlerType.RemoteAdmin, "remoteAdminOption", "Controls Remote Admin extensions.")]
[Aliases("raOption", "raOpt")]
[PlayerOnlyCommand]
[VanillaPermissions(PlayerPermissions.GameplayData)]
public sealed class OptionPreferencesContainer : ContainerCommand
{

    private static readonly Dictionary<string, HashSet<string>> HiddenCommandsByType = new();

    /// <summary>
    /// Sets the visibility of the given option for the user.
    /// </summary>
    /// <param name="userId">The user's Steam ID.</param>
    /// <param name="option">The option to set the visibility of.</param>
    /// <param name="visible">Whether to show the option.</param>
    /// <returns>Whether the visibility was changed.</returns>
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

    /// <summary>
    /// Determines whether the given option is hidden for the user.
    /// </summary>
    /// <param name="userId">The user's Steam ID.</param>
    /// <param name="option">The option to check.</param>
    /// <returns>Whether the option is hidden.</returns>
    public static bool IsHidden(string userId, RemoteAdminOptionBase option)
        => HiddenCommandsByType.TryGetValue(userId, out var set)
            ? set.Contains(option.GetType().FullName)
            : !option.IsVisibleByDefault;

}
