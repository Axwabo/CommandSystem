namespace Axwabo.CommandSystem.Attributes.Targeting.Interfaces;

/// <summary>
/// Supplies a static affected one player message.
/// </summary>
public interface IStaticAffectedOnePlayerMessage
{

    /// <summary>
    /// Gets the "affected one player" message.
    /// </summary>
    string AffectedOne { get; }

}
