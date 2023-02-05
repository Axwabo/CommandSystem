using System;
using System.Reflection;
using Axwabo.CommandSystem.Attributes.Advanced.Interfaces;
using Axwabo.CommandSystem.Commands.MessageOverrides;

namespace Axwabo.CommandSystem.PropertyManager;

public static class TargetingCommandPropertyManager {

    public static void ResolveProperties(CommandBase command, ref string noTargetsFound, ref string affectedMultipleMessage, ref string affectedSingleMessage, ref string noPlayersAffected, ref bool shouldAffectSpectators) {
        BaseCommandPropertyManager.ValidateRegistration(command);
        foreach (var attribute in command.GetType().GetCustomAttributes())
            ResolveBaseAttribute(attribute, ref noTargetsFound, ref affectedMultipleMessage, ref affectedSingleMessage, ref noPlayersAffected, ref shouldAffectSpectators);
    }

    private static void ResolveBaseAttribute(Attribute attribute, ref string noTargetsFound, ref string affectedMultipleMessage, ref string affectedSingleMessage, ref string noPlayersAffected, ref bool shouldAffectSpectators) {
        if (attribute is IStaticNoTargetsFoundMessage noTargets)
            noTargets.Message.SetFieldIfNotNull(ref noTargetsFound);

        if (attribute is IStaticAffectedMultiplePlayersMessage affected)
            affected.Message.SetFieldIfNotNull(ref affectedMultipleMessage);

        if (attribute is IStaticAffectedOnePlayerMessage affectedOne)
            affectedOne.Message.SetFieldIfNotNull(ref affectedSingleMessage);

        if (attribute is IStaticNoPlayersAffectedMessage noPlayers)
            noPlayers.Message.SetFieldIfNotNull(ref noPlayersAffected);

        if (attribute is IShouldAffectSpectators affectSpectators)
            shouldAffectSpectators = affectSpectators.AffectSpectators;
    }

    // ReSharper disable once SuspiciousTypeConversion.Global
    public static void ResolveGenerators(CommandBase command, out IAffectedMultiplePlayersMessageGenerator affectedMultiple) {
        if (command == null)
            throw new ArgumentNullException(nameof(command));
        affectedMultiple = command as IAffectedMultiplePlayersMessageGenerator;
        foreach (var attribute in command.GetType().GetCustomAttributes()) {
            var type = attribute.GetType();
            ResolveMultipleAffected(type, attribute, ref affectedMultiple);
        }
    }

    private static void ResolveMultipleAffected(Type type, Attribute attribute, ref IAffectedMultiplePlayersMessageGenerator affectedMultipleMessage) {
        foreach (var resolver in BaseCommandPropertyManager.CurrentProcessor.TargetingMultipleMessageResolvers)
            if (resolver.Takes(type))
                affectedMultipleMessage = resolver.Resolve(attribute);
    }

}
