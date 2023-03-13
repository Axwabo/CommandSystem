namespace Axwabo.CommandSystem.Commands.Interfaces;

/// <summary>
/// Specifies that the command may be hidden from the command list.
/// </summary>
public interface IHiddenCommand
{

    /// <summary>
    /// Gets a value indicating whether the command is hidden.
    /// </summary>
    bool IsHidden { get; }

}
