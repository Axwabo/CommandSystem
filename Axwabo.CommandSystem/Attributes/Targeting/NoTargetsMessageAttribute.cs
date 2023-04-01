using System;
using Axwabo.CommandSystem.Attributes.Targeting.Interfaces;

namespace Axwabo.CommandSystem.Attributes.Targeting;

/// <summary>
/// Supplies a static no targets found message. This attribute is inherited.
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public sealed class NoTargetsMessageAttribute : Attribute, IStaticNoTargetsFoundMessage
{

    /// <inheritdoc />
    public string NoTargets { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="NoTargetsMessageAttribute"/> class.
    /// </summary>
    /// <param name="message">The message.</param>
    public NoTargetsMessageAttribute(string message) => NoTargets = message;

}
