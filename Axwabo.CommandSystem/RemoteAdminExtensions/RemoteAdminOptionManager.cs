using System.Text;
using Axwabo.CommandSystem.Attributes.RaExt;
using Axwabo.CommandSystem.RemoteAdminExtensions.Commands;
using Axwabo.CommandSystem.RemoteAdminExtensions.Interfaces;
using RemoteAdmin.Interfaces;

namespace Axwabo.CommandSystem.RemoteAdminExtensions;

/// <summary>Manages the custom Remote Admin options.</summary>
public static class RemoteAdminOptionManager
{

    /// <summary>A string containing all characters which identifiers may not contain.</summary>
    public const string InvalidCharacters = "$@()\".<>";

    private static readonly char[] InvalidCharactersArray = InvalidCharacters.ToCharArray();

    private static readonly List<RemoteAdminOptionBase> Options = [];

    /// <summary>Retrieves the list of registered options.</summary>
    public static IReadOnlyList<RemoteAdminOptionBase> AllOptions => Options.AsReadOnly();

    /// <summary>
    /// Registers a new Remote Admin option.
    /// </summary>
    /// <param name="option">The option to register.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="option"/> is null.</exception>
    public static void RegisterOption(RemoteAdminOptionBase option)
    {
        if (option is null)
            throw new ArgumentNullException(nameof(option));
        if (TryGetOptionByType(option.GetType().FullName, out var sameType))
            throw new InvalidOperationException($"An option with the same type \"{sameType.GetType().FullName}\" has already been registered.");
        if (TryGetOption(option.OptionIdentifier, out var sameIdentifier))
            throw new InvalidOperationException($"An option with the same identifier \"{sameIdentifier.OptionIdentifier}\" has already been registered. Conflicting types: \"{option.GetType().FullName}\" against \"{sameIdentifier.GetType().FullName}\"");
        Options.Add(option);
    }

    /// <summary>
    /// Unregisters all Remote Admin options that are defined in the specified assembly.
    /// </summary>
    /// <param name="assembly">The assembly to unregister the options from.</param>
    public static void UnregisterAll(Assembly assembly) => Options.RemoveAll(e => e.GetType().Assembly == assembly);

    /// <summary>
    /// Attempts to retrieve an option by its identifier.
    /// </summary>
    /// <param name="identifier">The identifier of the option.</param>
    /// <param name="option">The found option.</param>
    /// <returns>Whether the option was found.</returns>
    public static bool TryGetOption(string identifier, out RemoteAdminOptionBase option)
    {
        option = Options.FirstOrDefault(e => e.OptionIdentifier.Equals(identifier, StringComparison.OrdinalIgnoreCase));
        return option != null;
    }

    /// <summary>
    /// Attempts to retrieve an option by its type's full name.
    /// </summary>
    /// <param name="fullName">The full name of the option's type.</param>
    /// <param name="option">The found option.</param>
    /// <returns>Whether the option was found.</returns>
    public static bool TryGetOptionByType(string fullName, out RemoteAdminOptionBase option)
    {
        option = Options.FirstOrDefault(e => e.GetType().FullName == fullName);
        return option != null;
    }

    /// <summary>
    /// Handles a Remote Admin button click.
    /// </summary>
    /// <param name="identifier">The identifier of the option.</param>
    /// <param name="button">The button that was clicked.</param>
    /// <param name="sender">The sender of the request.</param>
    /// <param name="response">The response to send to the sender.</param>
    /// <returns>True if the request was handled and the response is not null, false otherwise.</returns>
    public static bool HandleCustomRequest(string identifier, RequestDataButton button, PlayerCommandSender sender, out string response)
    {
        if (sender == null || identifier == null)
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
    /// <param name="hideIdentifier">Whether to hide the option identifier using the size tag and skip entries that are not visible with <see cref="IOptionVisibilityController"/>.</param>
    /// <param name="preferredOnly">Whether to only display options preferred by the sender.</param>
    /// <remarks>Only displays options accessible and visible to the given sender.</remarks>
    public static bool AppendAllOptions(CommandSender sender, StringBuilder builder, bool hideIdentifier = true, bool preferredOnly = true)
    {
        var textToFormat = hideIdentifier ? "<size=0>({0})</size>{1}" : "({0}) {1}";
        var success = false;
        foreach (var option in Options)
        {
            if (!option.VisibilityPermissions.CheckSafe(sender) || preferredOnly && OptionPreferencesContainer.IsHidden(sender.SenderId, option))
                continue;
            var hidden = option is IOptionVisibilityController controller && !controller.IsVisibleTo(sender);
            if (hideIdentifier && hidden)
                continue;
            success = true;
            var hiddenString = OptionPreferencesContainer.IsHidden(sender.SenderId, option)
                ? "[HIDDEN BY PREFS] ".Color("#c8c8c8")
                : hidden
                    ? "[HIDDEN BY CONTROLLER] ".Color("#c8c8c8")
                    : "";
            var idString = hideIdentifier ? option.OptionIdentifier : option.OptionIdentifier.Color("#9696ff");
            builder.Append(hiddenString);
            builder.AppendLine(string.Format(textToFormat, idString, option.GetText(sender)));
        }

        return success;
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
        if (s is AutoGenerateIdAttribute.Identifier)
            return true;
        return s.Length > 0 && s.IndexOfAny(InvalidCharactersArray) == -1 && s.Any(NonDigit);
    }

    /// <summary>
    /// Sends a reply to the given sender via a communication method.
    /// </summary>
    /// <param name="communication">The communication method to use.</param>
    /// <param name="sender">The user to reply send the to.</param>
    /// <param name="content">The content to send.</param>
    public static void SendReply(this IServerCommunication communication, CommandSender sender, string content)
        => sender.RaReply($"${communication.DataId} {content}", true, true, "");

}
