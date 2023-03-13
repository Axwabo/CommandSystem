namespace Axwabo.CommandSystem.Attributes.Interfaces;

/// <summary>
/// Interface for attributes that define a number of arguments required for a command.
/// </summary>
public interface IMinArguments
{

    /// <summary>
    /// Gets the minimum number of arguments.
    /// </summary>
    int MinArguments { get; }

}
