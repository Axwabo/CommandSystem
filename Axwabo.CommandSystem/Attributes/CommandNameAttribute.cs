using System;
using Axwabo.CommandSystem.Attributes.Interfaces;

namespace Axwabo.CommandSystem.Attributes;

/// <summary>
/// Sets a static name for the command.
/// </summary>
[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public sealed class CommandNameAttribute : Attribute, ICommandName
{

    /// <inheritdoc />
    public string Name { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="CommandNameAttribute"/> class.
    /// </summary>
    /// <param name="name">The name of the command.</param>
    public CommandNameAttribute(string name) => Name = !string.IsNullOrEmpty(name) ? name : throw new ArgumentNullException(name);

}
