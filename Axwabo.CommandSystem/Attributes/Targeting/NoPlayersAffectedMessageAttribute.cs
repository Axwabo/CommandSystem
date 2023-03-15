using System;
using Axwabo.CommandSystem.Attributes.Targeting.Interfaces;

namespace Axwabo.CommandSystem.Attributes.Targeting;

/// <summary>
/// Supplies a static no players affected message.
/// </summary>
[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public sealed class NoPlayersAffectedMessageAttribute : Attribute, IStaticNoPlayersAffectedMessage
{

    /// <inheritdoc />
    public string NoPlayersAffected { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="NoPlayersAffectedMessageAttribute"/> class.
    /// </summary>
    /// <param name="message">The message.</param>
    public NoPlayersAffectedMessageAttribute(string message) => NoPlayersAffected = message;

}
