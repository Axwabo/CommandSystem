namespace Axwabo.CommandSystem.Attributes.Interfaces;

/// <summary>
/// Specifies that the attribute returns an identifier for the Remote Admin option.
/// </summary>
public interface IRemoteAdminOptionIdentifier
{

    /// <summary>
    /// Gets the identifier of the Remote Admin option.
    /// </summary>
    string Id { get; }

}
