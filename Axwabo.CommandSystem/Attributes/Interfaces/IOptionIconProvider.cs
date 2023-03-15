using Axwabo.CommandSystem.RemoteAdminExtensions;

namespace Axwabo.CommandSystem.Attributes.Interfaces;

/// <summary>
/// An interface to provide a Remote Admin option icon.
/// </summary>
public interface IOptionIconProvider
{

    /// <summary>
    /// Creates a new icon.
    /// </summary>
    /// <returns>A new icon instance.</returns>
    BlinkingIcon CreateIcon();

}
