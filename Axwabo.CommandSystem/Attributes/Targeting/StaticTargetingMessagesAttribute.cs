using System;
using Axwabo.CommandSystem.Attributes.Targeting.Interfaces;

namespace Axwabo.CommandSystem.Attributes.Targeting;

/// <summary>
/// Supplies static targeting messages. This attribute is inherited.
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public sealed class StaticTargetingMessagesAttribute : Attribute,
    IStaticNoTargetsFoundMessage,
    IStaticAffectedOnePlayerMessage,
    IStaticAffectedMultiplePlayersMessage,
    IStaticNoPlayersAffectedMessage
{

    /// <inheritdoc />
    public string NoTargets { get; init; }

    /// <inheritdoc />
    public string AffectedOne { get; init; }

    /// <inheritdoc />
    public string AffectedMultiple { get; init; }

    /// <inheritdoc />
    public string NoPlayersAffected { get; init; }

    /// <summary>
    /// Initializes an instance of the <see cref="StaticTargetingMessagesAttribute"/> class.
    /// </summary>
    public StaticTargetingMessagesAttribute()
    {
    }

    /// <summary>
    /// Initializes an instance of the <see cref="StaticTargetingMessagesAttribute"/> class with all messages.
    /// </summary>
    /// <param name="noTargets">The "no targets were found" message.</param>
    /// <param name="affectedOne">The "affected one player" message.</param>
    /// <param name="affectedMultiple">The "affected multiple players" message.</param>
    /// <param name="noPlayersAffected">The "no players affected" message.</param>
    public StaticTargetingMessagesAttribute(string noTargets, string affectedOne, string affectedMultiple, string noPlayersAffected)
    {
        NoTargets = noTargets;
        AffectedOne = affectedOne;
        AffectedMultiple = affectedMultiple;
        NoPlayersAffected = noPlayersAffected;
    }

    /// <summary>
    /// Initializes an instance of the <see cref="StaticTargetingMessagesAttribute"/> class with the affected messages.
    /// </summary>
    /// <param name="affectedOne">The "affected one player" message.</param>
    /// <param name="affectedMultiple">The "affected multiple players" message.</param>
    public StaticTargetingMessagesAttribute(string affectedOne, string affectedMultiple)
    {
        AffectedOne = affectedOne;
        AffectedMultiple = affectedMultiple;
    }

}
