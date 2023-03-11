using System;
using System.Reflection;
using Axwabo.CommandSystem.Attributes.Advanced.Interfaces;
using Axwabo.CommandSystem.Commands;
using Axwabo.CommandSystem.Commands.MessageOverrides;
using Axwabo.CommandSystem.Registration;

namespace Axwabo.CommandSystem.PropertyManager;

/// <summary>Attribute to property handler for targeting commands.</summary>
public static class TargetingCommandPropertyManager {

    public static void ResolveProperties(CommandBase command,
        ref string noTargetsFound,
        ref string affectedMultipleMessage,
        ref string affectedOneMessage,
        ref string noPlayersAffected,
        ref bool shouldAffectSpectators) {
        if (command == null)
            throw new ArgumentNullException(nameof(command));
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

    private static bool ProcNull(out CommandRegistrationProcessor proc) {
        proc = BaseCommandPropertyManager.CurrentProcessor;
        return proc == null;
    }

    private static void ResolveMultipleAffected(Type type, Attribute attribute, ref IAffectedMultiplePlayersMessageGenerator affectedMultipleMessage) {
        if (attribute is IAffectedMultiplePlayersMessageGenerator generator) {
            affectedMultipleMessage = generator;
            return;
        }

        if (ProcNull(out var proc))
            return;
        foreach (var resolver in proc.TargetingMultipleMessageResolvers)
            if (resolver.Takes(type))
                affectedMultipleMessage = resolver.Resolve(attribute);
    }

    private static void ResolveSingleAffected(Type type, Attribute attribute, ref IAffectedOnePlayerMessageGenerator affectedSingle) {
        if (ProcNull(out var proc))
            return;
        foreach (var resolver in proc.TargetingSingleMessageResolvers)
            if (resolver.Takes(type))
                affectedSingle = resolver.Resolve(attribute);
    }

    private static void ResolveAllAffected(Type type, Attribute attribute, ref IAffectedAllPlayersGenerator affectedAll) {
        if (ProcNull(out var proc))
            return;
        foreach (var resolver in proc.TargetingAllMessageResolvers)
            if (resolver.Takes(type))
                affectedAll = resolver.Resolve(attribute);
    }

    private static void ResolveSelectionManager(Type type, Attribute attribute, ref ITargetSelectionManager selectionManager) {
        if (ProcNull(out var proc))
            return;
        foreach (var resolver in proc.TargetSelectionManagerResolvers)
            if (resolver.Takes(type))
                selectionManager = resolver.Resolve(attribute);
    }

}
