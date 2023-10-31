namespace Axwabo.CommandSystem.RemoteAdminExtensions.Interfaces;

/// <summary>
/// Specifies that the attribute controls whether the RA option can be used as a standalone selector.
/// </summary>
public interface IStandaloneSelectorOption
{

    /// <summary>Whether the RA option can be used as a standalone selector.</summary>
    bool CanBeUsedAsStandaloneSelector { get; }

}
