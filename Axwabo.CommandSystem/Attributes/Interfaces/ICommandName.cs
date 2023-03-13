namespace Axwabo.CommandSystem.Attributes.Interfaces;

/// <summary>
/// Specifies that the attribute returns the name of the command.
/// </summary>
public interface ICommandName
{

    /// <summary>
    /// Gets the name of the command.
    /// </summary>
    string Name { get; }

}
