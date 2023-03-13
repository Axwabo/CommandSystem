namespace Axwabo.CommandSystem.Attributes.Interfaces;

/// <summary>
/// Specifies that the attribute returns a description for the command.
/// </summary>
public interface IDescription
{

    /// <summary>
    /// Gets the description of the command.
    /// </summary>
    string Description { get; }

}
