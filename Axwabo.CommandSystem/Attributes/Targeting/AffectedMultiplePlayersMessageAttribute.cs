using System;
using Axwabo.CommandSystem.Attributes.Targeting.Interfaces;

namespace Axwabo.CommandSystem.Attributes.Targeting;

/// <summary>
/// Supplies a static affected multiple players message.
/// </summary>
[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public sealed class AffectedMultiplePlayersMessageAttribute : Attribute, IStaticAffectedMultiplePlayersMessage
{

    /// <inheritdoc />
    public string AffectedMultiple { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="AffectedMultiplePlayersMessageAttribute"/> class.
    /// </summary>
    /// <param name="message">The message.</param>
    public AffectedMultiplePlayersMessageAttribute(string message) => AffectedMultiple = message;

}
