using System;
using Axwabo.CommandSystem.Attributes.Advanced.Interfaces;

namespace Axwabo.CommandSystem.Attributes.Advanced;

[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public sealed class StaticTargetingMessagesAttribute : Attribute, IStaticNoTargetsFoundMessage, IStaticAffectedOnePlayerMessage, IStaticAffectedMultiplePlayersMessage, IStaticNoPlayersAffectedMessage {

    public string NoTargets { get; init; }

    public string AffectedOne { get; init; }

    public string AffectedMultiple { get; init; }

    public string NoPlayersAffected { get; init; }

    public StaticTargetingMessagesAttribute() {
    }

    public StaticTargetingMessagesAttribute(string noTargets, string affectedOne, string affectedMultiple, string noPlayersAffected) {
        NoTargets = noTargets;
        AffectedOne = affectedOne;
        AffectedMultiple = affectedMultiple;
        NoPlayersAffected = noPlayersAffected;
    }

    public StaticTargetingMessagesAttribute(string affectedOne, string affectedMultiple) {
        AffectedOne = affectedOne;
        AffectedMultiple = affectedMultiple;
    }

}
