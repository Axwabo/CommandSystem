using System;
using System.Reflection;
using Axwabo.CommandSystem.Attributes.Advanced.Interfaces;
using Axwabo.CommandSystem.Commands;
using Axwabo.CommandSystem.Commands.MessageOverrides;

namespace Axwabo.CommandSystem.PropertyManager;

public static class TargetingCommandPropertyManager {

    public static void ResolveProperties(CommandBase command,
        ref string noTargetsFound,
        ref string affectedMultipleMessage,
        ref string affectedOneMessage,
        ref string noPlayersAffected,
        ref bool shouldAffectSpectators) {
        BaseCommandPropertyManager.ValidateRegistration(command);
        foreach (var attribute in command.GetType().GetCustomAttributes())
            ResolveBaseAttribute(attribute, ref noTargetsFound, ref affectedMultipleMessage, ref affectedOneMessage, ref noPlayersAffected, ref shouldAffectSpectators);
    }

    private static void ResolveBaseAttribute(Attribute attribute,
        ref string noTargetsFound,
        ref string affectedMultipleMessage,
        ref string affectedOneMessage,
        ref string noPlayersAffected,
        ref bool shouldAffectSpectators) {
        if (attribute is IStaticNoTargetsFoundMessage noTargets)
            noTargets.NoTargets.SetFieldIfNotNull(ref noTargetsFound);

        if (attribute is IStaticAffectedMultiplePlayersMessage affected)
            affected.AffectedMultiple.SetFieldIfNotNull(ref affectedMultipleMessage);

        if (attribute is IStaticAffectedOnePlayerMessage affectedOne)
            affectedOne.AffectedOne.SetFieldIfNotNull(ref affectedOneMessage);

        if (attribute is IStaticNoPlayersAffectedMessage noPlayers)
            noPlayers.NoPlayersAffected.SetFieldIfNotNull(ref noPlayersAffected);

        if (attribute is IShouldAffectSpectators affectSpectators)
            shouldAffectSpectators = affectSpectators.AffectSpectators;
    }

    public static void ResolveGenerators(UnifiedTargetingCommand command,
        out IAffectedMultiplePlayersMessageGenerator affectedMultiple,
        out IAffectedAllPlayersGenerator affectedAll,
        out IAffectedOnePlayerMessageGenerator affectedOne,
        out ITargetSelectionManager selectionManager) {
        if (command == null)
            throw new ArgumentNullException(nameof(command));
        affectedMultiple = null;
        affectedAll = null;
        affectedOne = null;
        selectionManager = null;
        foreach (var attribute in command.GetType().GetCustomAttributes()) {
            var type = attribute.GetType();
            ResolveMultipleAffected(type, attribute, ref affectedMultiple);
            ResolveSingleAffected(type, attribute, ref affectedOne);
            ResolveAllAffected(type, attribute, ref affectedAll);
            ResolveSelectionManager(type, attribute, ref selectionManager);
        }

        command.SafeCastAndSetIfNotNull(ref affectedMultiple);
        command.SafeCastAndSetIfNotNull(ref affectedAll);
        command.SafeCastAndSetIfNotNull(ref affectedOne);
        command.SafeCastAndSetIfNotNull(ref selectionManager);
    }

    private static void ResolveMultipleAffected(Type type, Attribute attribute, ref IAffectedMultiplePlayersMessageGenerator affectedMultipleMessage) {
        foreach (var resolver in BaseCommandPropertyManager.CurrentProcessor.TargetingMultipleMessageResolvers)
            if (resolver.Takes(type))
                affectedMultipleMessage = resolver.Resolve(attribute);
    }

    private static void ResolveSingleAffected(Type type, Attribute attribute, ref IAffectedOnePlayerMessageGenerator affectedSingle) {
        foreach (var resolver in BaseCommandPropertyManager.CurrentProcessor.TargetingSingleMessageResolvers)
            if (resolver.Takes(type))
                affectedSingle = resolver.Resolve(attribute);
    }

    private static void ResolveAllAffected(Type type, Attribute attribute, ref IAffectedAllPlayersGenerator affectedAll) {
        foreach (var resolver in BaseCommandPropertyManager.CurrentProcessor.TargetingAllMessageResolvers)
            if (resolver.Takes(type))
                affectedAll = resolver.Resolve(attribute);
    }

    private static void ResolveSelectionManager(Type type, Attribute attribute, ref ITargetSelectionManager selectionManager) {
        foreach (var resolver in BaseCommandPropertyManager.CurrentProcessor.TargetSelectionManagerResolvers)
            if (resolver.Takes(type))
                selectionManager = resolver.Resolve(attribute);
    }

}
