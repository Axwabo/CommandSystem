namespace Axwabo.CommandSystem.Attributes.Interfaces;

/// <summary>
/// Specifies that the attribute returns usages of the command.
/// </summary>
/// <remarks>Usage strings should not contain the command name.</remarks>
public interface IUsage
{

    /// <summary>
    /// Gets the usages of the command.
    /// </summary>
    string[] Usage { get; }

}
