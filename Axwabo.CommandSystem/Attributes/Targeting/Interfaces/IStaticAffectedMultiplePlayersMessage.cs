namespace Axwabo.CommandSystem.Attributes.Targeting.Interfaces;

/// <summary>
/// Supplies a static affected multiple players message.
/// </summary>
public interface IStaticAffectedMultiplePlayersMessage
{

    /// <summary>
    /// Gets the "affected multiple players" message.
    /// </summary>
    string AffectedMultiple { get; }

}
