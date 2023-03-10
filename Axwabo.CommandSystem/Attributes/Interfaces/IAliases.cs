namespace Axwabo.CommandSystem.Attributes.Interfaces;

/// <summary>
/// Specifies that the attribute define aliases of a command.
/// </summary>
public interface IAliases {

    /// <summary>
    /// Gets the aliases for the command.
    /// </summary>
    string[] Aliases { get; }

}
