namespace Axwabo.CommandSystem.RemoteAdminExtensions.Interfaces;

/// <summary>
/// Controls the visibility of a <see cref="RemoteAdminOptionBase"/>.
/// </summary>
public interface IOptionVisibilityController {

    /// <summary>
    /// Determines whether the option is visible to the specified sender.
    /// </summary>
    /// <param name="sender">The sender to check.</param>
    /// <returns>Whether the option should be shown.</returns>
    bool IsVisibleTo(CommandSender sender);

}
