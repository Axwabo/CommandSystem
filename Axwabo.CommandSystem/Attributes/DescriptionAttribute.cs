using System;
using Axwabo.CommandSystem.Attributes.Interfaces;

namespace Axwabo.CommandSystem.Attributes;

/// <summary>
/// Sets a static description for the command.
/// </summary>
[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public sealed class DescriptionAttribute : Attribute, IDescription
{

    /// <inheritdoc />
    public string Description { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="DescriptionAttribute"/> class.
    /// </summary>
    /// <param name="description">The description of the command.</param>
    public DescriptionAttribute(string description) => Description = description;

}
