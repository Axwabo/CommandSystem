namespace Axwabo.CommandSystem.Attributes.Interfaces;

/// <summary>
/// Sets a static display text for the option.
/// </summary>
public interface IStaticOptionText
{

    /// <summary>
    /// The text to display for all users.
    /// </summary>
    string Text { get; }

}
