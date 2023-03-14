using System;
using System.Collections.Generic;
using Axwabo.CommandSystem.Permissions;
using Axwabo.CommandSystem.PropertyManager;
using RemoteAdmin;

namespace Axwabo.CommandSystem.RemoteAdminExtensions;

/// <summary>
/// A Remote Admin option class that handles each button individually, allowing for different permissions.
/// </summary>
public abstract class IndividualButtonBasedRemoteAdminOption : RemoteAdminOptionBase
{

    private static readonly RequestDataButton[] RequestDataButtonValues = {RequestDataButton.BasicInfo, RequestDataButton.RequestIP, RequestDataButton.RequestAuth, RequestDataButton.ExternalLookup};

    private readonly Dictionary<RequestDataButton, IPermissionChecker> _buttonPermissions = new();

    private Func<PlayerCommandSender, string> GetMethodByButton(RequestDataButton button) => button switch
    {
        RequestDataButton.BasicInfo => OnBasicInfoClicked,
        RequestDataButton.RequestIP => OnRequestIPClicked,
        RequestDataButton.RequestAuth => OnRequestAuthClicked,
        RequestDataButton.ExternalLookup => OnExternalLookupClicked,
        _ => throw new ArgumentOutOfRangeException(nameof(button), button, "Button could not be mapped to a method")
    };

    /// <summary>Creates a new <see cref="IndividualButtonBasedRemoteAdminOption"/> instance.</summary>
    protected IndividualButtonBasedRemoteAdminOption()
    {
        foreach (var button in RequestDataButtonValues)
            _buttonPermissions[button] = RemoteAdminExtensionPropertyManager.ResolvePermissionChecker(this, GetMethodByButton(button).Method);
    }

    /// <summary>
    /// Handles the <see cref="RequestDataButton.BasicInfo"/> button click.
    /// </summary>
    /// <param name="sender">The user that clicked the button.</param>
    /// <returns>The text to display.</returns>
    protected virtual string OnBasicInfoClicked(PlayerCommandSender sender) => null;

    /// <summary>
    /// Handles the <see cref="RequestDataButton.RequestIP"/> button click.
    /// </summary>
    /// <param name="sender">The user that clicked the button.</param>
    /// <returns>The text to display.</returns>
    protected virtual string OnRequestIPClicked(PlayerCommandSender sender) => null;

    /// <summary>
    /// Handles the <see cref="RequestDataButton.RequestAuth"/> button click.
    /// </summary>
    /// <param name="sender">The user that clicked the button.</param>
    /// <returns>The text to display.</returns>
    protected virtual string OnRequestAuthClicked(PlayerCommandSender sender) => null;

    /// <summary>
    /// Handles the <see cref="RequestDataButton.ExternalLookup"/> button click.
    /// </summary>
    /// <param name="sender">The user that clicked the button.</param>
    /// <returns>The text to display.</returns>
    protected virtual string OnExternalLookupClicked(PlayerCommandSender sender) => null;

    /// <inheritdoc />
    protected override string HandleButtonClick(RequestDataButton button, PlayerCommandSender sender)
    {
        var method = GetMethodByButton(button);
        if (!_buttonPermissions.TryGetValue(button, out var permissionChecker) || permissionChecker == null)
            return method(sender);
        var result = permissionChecker.CheckSafe(sender);
        return result ? method(sender) : result;
    }

}
