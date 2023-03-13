#if EXILED
using Exiled.API.Features;
#else
using PluginAPI.Core;
#endif
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Axwabo.CommandSystem.RemoteAdminExtensions.Interfaces;
using Axwabo.Helpers;
using RemoteAdmin;

namespace Axwabo.CommandSystem.RemoteAdminExtensions;

/// <summary>
/// Manages the custom Remote Admin options.
/// </summary>
public static class RemoteAdminOptionManager
{

    /// <summary>A string containing all characters which identifiers may not contain.</summary>
    public const string InvalidCharacters = "$@()\"<>.";

    private static readonly char[] InvalidCharactersArray = InvalidCharacters.ToCharArray();

    private static readonly List<RemoteAdminOptionBase> Options = new();

    /// <summary>
    /// Registers a new Remote Admin option.
    /// </summary>
    /// <param name="option">The option to register.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="option"/> is null.</exception>
    public static void RegisterOption(RemoteAdminOptionBase option)
    {
        if (option is null)
            throw new ArgumentNullException(nameof(option));
        Options.Add(option);
    }

    /// <summary>
    /// Retrieves the list of registered options.
    /// </summary>
    public static IEnumerable<RemoteAdminOptionBase> AllOptions => Options.AsReadOnly();

    /// <summary>
    /// Handles a Remote Admin button click.
    /// </summary>
    /// <param name="identifier">The identifier of the option.</param>
    /// <param name="button">The button that was clicked.</param>
    /// <param name="sender">The sender of the request.</param>
    /// <param name="response">The response to send to the sender.</param>
    /// <returns>True if the request was handled, false otherwise.</returns>
    public static bool HandleCustomRequest(string identifier, RequestDataButton button, PlayerCommandSender sender, out string response)
    {
        if (sender == null)
        {
            response = null;
            return false;
        }

        identifier = identifier.Trim().TrimEnd('.');
        try
        {
            foreach (var option in Options)
            {
                if (!option.OptionIdentifier.Equals(identifier, StringComparison.OrdinalIgnoreCase))
                    continue;
                response = option.OnClick(button, sender);
                if (response == null)
                    return false;
                response = response.Color("white");
                return true;
            }
        }
        catch (Exception e)
        {
            Log.Error($"Error while processing Remote Admin data button request!\nButton: {button}, query: {identifier}\n" + e);
            response = "Failed to process request!".Color("red");
            return true;
        }

        response = null;
        return false;
    }

    /// <summary>
    /// Adds all available options to the specified <see cref="StringBuilder"/>.
    /// </summary>
    /// <param name="sender">The sender of the request.</param>
    /// <param name="builder">The <see cref="StringBuilder"/> to append the options to.</param>
    /// <param name="hideIdentifier">Whether to hide the option identifier using the size tag.</param>
    /// <remarks>Only displays options accessible and visible to the given sender.</remarks>
    public static void AppendAllOptions(CommandSender sender, StringBuilder builder, bool hideIdentifier = true)
    {
        var textToFormat = hideIdentifier ? "<size=0>({0})</size>{1}" : "({0}) {1}";
        foreach (var option in Options)
            if (option.Permissions.CheckSafe(sender) && (option is not IOptionVisibilityController controller || controller.IsVisibleTo(sender)))
                builder.AppendLine(string.Format(textToFormat, option.OptionIdentifier, option.GetText(sender)));
    }

    private static bool NonDigit(char arg) => arg != '-' && !char.IsDigit(arg);

    /// <summary>
    /// Checks if the given string is a valid option id.
    /// </summary>
    /// <param name="s">The string to check.</param>
    /// <returns>Whether the string is a valid option id.</returns>
    public static bool IsValidOptionId(string s)
    {
        if (s == null)
            return false;
        s = s.Trim();
        return s.Length > 0 && s.IndexOfAny(InvalidCharactersArray) == -1 && s.Any(NonDigit);
    }

}
