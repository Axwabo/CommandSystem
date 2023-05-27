using Axwabo.CommandSystem.Attributes.Interfaces;

namespace Axwabo.CommandSystem.Attributes;

/// <summary>
/// Attribute to set the minimum number of arguments required to execute the command.
/// </summary>
[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public sealed class MinArgumentsAttribute : Attribute, IMinArguments
{

    /// <inheritdoc />
    public int MinArguments { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="MinArgumentsAttribute"/> class.
    /// </summary>
    /// <param name="minArguments">The number of arguments.</param>
    public MinArgumentsAttribute(int minArguments) => MinArguments = minArguments;

}
