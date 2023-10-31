using System.IO;
using Axwabo.CommandSystem.Attributes;
using Axwabo.CommandSystem.Commands;
using Axwabo.Helpers;

namespace Axwabo.CommandSystem.RemoteAdminExtensions.Commands;

/// <summary>
/// A command for managing RA option visibility preferences.
/// </summary>
[CommandProperties(CommandHandlerType.RemoteAdmin, "remoteAdminOption", "Controls Remote Admin extensions.")]
[Aliases("raOption", "raOpt")]
[PlayerOnlyCommand]
public sealed class OptionPreferencesContainer : ContainerCommand
{

    private static string FolderPath => Path.Combine(Plugin.PluginDirectory, "Preferences");

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

        Func<string, bool> method = visible ? set.Remove : set.Add;
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

    /// <summary>
    /// Stores the current state of the option preferences on the disk.
    /// </summary>
    public static void SaveState()
    {
        try
        {
            var path = FolderPath;
            Directory.CreateDirectory(path);
            foreach (var kvp in HiddenCommandsByType)
                File.WriteAllLines(Path.Combine(path, kvp.Key), kvp.Value);
        }
        catch (Exception e)
        {
            Log.Error(e);
        }
    }

    /// <summary>
    /// Loads the option preferences from the disk.
    /// </summary>
    public static void LoadState()
    {
        try
        {
            Directory.CreateDirectory(FolderPath);
            var files = Directory.GetFiles(FolderPath);
            foreach (var file in files)
            {
                var userId = Path.GetFileName(file);
                var lines = File.ReadAllLines(file);
                if (lines.Length != 0)
                    HiddenCommandsByType[userId] = new HashSet<string>(lines);
            }
        }
        catch (Exception e)
        {
            Log.Error(e);
        }
    }

}
