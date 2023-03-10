namespace Axwabo.CommandSystem.Attributes.Advanced.Interfaces;

/// <summary>
/// Controls whether the command should affect spectators.
/// </summary>
public interface IShouldAffectSpectators {

    /// <summary>
    /// Gets a value indicating whether the command should affect spectators.
    /// </summary>
    bool AffectSpectators { get; }

}
