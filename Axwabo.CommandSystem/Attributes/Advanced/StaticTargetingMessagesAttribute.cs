using System;
using Axwabo.CommandSystem.Attributes.Advanced.Interfaces;

namespace Axwabo.CommandSystem.Attributes.Advanced;

/// <summary>
/// Supplies static targeting messages.
/// </summary>
[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public sealed class StaticTargetingMessagesAttribute : Attribute, IStaticNoTargetsFoundMessage, IStaticAffectedOnePlayerMessage, IStaticAffectedMultiplePlayersMessage, IStaticNoPlayersAffectedMessage {

    /// <inheritdoc />
    public string NoTargets { get; init; }

    /// <inheritdoc />
    public string AffectedOne { get; init; }

    /// <inheritdoc />
    public string AffectedMultiple { get; init; }

    /// <inheritdoc />
    public string NoPlayersAffected { get; init; }

    /// <summary>
    /// Creates a new instance of the <see cref="StaticTargetingMessagesAttribute"/> class.
    /// </summary>
    public StaticTargetingMessagesAttribute() {
    }

    /// <summary>
    /// Creates a new instance of the <see cref="StaticTargetingMessagesAttribute"/> class with all messages.
    /// </summary>
    /// <param name="noTargets">The "no targets were found" message.</param>
    /// <param name="affectedOne">The "affected one player" message.</param>
    /// <param name="affectedMultiple">The "affected multiple players" message.</param>
    /// <param name="noPlayersAffected">The "no players affected" message.</param>
    public StaticTargetingMessagesAttribute(string noTargets, string affectedOne, string affectedMultiple, string noPlayersAffected) {
        NoTargets = noTargets;
        AffectedOne = affectedOne;
        AffectedMultiple = affectedMultiple;
        NoPlayersAffected = noPlayersAffected;
    }

    /// <summary>
    /// Creates a new instance of the <see cref="StaticTargetingMessagesAttribute"/> class with the affected messages.
    /// </summary>
    /// <param name="affectedOne">The "affected one player" message.</param>
    /// <param name="affectedMultiple">The "affected multiple players" message.</param>
    public StaticTargetingMessagesAttribute(string affectedOne, string affectedMultiple) {
        AffectedOne = affectedOne;
        AffectedMultiple = affectedMultiple;
    }

}
