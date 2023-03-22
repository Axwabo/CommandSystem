using Axwabo.CommandSystem.Attributes.RaExt;
using Axwabo.CommandSystem.Exceptions;
using Axwabo.CommandSystem.Permissions;
using Axwabo.CommandSystem.PropertyManager;
using Axwabo.CommandSystem.RemoteAdminExtensions.Interfaces;
using RemoteAdmin;

namespace Axwabo.CommandSystem.RemoteAdminExtensions;

/// <summary>Base class for creating Remote Admin options.</summary>
public abstract class RemoteAdminOptionBase
{

    private static uint _autoId = 10; // CedMod compatibility

    private readonly string _staticText;

    private BlinkingIcon _icon;

    /// <summary>Whether this option identifier can also be used as a standalone selector.</summary>
    /// <seealso cref="StandaloneSelectorAttribute"/>
    /// <seealso cref="Axwabo.CommandSystem.Selectors.PlayerSelectionManager"/>
    protected virtual bool CanBeUsedAsStandaloneSelector => false;

    private InvalidNameException InvalidId => new($"Option identifier on type {GetType().FullName} is empty, numeric only or contains one of the following invalid characters: {RemoteAdminOptionManager.InvalidCharacters}");

    /// <summary>When overridden in a derived class, it will be used to set the <see cref="OptionIdentifier"/>.</summary>
    protected virtual string InitOnlyOptionId => null;

    /// <summary>Gets the identifier of this option.</summary>
    public string OptionIdentifier { get; }

    /// <summary>A permission checker that controls the global visibility of the option.</summary>
    public virtual IPermissionChecker VisibilityPermissions { get; }

    /// <summary>The leading icon of the option (before the generated text).</summary>
    public BlinkingIcon Icon
    {
        get => _icon;
        protected set => _icon = value;
    }

    /// <summary>Whether this instance is visible to all users by default.</summary>
    public bool IsVisibleByDefault { get; }

    // ReSharper disable VirtualMemberCallInConstructor
    /// <summary>
    /// Initializes an instance of <see cref="RemoteAdminOptionBase"/>.
    /// </summary>
    /// <exception cref="InvalidNameException">Thrown if <see cref="InitOnlyOptionId"/> is not null and invalid or the attributes don't provide a valid identifier.</exception>
    protected RemoteAdminOptionBase()
    {
        var id = InitOnlyOptionId;
        var derivedIsEmpty = string.IsNullOrWhiteSpace(id);
        if (!derivedIsEmpty && !RemoteAdminOptionManager.IsValidOptionId(id))
            throw InvalidId;
        var resolved = RemoteAdminExtensionPropertyManager.TryResolveProperties(this, out var idFromAttribute, out _staticText, out _icon, out var visibleByDefault, out var standaloneSelector);
        IsVisibleByDefault = visibleByDefault;
        if (derivedIsEmpty)
            id = idFromAttribute;
        if (id == AutoGenerateIdAttribute.Identifier)
            OptionIdentifier = "-" + ++_autoId;
        else if (!resolved)
            throw InvalidId;
        else
            OptionIdentifier = (standaloneSelector || CanBeUsedAsStandaloneSelector ? "@" : "$") + id;
        VisibilityPermissions ??= RemoteAdminExtensionPropertyManager.ResolvePermissionChecker(this);
    }

    /// <summary>
    /// Gets the text to display for the given sender.
    /// </summary>
    /// <param name="sender">The user to get the text for.</param>
    /// <returns>The text to display.</returns>
    public string GetText(CommandSender sender) => (Icon == null ? "" : Icon) + GenerateDisplayText(sender);

    /// <summary>
    /// Generates the text to display for the given sender.
    /// </summary>
    /// <param name="sender">The user to get the text for.</param>
    /// <returns>The generated text.</returns>
    protected virtual string GenerateDisplayText(CommandSender sender) => _staticText;

    /// <summary>
    /// Handles the click of the given button and checks for insufficient permissions beforehand.
    /// </summary>
    /// <param name="button">The button that was clicked.</param>
    /// <param name="sender">The user that clicked the button.</param>
    /// <returns>The text to display.</returns>
    public string OnClick(RequestDataButton button, PlayerCommandSender sender)
    {
        var permissions = VisibilityPermissions.CheckSafe(sender);
        return !permissions
            ? permissions
            : this is IOptionVisibilityController {AllowInteractionsWhenHidden: false} controller && !controller.IsVisibleTo(sender)
                ? "This option is not accessible at the moment."
                : HandleButtonClick(button, sender);
    }

    /// <summary>
    /// Handles the functionality of the given button.
    /// </summary>
    /// <param name="button">The button that was clicked.</param>
    /// <param name="sender">The user that clicked the button.</param>
    /// <returns>The text to display.</returns>
    protected abstract string HandleButtonClick(RequestDataButton button, PlayerCommandSender sender);

}
